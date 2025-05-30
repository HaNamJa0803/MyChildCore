using System;
using System.Collections.Generic;
using System.Linq;
using StardewValley;
using StardewValley.Characters;
using MyChildCore.Utilities;

namespace MyChildCore.Utilities
{
    public static class ChildManager
    {
        // 1. 모든 Child 리스트 반환
        public static List<Child> GetAllChildren()
            => Utility.getAllCharacters()?.OfType<Child>()?.ToList() ?? new List<Child>();

        // 2. 현재 플레이어(호스트)의 모든 자녀 반환
        public static List<Child> GetChildrenByPlayer()
        {
            if (Game1.player == null)
                return new List<Child>();
            var playerId = Game1.player.UniqueMultiplayerID;
            return GetAllChildren().Where(c => c.idOfParent?.Value == playerId).ToList();
        }

        // 3. 내 첫번째 자녀 (없으면 null)
        public static Child GetMyFirstChild()
            => GetChildrenByPlayer().FirstOrDefault();

        // 4. 성별 필터 (true=남, false=여)
        public static List<Child> FilterByGender(bool isMale)
            => GetAllChildren().Where(c => (int)c.Gender == (isMale ? 0 : 1)).ToList();

        // 5. 파츠 키 기반 필터 (DataManager/CacheManager 연동)
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
            var allData = DataManager.LoadChildrenData();
            return GetAllChildren().Where(child =>
            {
                var data = allData.Find(d => d.Name == child.Name && d.ParentID == child.idOfParent.Value);
                if (data == null) return false;
                bool cond = true;
                if (!string.IsNullOrEmpty(spouseName))  cond &= (AppearanceManager.GetSpouseName(child) == spouseName);
                if (!string.IsNullOrEmpty(hairKey))     cond &= (data.HairKey == hairKey);
                if (!string.IsNullOrEmpty(eyeKey))      cond &= (data.EyeKey == eyeKey);
                if (!string.IsNullOrEmpty(skinKey))     cond &= (data.SkinKey == skinKey);
                if (!string.IsNullOrEmpty(topKey))      cond &= (data.TopKey == topKey);
                if (!string.IsNullOrEmpty(bottomKey))   cond &= (data.BottomKey == bottomKey);
                if (!string.IsNullOrEmpty(shoesKey))    cond &= (data.ShoesKey == shoesKey);
                if (!string.IsNullOrEmpty(neckKey))     cond &= (data.NeckKey == neckKey);
                if (!string.IsNullOrEmpty(pajamaStyle)) cond &= (data.PajamaStyle == pajamaStyle);
                if (pajamaColorIndex.HasValue)          cond &= (data.PajamaColorIndex == pajamaColorIndex.Value);
                return cond;
            }).ToList();
        }
    }
}