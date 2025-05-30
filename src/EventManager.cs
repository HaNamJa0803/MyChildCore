using System;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Characters;

namespace MyChildCore.Utilities
{
    /// <summary>
    /// 자녀 외형 동기화 및 이벤트 매니저 (유니크 칠드런 방식)
    /// - 외형 동기화는 무조건 "전체 자녀 일괄 적용"
    /// - 외부: DropdownConfig, AppearanceManager 등에 의존
    /// </summary>
    public static class EventManager
    {
        // GMCM 연동 및 동기화 이벤트 등록
        public static void RegisterEvents(IModHelper helper)
        {
            helper.Events.GameLoop.SaveLoaded   += OnSaveLoaded;
            helper.Events.GameLoop.DayStarted   += OnDayStarted;
            helper.Events.Player.Warped         += OnWarped;
            helper.Events.Display.MenuChanged   += OnMenuChanged;
            helper.Events.GameLoop.Saved        += OnSaved;
        }

        // 아래 콜백들은 항상 "DropdownConfig.Instance"를 가져다 씀
        public static void OnSaveLoaded(object sender, SaveLoadedEventArgs e)   => SyncAllChildrenAppearance(DropdownConfigInstance());
        public static void OnDayStarted(object sender, DayStartedEventArgs e)   => SyncAllChildrenAppearance(DropdownConfigInstance());
        public static void OnWarped(object sender, WarpedEventArgs e)           => SyncAllChildrenAppearance(DropdownConfigInstance());
        public static void OnMenuChanged(object sender, MenuChangedEventArgs e) => SyncAllChildrenAppearance(DropdownConfigInstance());
        public static void OnSaved(object sender, SavedEventArgs e)             => SyncAllChildrenAppearance(DropdownConfigInstance());

        // 현재 DropdownConfig를 안전하게 불러오는 래퍼(싱글턴/DI 구조면 그에 맞게 수정)
        private static DropdownConfig DropdownConfigInstance()
        {
            // 팀장님 프로젝트에서 싱글턴/DI로 구성 시 이 메서드를 바꾸면 됩니다.
            // 여기선 전역 인스턴스 가정. 필요 시 직접 참조 변경.
            return GlobalDropdownConfig.Instance;
        }

        /// <summary>
        /// 유니크 칠드런 스타일: 모든 자녀에게 config 적용 (성별별 파츠 분기 하드코딩)
        /// </summary>
        public static void SyncAllChildrenAppearance(DropdownConfig config)
        {
            foreach (var child in ChildManager.GetAllChildren())
            {
                string spouseName = AppearanceManager.GetSpouseName(child);
                bool isMale = ((int)child.Gender == 0);

                // 배우자/성별 파츠 옵션 안전하게 추출(없으면 기본값)
                if (!config.SpouseConfigs.TryGetValue(spouseName, out var spouseConfig) || spouseConfig == null)
                    spouseConfig = new SpouseChildConfig();

                // 남아: 헤어 Short 고정, 하의 Pants, 신발/넥칼라/잠옷 별도
                // 여아: 헤어 선택, 하의 Skirt, 신발/넥칼라/잠옷 별도
                string hairStyle = isMale ? "Short" : (spouseConfig.GirlHairStyle ?? "CherryTwin");
                string bottom    = isMale ? spouseConfig.BoyPants ?? "Pants_01" : spouseConfig.GirlSkirt ?? "Skirt_01";
                string shoes     = isMale ? spouseConfig.BoyShoes ?? "Shoes_01" : spouseConfig.GirlShoes ?? "Shoes_01";
                string neck      = isMale ? spouseConfig.BoyNeckCollar ?? "NeckCollar_01" : spouseConfig.GirlNeckCollar ?? "NeckCollar_01";
                string pajama    = isMale ? spouseConfig.BoyPajamaStyle ?? "Frog" : spouseConfig.GirlPajamaStyle ?? "Frog";
                int pajamaColor  = isMale ? spouseConfig.BoyPajamaColorIndex : spouseConfig.GirlPajamaColorIndex;
                if (pajamaColor < 1) pajamaColor = 1;

                // 외형 적용 (실제 파츠 반영 로직은 AppearanceManager로 위임)
                AppearanceManager.ApplyToddlerParts(
                    child,
                    isMale,
                    hairStyle,
                    ToIndex(bottom),
                    ToIndex(shoes),
                    ToIndex(neck),
                    pajama,
                    pajamaColor
                );
            }
        }

        // 파츠명(예: "Shoes_03") → int 변환
        private static int ToIndex(string partName)
        {
            if (string.IsNullOrEmpty(partName)) return 1;
            var num = new string(partName.Where(char.IsDigit).ToArray());
            if (int.TryParse(num, out int idx)) return idx;
            return 1;
        }
    }
}