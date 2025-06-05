using System.Linq;
using StardewValley.Characters;
using System;
using System.Collections.Generic;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using GenericModConfigMenu;
using MyChildCore;

namespace MyChildCore
{
    /// <summary>
    /// 자녀-설정 매핑을 위한 GMCM 고유키 생성 유틸
    /// (예시: "부모이름_자녀이름_성별_순번")
    /// </summary>
    public static class GMCMKeyUtil
    {
        /// <summary>
        /// 자녀 고유키 반환 (동명이인, 폴리아모리, 예외 모두 대응)
        /// </summary>
        public static string GetChildKey(Child child)
        {
            if (child == null)
                return "Unknown_NullChild";
            try
            {
                // 부모이름
                string parent = GetParentName(child);
                if (string.IsNullOrWhiteSpace(parent))
                    parent = "UnknownParent";
                // 자녀이름
                string name = string.IsNullOrWhiteSpace(child.Name) ? "UnknownChild" : child.Name;
                // 성별(0=남, 1=여)
                string gender = ((int)child.Gender == 0) ? "M" : "F";
                // 순번(같은 이름+성별 중복 방지)
                int index = GetChildIndex(child);

                return $"{parent}_{name}_{gender}_{index}";
            }
            catch (Exception ex)
            {
                // 예외 발생 시 fallback
                return $"Unknown_Error_{ex.GetType().Name}";
            }
        }

        /// <summary>
        /// 부모(플레이어/배우자) 이름 반환
        /// </summary>
        private static string GetParentName(Child child)
        {
            try
            {
                long parentId = child.idOfParent?.Value ?? -1;
                if (parentId > 0)
                {
                    var farmer = StardewValley.Game1.getAllFarmers()
                        .FirstOrDefault(f => f.UniqueMultiplayerID == parentId);
                    if (farmer != null && !string.IsNullOrEmpty(farmer.Name))
                        return farmer.Name;
                }
            }
            catch { }
            return "UnknownParent";
        }

        /// <summary>
        /// 동일 부모-이름-성별 자녀 중 순번 반환 (동명이인/쌍둥이/폴리아모리 대응)
        /// </summary>
        private static int GetChildIndex(Child child)
        {
            try
            {
                var all = StardewValley.Utility.getAllCharacters();
                if (all == null) return 0;
                int idx = 0;
                foreach (var c in all)
                {
                    if (c is Child other)
                    {
                        bool sameParent = (other.idOfParent?.Value ?? -1) == (child.idOfParent?.Value ?? -2);
                        bool sameName   = other.Name == child.Name;
                        bool sameGender = other.Gender == child.Gender;
                        if (sameParent && sameName && sameGender)
                        {
                            if (ReferenceEquals(other, child))
                                return idx;
                            idx++;
                        }
                    }
                }
            }
            catch { }
            return 0; // 예외 시 0
        }
    }
}