using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using MyChildCore.Utilities;

namespace MyChildCore.Utilities
{
    /// <summary>
    /// SDV 1.6.10+ 단일 프로세스 최적화 캐시 매니저 (멀티 프로세스 락 불필요)
    /// - 파일 저장/불러오기는 try-catch로 예외만 커버
    /// - 모든 데이터는 한 프로세스, 한 인스턴스 내에서만 접근
    /// </summary>
    public static class CacheManager
    {
        private static Dictionary<string, object> _cache = new();
        private static string? _lastSavePath = null;

        /// <summary>
        /// 캐시 데이터 저장
        /// </summary>
        public static void Set<T>(string key, T value)
        {
            if (key == null) return;
            _cache[key] = value!;
        }

        /// <summary>
        /// 캐시에서 데이터 조회 (JObject/JToken 자동 변환 지원)
        /// </summary>
        public static T? Get<T>(string key)
        {
            if (key == null) return default;
            if (_cache.TryGetValue(key, out var value))
            {
                if (value is T tValue)
                    return tValue;
                if (value is JObject jObj)
                    return jObj.ToObject<T>();
                if (value is JToken jToken)
                    return jToken.ToObject<T>();
            }
            return default;
        }

        /// <summary>
        /// 캐시 전체 파일로 저장 (예외만 커버)
        /// </summary>
        public static void SaveCacheToFile(string path)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(path)) return;
                var json = JsonConvert.SerializeObject(_cache, Formatting.Indented);
                File.WriteAllText(path, json);
                _lastSavePath = path;
            }
            catch (Exception ex)
            {
                CustomLogger.Warn($"[CacheManager] 캐시 저장 실패: {ex.Message}");
            }
        }

        /// <summary>
        /// 캐시 파일에서 복구 (예외만 커버)
        /// </summary>
        public static void LoadCacheFromFile(string path)
        {
            try
            {
                if (!File.Exists(path)) return;
                string json = File.ReadAllText(path);
                var dict = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
                if (dict != null)
                {
                    _cache = dict;
                    _lastSavePath = path;
                }
            }
            catch (Exception ex)
            {
                CustomLogger.Warn($"[CacheManager] 캐시 복구 실패: {ex.Message}");
            }
        }

        /// <summary>
        /// 캐시 전체 비우기
        /// </summary>
        public static void Clear()
        {
            _cache.Clear();
        }
    }
}