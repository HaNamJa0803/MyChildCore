using System;
using System.Collections.Generic;

namespace MyChildCore.Utilities
{
    /// <summary>
    /// 반복적으로 사용되는 데이터를 캐싱(임시저장)하여 성능 향상 및 중복 연산 방지.
    /// (예: 자녀 전체 리스트, 필터 결과 등)
    /// </summary>
    public static class CacheManager
    {
        private static readonly Dictionary<string, object> _cache = new Dictionary<string, object>();

        public static void Set<T>(string key, T value)
        {
            _cache[key] = value;
        }

        public static T Get<T>(string key)
        {
            return _cache.TryGetValue(key, out var value) && value is T tValue ? tValue : default;
        }

        public static void Remove(string key)
        {
            _cache.Remove(key);
        }

        public static void Clear()
        {
            _cache.Clear();
        }
    }
}