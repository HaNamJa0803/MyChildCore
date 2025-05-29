using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace MyChildCore.Utilities
{
    /// <summary>
    /// 임시 데이터/연산 결과 캐싱 유틸리티 (SDV 1.6.10+ 실전형)
    /// - 예: 자녀 전체 리스트, JSON 결과, 계산식 등 반복 연산 방지용
    /// </summary>
    public static class CacheManager
    {
        private static readonly Dictionary<string, object> _cache = new();
        private static string? _lastSavePath = null;

        /// <summary>
        /// 캐시에 데이터 저장/갱신
        /// </summary>
        public static void Set<T>(string key, T value)
        {
            if (key == null) return;
            _cache[key] = value!;
        }

        /// <summary>
        /// 캐시에서 데이터 조회. 없으면 default(T) 반환
        /// </summary>
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

        /// <summary>
        /// 캐시 항목 삭제
        /// </summary>
        public static void Remove(string key)
        {
            if (key != null)
                _cache.Remove(key);
        }

        /// <summary>
        /// 모든 캐시 비우기
        /// </summary>
        public static void Clear()
        {
            _cache.Clear();
        }

        /// <summary>
        /// 캐시 전체를 파일로 저장 (JSON 직렬화)
        /// </summary>
        public static void SaveCacheToFile(string path)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(path)) throw new ArgumentException("경로 없음");
                var json = JsonConvert.SerializeObject(_cache, Formatting.Indented);
                // 백업 기능: 덮어쓰기 전 기존 파일 백업
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

        /// <summary>
        /// 캐시를 파일에서 복구 (JSON 역직렬화)
        /// </summary>
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

        /// <summary>
        /// 가장 최근 저장된 파일로 즉시 복구 (백업/자동화용)
        /// </summary>
        public static void RestoreFromLastSave()
        {
            if (_lastSavePath != null)
                LoadCacheFromFile(_lastSavePath);
        }

        /// <summary>
        /// 파일/메모리 일관성 검사 (테스트/디버그용)
        /// </summary>
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