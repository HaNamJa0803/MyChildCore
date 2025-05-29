using System;
using System.Collections.Generic;
using System.Linq;
using StardewValley;
using StardewValley.Characters;

namespace MyChildCore.Utilities
{
    /// <summary>
    /// 자녀 관리 유틸리티 (1.6.10+ 실전/파츠/성능/확장성)
    /// ─────────────────────────────────────────────
    /// - 성능 위주: 모든 연산, 캐싱 없이 매번 전체 순회! (누락 0%)
    /// - 파츠 시스템 전면 대응: 모든 파츠 기준 필터 구조(최신 구조 반영)
    /// </summary>
    public static class ChildManager
    {
        /// <summary>
        /// (성능) 게임 내 존재하는 모든 Child 리스트 반환
        /// </summary>
        public static List<Child> GetAllChildren()
            => Utility.getAllCharacters()?.OfType<Child>()?.ToList() ?? new List<Child>();

        /// <summary>
        /// (성능) 현재 플레이어의 모든 자녀만 반환 (입양/쌍둥이/멀티/동명이인 전부 대응)
        /// </summary>
        public static List<Child> GetChildrenByPlayer()
        {
            if (Game1.player == null)
                return new List<Child>();
            var playerId = Game1.player.UniqueMultiplayerID;
            return GetAllChildren().Where(c => c.idOfParent?.Value == playerId).ToList();
        }

        /// <summary>
        /// (성능) 내 첫 번째 자녀 (없으면 null)
        /// </summary>
        public static Child GetMyFirstChild()
            => GetChildrenByPlayer().FirstOrDefault();

        /// <summary>
        /// (성능) 성별 필터 (isMale: true=남, false=여)
        /// </summary>
        public static List<Child> FilterByGender(bool isMale)
            => GetChildrenByPlayer().Where(c => (int)c.Gender == (isMale ? 0 : 1)).ToList();

        /// <summary>
        /// (확장) 파츠 키(최신 파츠 구조: 모자/헤어/눈/스킨/상의/하의/넥칼라/신발/의상/잠옷 등) 기준 자녀 필터링
        /// - null 파라미터는 조건 무시
        /// - 추후 파츠 더 늘어나도 파라미터만 추가하면 구조 변경 불필요
        /// </summary>
        public static List<Child> FilterByParts(
            string? hatKey = null,
            string? hairKey = null,
            string? eyeKey = null,
            string? skinKey = null,
            string? topKey = null,
            string? bottomKey = null,
            string? neckCollarKey = null,
            string? shoesKey = null,
            string? clothesKey = null,
            string? pajamaStyle = null,
            string? pajamaColor = null
        )
        {
            // 모든 파츠 기준 필터 (아무거나 null이면 조건 무시)
            return GetChildrenByPlayer().Where(child =>
            {
                // ChildData 연동 필요: 실제 Child → ChildData 변환 로직 별도 필요
                var data = DataManager.LoadChildrenData()
                    .Find(d => d.Name == child.Name && d.ParentID == child.idOfParent.Value);

                if (data == null) return false;
                bool cond = true;
                if (!string.IsNullOrEmpty(hatKey)) cond &= (data.HatKey == hatKey);
                if (!string.IsNullOrEmpty(hairKey)) cond &= (data.HairKey == hairKey);
                if (!string.IsNullOrEmpty(eyeKey)) cond &= (data.EyeKey == eyeKey);
                if (!string.IsNullOrEmpty(skinKey)) cond &= (data.SkinKey == skinKey);
                if (!string.IsNullOrEmpty(topKey)) cond &= (data.TopKey == topKey);
                if (!string.IsNullOrEmpty(bottomKey)) cond &= (data.BottomKey == bottomKey);
                if (!string.IsNullOrEmpty(neckCollarKey)) cond &= (data.NeckCollarKey == neckCollarKey);
                if (!string.IsNullOrEmpty(shoesKey)) cond &= (data.ShoesKey == shoesKey);
                if (!string.IsNullOrEmpty(clothesKey)) cond &= (data.ClothesKey == clothesKey);
                if (!string.IsNullOrEmpty(pajamaStyle)) cond &= (data.PajamaStyle == pajamaStyle);
                if (!string.IsNullOrEmpty(pajamaColor)) cond &= (data.PajamaColor == pajamaColor);
                return cond;
            }).ToList();
        }
    }
}