using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace MyChildCore.Utilities
{
    public static class CacheManager
    {
        private static readonly Dictionary<string, object> _cache = new();
        private static string? _lastSavePath = null;

        public static void Set<T>(string key, T value)
        {
            if (key == null) return;
            _cache[key] = value!;
        }

        public static T? Get<T>(string key)
        {
            if (key == null) return default;
            try
            {
                if (_cache.TryGetValue(key, out var value) && value is T tValue)
                    return tValue;
            }
            catch (Exception ex)
            {
                CustomLogger.Warn($"[CacheManager] Get<{typeof(T).Name}> 실패: {ex.Message}");
            }
            return default;
        }

        public static void Remove(string key)
        {
            if (key != null)
                _cache.Remove(key);
        }

        public static void Clear()
        {
            _cache.Clear();
        }

        public static void SaveCacheToFile(string path)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(path)) throw new ArgumentException("경로 없음");
                var json = JsonConvert.SerializeObject(_cache, Formatting.Indented);

                if (File.Exists(path))
                {
                    var backupPath = path + ".bak";
                    File.Copy(path, backupPath, overwrite: true);
                    CustomLogger.Info($"[CacheManager] 기존 캐시 백업: {backupPath}");
                }

                File.WriteAllText(path, json);
                _lastSavePath = path;
                CustomLogger.Info($"[CacheManager] 캐시 저장 완료: {path}");
            }
            catch (Exception ex)
            {
                CustomLogger.Error($"[CacheManager] 캐시 저장 실패: {ex.Message}");
            }
        }

        public static void LoadCacheFromFile(string path)
        {
            try
            {
                if (!File.Exists(path)) throw new FileNotFoundException(path);
                string json = File.ReadAllText(path);
                var dict = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
                if (dict != null)
                {
                    _cache.Clear();
                    foreach (var kv in dict)
                        _cache[kv.Key] = kv.Value!;
                    CustomLogger.Info($"[CacheManager] 캐시 파일 복구: {path} (항목: {dict.Count})");
                }
            }
            catch (Exception ex)
            {
                CustomLogger.Error($"[CacheManager] 캐시 복구 실패: {ex.Message}");
            }
        }

        public static void RestoreFromLastSave()
        {
            if (_lastSavePath != null)
                LoadCacheFromFile(_lastSavePath);
        }

        public static bool CheckConsistencyWithFile(string path)
        {
            if (!File.Exists(path)) return false;
            try
            {
                string json = File.ReadAllText(path);
                var fileDict = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
                if (fileDict == null) return false;

                bool isSame = fileDict.Count == _cache.Count;
                if (!isSame)
                    CustomLogger.Warn($"[CacheManager] 파일-메모리 캐시 항목수 다름 ({fileDict.Count} vs {_cache.Count})");

                return isSame;
            }
            catch (Exception ex)
            {
                CustomLogger.Warn($"[CacheManager] 일관성 검사 실패: {ex.Message}");
                return false;
            }
        }
    }
}