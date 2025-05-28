using System;
using System.Collections.Generic;

namespace MyChildCore.Utilities
{
    /// <summary>
    /// 임시 데이터/연산 결과 캐싱 유틸리티 (SDV 1.6.10+ 실전형)
    /// - 예: 자녀 전체 리스트, JSON 결과, 계산식 등 반복 연산 방지용
    /// </summary>
    public static class CacheManager
    {
        private static readonly Dictionary<string, object> _cache = new();

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
            return _cache.TryGetValue(key, out var value) && value is T tValue ? tValue : default;
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
    }
}