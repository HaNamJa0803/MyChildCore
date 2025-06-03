using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using StardewValley;
using StardewValley.Characters;
using MyChildCore;

namespace MyChildCore
{
    public static class DataManager
    {
        private static string SaveDir => Path.Combine("Saves", Game1.player?.Name ?? "Unknown");
        private static string DataPath => Path.Combine(SaveDir, "MyChildData.json");

        /// <summary>
        /// 저장 파일에서 자녀 데이터 모두 로드
        /// </summary>
        public static List<ChildData> LoadData()
        {
            if (!File.Exists(DataPath)) return new();
            return JsonConvert.DeserializeObject<List<ChildData>>(File.ReadAllText(DataPath)) ?? new();
        }

        /// <summary>
        /// 자녀 데이터를 파일로 저장
        /// </summary>
        public static void SaveData(List<ChildData> data)
        {
            Directory.CreateDirectory(SaveDir);
            File.WriteAllText(DataPath, JsonConvert.SerializeObject(data, Formatting.Indented));
        }

        /// <summary>
        /// 외부 저장소 → 캐시 동기화
        /// </summary>
        public static void AutoSyncCache()
        {
            var data = LoadData();
            CacheManager.SyncWithData(data);
        }

        /// <summary>
        /// 캐시 → 외부 저장소로 저장
        /// </summary>
        public static void ManualSaveFromCache()
        {
            SaveData(CacheManager.GetChildCache());
        }

        /// <summary>
        /// 현재 데이터 백업 (.bak)
        /// </summary>
        public static void Backup()
        {
            var data = LoadData();
            File.WriteAllText(DataPath + ".bak", JsonConvert.SerializeObject(data, Formatting.Indented));
        }

        /// <summary>
        /// 외부 저장소(디스크)에서 데이터 읽어와 캐시 동기화
        /// </summary>
        public static void SyncFromDisk()
        {
            var data = LoadData();
            CacheManager.SyncWithData(data);
        }

        /// <summary>
        /// 마지막 백업에서 데이터 복원 (캐시까지 동기화)
        /// </summary>
        public static void RestoreLatestBackup()
        {
            string backupPath = DataPath + ".bak";
            if (File.Exists(backupPath))
            {
                var backupData = JsonConvert.DeserializeObject<List<ChildData>>(File.ReadAllText(backupPath));
                if (backupData != null)
                {
                    SaveData(backupData);
                    CacheManager.SyncWithData(backupData);
                }
            }
        }

        /// <summary>
        /// 자녀 전체 외형 일괄 적용 (기본 방식, GMCM 전체 반영)
        /// </summary>
        public static void ApplyAllAppearances(DropdownConfig config)
        {
            foreach (var child in ChildManager.GetAllChildren())
            {
                if (child.Age >= 1)
                {
                    var parts = PartsManager.GetPartsForChild(child, config);
                    if (parts != null)
                        AppearanceManager.ApplyToddlerAppearance(child, parts, config);
                }
                else
                {
                    var babyParts = PartsManager.GetPartsForBaby(child, config);
                    if (babyParts != null)
                        AppearanceManager.ApplyBabyAppearance(child, babyParts);
                }
            }
        }

        /// <summary>
        /// GMCM 커스텀 키 기반 자녀별 외형만 선택 적용 (폴리아모리 등 커스텀 대응)
        /// </summary>
        public static void ApplyAppearancesByGMCMKey(DropdownConfig config)
        {
            foreach (var child in ChildManager.GetAllChildren())
            {
                string gmcmKey = GMCMKeyUtil.GetChildKey(child);
                if (config.SpouseConfigs.TryGetValue(gmcmKey, out var spouseConfig))
                {
                    var parts = PartsManager.GetPartsForChild(child, config);
                    if (parts != null)
                        AppearanceManager.ApplyToddlerAppearance(child, parts, config);
                }
            }
        }
    }
}