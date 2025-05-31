using System.Linq;
using System;
using System.IO;
using StardewValley;
using StardewValley.Characters;

namespace MyChildCore.Utilities
{
    /// <summary>
    /// 자녀 외형(스프라이트) 적용 전담 매니저
    /// </summary>
    public static class AppearanceManager
    {
        /// <summary>
        /// 유아(Child) 외형 적용
        /// </summary>
        public static void ApplyToddlerAppearance(Child child, ChildParts parts)
        {
            if (child == null || parts == null) return;

            // 시즌별 숏/롱 상의 경로 결정
            string season = Utility.getSeasonNameFromNumber(Game1.seasonIndex).ToLower();
            string topType = "Short";
            switch (season)
            {
                case "spring":
                case "summer":
                    topType = "Short";
                    break;
                case "fall":
                case "autumn":
                case "winter":
                    topType = "Long";
                    break;
            }
            string topPath = parts.IsMale
                ? $"assets/Top/Top_Male_{topType}.png"
                : $"assets/Top/Top_Female_{topType}.png";

            // 하의, 신발, 넥칼라 경로
            string bottomPath = parts.IsMale
                ? $"assets/Bottom/Pants/Pants_{parts.PantsKey}.png"
                : $"assets/Bottom/Skirt/Skirt_{parts.SkirtKey}.png";
            string shoesPath = $"assets/Shoes/Shoes_{parts.ShoesKey}.png";
            string neckPath = $"assets/Neck/Neck_{parts.NeckKey}.png";

            // 헤어, 눈, 피부 경로
            string spouseName = parts.SpouseName;
            string hairPath = $"assets/{spouseName}/Toddler/Hair/{spouseName}_Toddler_{parts.HairKey}.png";
            string eyePath = $"assets/{spouseName}/Toddler/Eye/{spouseName}_Toddler_Eye.png";
            string skinPath = $"assets/{spouseName}/Toddler/Skin/{spouseName}_Toddler_Skin.png";

            // 축제복 경로 (남/여 공용 상의, 모자, 넥칼라까지 계절별 적용)
            string festivalKey = parts.IsMale ? "Male" : "Female";
            string festivalTop = "", festivalHat = "", festivalNeck = "";

            // [수정1] isFestival을 메서드로 호출
            if (Game1.isFestival())
            {
                // 봄: 모자만
                if (season == "spring")
                {
                    festivalHat = $"assets/Festival/Spring/FestivalHat_{festivalKey}_Spring.png";
                    ApplyIfExist(child, festivalHat, shoesPath);
                    return;
                }
                // 여름: 상의 + 모자
                else if (season == "summer")
                {
                    festivalTop = $"assets/Festival/Summer/FestivalTop_{festivalKey}_Summer.png";
                    festivalHat = $"assets/Festival/Summer/FestivalHat_{festivalKey}_Summer.png";
                    ApplyIfExist(child, festivalTop, festivalHat, shoesPath);
                    return;
                }
                // 가을: 상의만
                else if (season == "fall" || season == "autumn")
                {
                    festivalTop = $"assets/Festival/Fall/FestivalTop_{festivalKey}_Fall.png";
                    ApplyIfExist(child, festivalTop, shoesPath);
                    return;
                }
                // 겨울: 상의 + 모자 + 넥칼라
                else if (season == "winter")
                {
                    festivalTop = $"assets/Festival/Winter/FestivalTop_{festivalKey}_Winter.png";
                    festivalHat = $"assets/Festival/Winter/FestivalHat_{festivalKey}_Winter.png";
                    festivalNeck = $"assets/Festival/Winter/FestivalNeck_{festivalKey}_Winter.png";
                    ApplyIfExist(child, festivalTop, festivalHat, festivalNeck, shoesPath);
                    return;
                }
            }

            // 잠옷 경로
            if (Game1.timeOfDay >= 1800 || Game1.timeOfDay < 600)
            {
                string pajamaPath = $"assets/Sleep/{parts.PajamaStyle}/{parts.PajamaStyle}_{parts.PajamaColorIndex:D2}.png";
                ApplyIfExist(child, pajamaPath);
                return;
            }

            // 일반 외형 적용 (순서: 헤어 → 상의 → 하의 → 눈 → 피부 → 신발 → 넥칼라)
            ApplyIfExist(child, hairPath, topPath, bottomPath, eyePath, skinPath, shoesPath, neckPath);
        }

        /// <summary>
        /// 아기(Child) 외형 적용 (모든 아기 공용 Body, 눈/스킨/헤어는 배우자 폴더)
        /// </summary>
        public static void ApplyBabyAppearance(Child Child, string spouseName)
        {
            if (Child == null || string.IsNullOrEmpty(spouseName)) return;

            // 공용 바디(의상) 경로
            string bodyPath = "assets/Child/Child_Body.png";
            // 배우자별 헤어, 눈, 스킨 경로
            string hairPath = $"assets/{spouseName}/Child/hair/{spouseName}_Child_hair.png";
            string eyePath = $"assets/{spouseName}/Child/eye/{spouseName}_Child_eye.png";
            string skinPath = $"assets/{spouseName}/Child/skin/{spouseName}_Child_skin.png";

            // 적용 (순서: 바디 → 헤어 → 눈 → 스킨)
            ApplyIfExist(Child, bodyPath, hairPath, eyePath, skinPath);
        }

        // 경로 중 존재하는 파일만 적용
        private static void ApplyIfExist(Character character, params string[] spritePaths)
        {
            foreach (var path in spritePaths)
            {
                if (string.IsNullOrEmpty(path)) continue;
                if (File.Exists(path))
                {
                    character.Sprite = new AnimatedSprite(path, 0, 16, 32);
                    break;
                }
            }
        }

        /// <summary>
        /// 자녀 인스턴스에서 배우자 이름 추출 (SDV 1.6.15 : idOfParent, motherName, fatherName 사용)
        /// </summary>
        public static string GetSpouseName(Child child)
        {
            try
            {
                // 1.6.10+는 spouse 필드 없음 → idOfParent, motherName, fatherName 활용
                // 보통 Player의 이름 또는 idOfParent/Name값 반환
                if (child == null) return "Unknown";

                // 시도1: idOfParent → Farmer 찾기
                long parentId = child.idOfParent?.Value ?? -1;
                if (parentId > 0)
                {
                    var parent = Game1.getAllFarmers().FirstOrDefault(f => f.UniqueMultiplayerID == parentId);
                    if (parent != null && !string.IsNullOrEmpty(parent.Name))
                        return parent.Name;
                }

                // 시도2: motherName, fatherName 필드
                // if (!string.IsNullOrEmpty(child.motherName))
                //     return child.motherName;
                // if (!string.IsNullOrEmpty(child.fatherName))
                //     return child.fatherName;
            }
            catch { }
            return "Unknown";
        }
    }
}