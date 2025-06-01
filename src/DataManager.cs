using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using StardewValley.Characters;

namespace MyChildCore.Utilities
{
    // 단일 커스텀 로그
    public static class CustomLogger
    {
        private static string LogDir =>
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "MyChildCoreLogs");
        private static string LogPath =>
            Path.Combine(LogDir, $"Log_{DateTime.Now:yyyyMMdd}.txt");
        private static bool _enabled = true;

        public static void Info(string msg)  => Write($"[INFO] {msg}");
        public static void Warn(string msg)  => Write($"[WARN] {msg}");
        public static void Error(string msg) => Write($"[ERROR] {msg}");
        private static void Write(string msg)
        {
            if (!_enabled) return;
            var line = $"[{DateTime.Now:HH:mm:ss}] {msg}";
            try
            {
                Directory.CreateDirectory(LogDir);
                File.AppendAllText(LogPath, line + Environment.NewLine);
            }
            catch { }
#if DEBUG
            Console.WriteLine(line);
#endif
        }
    }

    // 자녀 데이터 관리(저장/불러오기/백업/실시간 외형)
    public static class DataManager
    {
        private static string StoragePath =>
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "MyChildData.json");
        private static string BackupDir =>
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "MyChildDataBackup");

        public static List<ChildData> Cache => CacheManager.GetChildCache();

        // 외부 저장소에서 로드 → 캐시로 반영
        public static List<ChildData> LoadChildrenData(string path = null)
        {
            path ??= StoragePath;
            if (!File.Exists(path)) return new List<ChildData>();
            try
            {
                var json = File.ReadAllText(path);
                var list = JsonConvert.DeserializeObject<List<ChildData>>(json) ?? new List<ChildData>();
                CacheManager.SetChildCache(list);
                CustomLogger.Info($"자녀 데이터 로드 완료 ({list.Count}명): {path}");
                return list;
            }
            catch (Exception ex)
            {
                CustomLogger.Error($"Load 실패: {ex.Message}");
                return new List<ChildData>();
            }
        }

        // 캐시 → 저장소로 저장
        public static void SaveChildrenData(List<ChildData> data = null, string path = null)
        {
            path ??= StoragePath;
            data ??= Cache;
            try
            {
                var json = JsonConvert.SerializeObject(data, Formatting.Indented);
                File.WriteAllText(path, json);
                CustomLogger.Info($"자녀 데이터 저장 완료: {path}");
            }
            catch (Exception ex)
            {
                CustomLogger.Error($"Save 실패: {ex.Message}");
            }
        }

        // 즉시 백업 (시간명시)
        public static void BackupNow()
        {
            Directory.CreateDirectory(BackupDir);
            var backupPath = Path.Combine(BackupDir, $"MyChildData_{DateTime.Now:yyyyMMdd_HHmmss}.json");
            SaveChildrenData(null, backupPath);
            CustomLogger.Info($"백업 생성: {backupPath}");
        }

        // 최신 백업 복원 (캐시/저장소 동기화)
        public static bool RestoreLatestBackup()
        {
            if (!Directory.Exists(BackupDir)) return false;
            var files = Directory.GetFiles(BackupDir, "MyChildData_*.json").OrderByDescending(f => f).ToList();
            if (files.Count == 0) return false;
            var data = LoadChildrenData(files.First());
            CacheManager.SetChildCache(data);
            SaveChildrenData(); // 주 저장소로 복원
            CustomLogger.Warn($"최신 백업 복원: {files.First()}");
            return true;
        }

        // 디스크 → 캐시 동기화
        public static void SyncFromDisk()
        {
            CacheManager.SyncFromDisk(StoragePath);
            CustomLogger.Info($"캐시 디스크 동기화 완료");
        }

        // 일괄 외형 적용
        public static void ApplyAllAppearances(DropdownConfig config)
        {
            foreach (var child in ChildManager.GetAllChildren())
            {
                if (child.Age >= 1)
                {
                    var parts = PartsManager.GetPartsForChild(child, config);
                    if (parts != null)
                        AppearanceManager.ApplyToddlerAppearance(child, parts);
                }
                else
                {
                    var spouseName = AppearanceManager.GetSpouseName(child);
                    AppearanceManager.ApplyBabyAppearance(child, spouseName);
                }
            }
            CustomLogger.Info("모든 자녀 외형 적용 완료");
        }

        // 개별 외형 적용
        public static bool ApplyAppearanceTo(string name, long parentId, DropdownConfig config)
        {
            var child = ChildManager.GetAllChildren().FirstOrDefault(c =>
                c.Name == name && c.idOfParent?.Value == parentId);
            if (child == null)
            {
                CustomLogger.Warn($"ApplyAppearanceTo: 대상 없음 ({name}/{parentId})");
                return false;
            }
            if (child.Age >= 1)
            {
                var parts = PartsManager.GetPartsForChild(child, config);
                if (parts == null) return false;
                AppearanceManager.ApplyToddlerAppearance(child, parts);
            }
            else
            {
                var spouseName = AppearanceManager.GetSpouseName(child);
                AppearanceManager.ApplyBabyAppearance(child, spouseName);
            }
            CustomLogger.Info($"외형 적용: {name}/{parentId}");
            return true;
        }

        // 키워드 검색(이름/헤어/잠옷)
        public static List<ChildData> Search(string keyword)
        {
            if (string.IsNullOrEmpty(keyword)) return Cache;
            keyword = keyword.ToLowerInvariant();
            return Cache.Where(c =>
                (c.Name != null && c.Name.ToLowerInvariant().Contains(keyword)) ||
                (c.HairKey != null && c.HairKey.ToLowerInvariant().Contains(keyword)) ||
                (c.PajamaStyle != null && c.PajamaStyle.ToLowerInvariant().Contains(keyword))
            ).ToList();
        }

        // 요약 문자열
        public static string GetSummary()
        {
            var list = Cache ?? new List<ChildData>();
            int total = list.Count;
            int babies = list.Count(c => c.Age == 0);
            int toddlers = list.Count(c => c.Age == 1);
            int boys = list.Count(c => c.Gender == 0);
            int girls = list.Count(c => c.Gender == 1);
            int dups = list.GroupBy(c => c.Name).Count(g => g.Count() > 1);
            return $"총 {total}명 (아기:{babies}, 유아:{toddlers}, 남:{boys}, 여:{girls}, 이름중복:{dups})";
        }
    }
}