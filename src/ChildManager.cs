using System;
using System.Collections.Generic;
using System.Linq;
using StardewValley;
using StardewValley.Characters;

namespace MyChildCore.Utilities
{
    /// <summary>
    /// 자녀(Child) 관리 전용 매니저
    /// </summary>
    public static class ChildManager
    {
        /// <summary>
        /// 세이브상 모든 Child 객체 반환 (null 안전)
        /// </summary>
        public static List<Child> GetAllChildren()
            => Utility.getAllCharacters()?.OfType<Child>()?.ToList() ?? new List<Child>();

        /// <summary>
        /// 현재 플레이어(호스트)의 모든 자녀 반환
        /// </summary>
        public static List<Child> GetChildrenByPlayer()
        {
            if (Game1.player == null)
                return new List<Child>();
            var playerId = Game1.player.UniqueMultiplayerID;
            return GetAllChildren().Where(c => c.idOfParent?.Value == playerId).ToList();
        }

        /// <summary>
        /// 현재 플레이어의 첫번째 자녀 (없으면 null)
        /// </summary>
        public static Child GetMyFirstChild()
            => GetChildrenByPlayer().FirstOrDefault();

        /// <summary>
        /// 성별 필터 (isMale: true=남, false=여)
        /// </summary>
        public static List<Child> FilterByGender(bool isMale)
            => GetAllChildren().Where(c => (int)c.Gender == (isMale ? 0 : 1)).ToList();

        /// <summary>
        /// 파츠별로 자녀 필터 (ChildData 정보와 연동 필요)
        /// </summary>
        public static List<Child> FilterByParts(
            string? spouseName = null,
            string? hairKey = null,
            string? eyeKey = null,
            string? skinKey = null,
            string? topKey = null,
            string? bottomKey = null,
            string? shoesKey = null,
            string? neckKey = null,
            string? pajamaStyle = null,
            int? pajamaColorIndex = null)
        {
            // ChildData는 외부 데이터 매니저와 연동 필요 (예시)
            var allData = DataManager.LoadChildrenData();
            return GetAllChildren().Where(child =>
            {
                var data = allData.Find(d => d.Name == child.Name && d.ParentID == child.idOfParent.Value);
                if (data == null) return false;
                bool cond = true;
                if (!string.IsNullOrEmpty(spouseName))  cond &= (AppearanceManager.GetSpouseName(child) == spouseName);
                if (!string.IsNullOrEmpty(hairKey))     cond &= (data.HairStyle == hairKey);
                // eyeKey/skinKey 등은 ChildData 구조에 따라 필요시 추가
                if (!string.IsNullOrEmpty(topKey))      cond &= (data.PajamaStyle == topKey); // 예시
                if (!string.IsNullOrEmpty(bottomKey))   cond &= (data.BottomColorIndex.ToString() == bottomKey); // 예시
                if (!string.IsNullOrEmpty(shoesKey))    cond &= (data.ShoesColorIndex.ToString() == shoesKey); // 예시
                if (!string.IsNullOrEmpty(neckKey))     cond &= (data.NeckColorIndex.ToString() == neckKey); // 예시
                if (!string.IsNullOrEmpty(pajamaStyle)) cond &= (data.PajamaStyle == pajamaStyle);
                if (pajamaColorIndex.HasValue)          cond &= (data.PajamaColorIndex == pajamaColorIndex.Value);
                return cond;
            }).ToList();
        }
    }
}