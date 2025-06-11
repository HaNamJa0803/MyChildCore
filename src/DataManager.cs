using MyChildCore;
using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using StardewValley;
using StardewValley.Characters;
using StardewModdingAPI;

namespace MyChildCore
{
    /// <summary>
    /// 자녀 데이터 외부저장소/캐시/외형 동기화 매니저 (게임 데이터 연동 보완)
    /// </summary>
    public static class DataManager
    {
        // 반드시 Helper 등록! (Entry에서 DataManager.Init(helper) 호출)
        private static IModHelper Helper;
        public static void Init(IModHelper helper) => Helper = helper;

        // === 세이브별 데이터 폴더 (SMAPI 권장 방식)
        private static string SaveDir
        {
            get
            {
                var folder = Constants.SaveFolderName;
                if (string.IsNullOrEmpty(folder)) folder = "Unknown";
                // "Mods/모드이름/data/플레이어명_숫자/"
                return Path.Combine(Helper.DirectoryPath, "data", folder);
            }
        }
        public static string DataPath => Path.Combine(SaveDir, "MyChildData.json");

        public static event Action<List<ChildData>> OnDataChanged;

        // 1. 실제 게임 내 자녀 객체 → 데이터 반환 (없으면 외부저장소)
        public static List<ChildData> LoadData()
        {
            var children = ChildManager.GetAllChildren();
            if (children.Count > 0)
            {
                var list = new List<ChildData>();
                foreach (var child in children)
                    list.Add(ChildData.FromChild(child));

                CacheManager.SyncWithData(list);
                NotifyDataChanged(list);
                return list;
            }

            // 2. 외부저장소(보험) 읽기 (SMAPI-safe)
            if (Helper.Data.ReadJsonFile<List<ChildData>>(DataPath) is List<ChildData> backup && backup.Count > 0)
            {
                CacheManager.SyncWithData(backup);
                NotifyDataChanged(backup);
                return backup;
            }

            // 3. 아무것도 없으면 빈 리스트
            var empty = new List<ChildData>();
            CacheManager.SyncWithData(empty);
            NotifyDataChanged(empty);
            return empty;
        }

        // 캐시 → 외부저장소(보험) 저장 (실제 게임 내 자녀는 SMAPI가 저장)
        public static void SaveData(List<ChildData> data)
        {
            try
            {
                Directory.CreateDirectory(SaveDir);
                Helper.Data.WriteJsonFile(DataPath, data); // SMAPI-safe 저장!
                CacheManager.SetChildCache(data);
                NotifyDataChanged(data);
            }
            catch (Exception ex)
            {
                CustomLogger.Error($"[DataManager] SaveData 예외: {ex.Message}");
            }
        }

        // 외부저장소 → 캐시 동기화 (수동/긴급 복구)
        public static void SyncFromDisk()
        {
            if (Helper.Data.ReadJsonFile<List<ChildData>>(DataPath) is List<ChildData> backup && backup.Count > 0)
            {
                CacheManager.SyncWithData(backup);
                NotifyDataChanged(backup);
            }
            else
            {
                var empty = new List<ChildData>();
                CacheManager.SyncWithData(empty);
                NotifyDataChanged(empty);
            }
        }

        // === 백업/복구 ===
        public static void Backup()
        {
            try
            {
                var data = LoadData();
                string backupPath = DataPath + ".bak";
                Helper.Data.WriteJsonFile(backupPath, data);
                CustomLogger.Info("[DataManager] 백업 완료");
            }
            catch (Exception ex)
            {
                CustomLogger.Error($"[DataManager] Backup 예외: {ex.Message}");
            }
        }

        public static void RestoreLatestBackup()
        {
            string backupPath = DataPath + ".bak";
            try
            {
                if (Helper.Data.ReadJsonFile<List<ChildData>>(backupPath) is List<ChildData> backupData && backupData != null)
                {
                    SaveData(backupData);
                    CacheManager.SyncWithData(backupData);
                    NotifyDataChanged(backupData);
                }
            }
            catch (Exception ex)
            {
                CustomLogger.Error($"[DataManager] RestoreLatestBackup 예외: {ex.Message}");
            }
        }

        // === 자녀 전체 외형 적용 (항상 컨피그 기준으로!) ===
        public static void ApplyAllAppearances(ModConfig config)
        {
            if (config == null || !config.EnableMod) return;
            foreach (var child in ChildManager.GetAllChildren())
            {
                if (child == null) continue;
                var parts = (child.Age >= 1)
                    ? PartsManager.GetPartsForChild(child, config)
                    : PartsManager.GetPartsForBaby(child, config);
                if (parts == null)
                    parts = PartsManager.GetDefaultParts(child, child.Age == 0);

                try
                {
                    if (child.Age >= 1)
                        AppearanceManager.ApplyToddlerAppearance(child, parts);
                    else
                        AppearanceManager.ApplyBabyAppearance(child, parts);

                    ResourceManager.InvalidateChildSprite(child.Name);
                }
                catch (Exception ex)
                {
                    CustomLogger.Warn($"[DataManager] {child.Name} 외형 적용 예외: {ex.Message}");
                }
            }
            NotifyDataChanged(CacheManager.GetChildCache());
        }

        // === GMCMKey별 외형 적용 (컨피그/GMCM 수동적용 가능) ===
        public static void ApplyAppearancesByGMCMKey(ModConfig config)
        {
            if (config == null || !config.EnableMod) return;
            foreach (var child in ChildManager.GetAllChildren())
            {
                if (child == null) continue;
                string gmcmKey = GMCMKeyUtil.GetChildKey(child);
                if (config.SpouseConfigs != null && config.SpouseConfigs.ContainsKey(gmcmKey))
                {
                    var parts = (child.Age >= 1)
                        ? PartsManager.GetPartsForChild(child, config)
                        : PartsManager.GetPartsForBaby(child, config);
                    if (parts == null)
                        parts = PartsManager.GetDefaultParts(child, child.Age == 0);

                    try
                    {
                        if (child.Age >= 1)
                            AppearanceManager.ApplyToddlerAppearance(child, parts);
                        else
                            AppearanceManager.ApplyBabyAppearance(child, parts);

                        ResourceManager.InvalidateChildSprite(child.Name);
                    }
                    catch (Exception ex)
                    {
                        CustomLogger.Warn($"[DataManager] {child.Name} 외형 적용 예외: {ex.Message}");
                    }
                }
            }
            NotifyDataChanged(CacheManager.GetChildCache());
        }

        private static void NotifyDataChanged(List<ChildData> data)
        {
            try { OnDataChanged?.Invoke(data); }
            catch (Exception ex) { CustomLogger.Warn($"[DataManager] OnDataChanged 알림 예외: {ex.Message}"); }
        }
    }
}