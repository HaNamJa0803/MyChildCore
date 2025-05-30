using System.Collections.Generic;
using System.Linq;
using StardewValley;
using StardewValley.Characters;

namespace MyChildCore.Utilities
{
    /// <summary>
    /// 게임 내 자녀(Child) 인스턴스 조회/필터 전용 매니저
    /// - 메모리/캐시/저장소 접근은 포함하지 않음
    /// </summary>
    public static class ChildManager
    {
        /// <summary>
        /// 현재 게임 내 모든 Child 리스트 반환
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
        /// 내 첫번째 자녀 반환(없으면 null)
        /// </summary>
        public static Child GetMyFirstChild()
            => GetChildrenByPlayer().FirstOrDefault();

        /// <summary>
        /// 성별로 필터 (isMale==true: 남자, false: 여자)
        /// </summary>
        public static List<Child> FilterByGender(bool isMale)
            => GetAllChildren().Where(c => (int)c.Gender == (isMale ? 0 : 1)).ToList();

        /// <summary>
        /// 부모 이름(혹은 spouseKey)로 필터
        /// </summary>
        public static List<Child> FilterBySpouseName(string spouseName)
            => GetAllChildren().Where(child =>
            {
                string spouse = AppearanceManager.GetSpouseName(child);
                return spouse == spouseName;
            }).ToList();

        // ※ Child의 외형 파츠 속성(헤어, 스커트 등)으로의 필터는
        // "DataManager" 등에서 ChildData와 매칭해서 처리하세요.
    }
}