using System;
using System.Collections.Generic;
using System.IO;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Characters;
using Newtonsoft.Json;

namespace MyChildCore.Utilities
{
    public static class MyChildCoreUtilities
    {
        // 1. 외형 적용 (축제/잠옷/평상복)
        public static void ApplyToddlerParts(
            Child child, bool isMale, string selectedHair, int bottomIndex, int shoesIndex, int neckIndex, string pajamaStyle, int pajamaColorIndex)
        {
            if (child == null) return;
            string spouseName = GetSpouseName(child);

            // Clothes(공용) 폴더 베이스
            string clothesBase = "Clothes/";

            // 축제 분기
            if (Game1.isFestivalDay || Game1.isFestival)
            {
                string season = Utility.getSeasonNameFromNumber(Game1.seasonIndex).ToLower();
                if (season == "spring")
                    ApplyIfExist(child, $"{clothesBase}Festival/Spring/FestivalHat_Spring.png");
                else if (season == "summer")
                    ApplyIfExist(child,
                        $"{clothesBase}Festival/Summer/FestivalTop_Summer.png",
                        $"{clothesBase}Festival/Summer/FestivalHat_Summer.png");
                else if (season == "fall" || season == "autumn")
                    ApplyIfExist(child, $"{clothesBase}Festival/Fall/FestivalTop_Fall.png");
                else if (season == "winter")
                    ApplyIfExist(child,
                        $"{clothesBase}Festival/Winter/FestivalTop_Winter.png",
                        $"{clothesBase}Festival/Winter/FestivalHat_Winter.png",
                        $"{clothesBase}Festival/Winter/FestivalNeck_Winter.png");
                return;
            }

            // 잠옷 분기 (Clothes/Sleep/스타일/스타일_번호.png)
            if (Game1.timeOfDay >= 1800 || Game1.timeOfDay < 600)
            {
                string pajamaFile = $"{pajamaStyle}_{pajamaColorIndex:D2}.png";
                string pajamaPath = $"{clothesBase}Sleep/{pajamaStyle}/{pajamaFile}";
                ApplyIfExist(child, pajamaPath);
                return;
            }

            // 평상복
            string topPath = isMale
                ? $"{clothesBase}Top/Male/Top_Male_{(IsShortTop() ? "Short" : "Long")}.png"
                : $"{clothesBase}Top/Female/Top_Female_{(IsShortTop() ? "Short" : "Long")}.png";

            string bottomPath = isMale
                ? $"{clothesBase}Bottom/Pants/Pants_{bottomIndex:D2}.png"
                : $"{clothesBase}Bottom/Skirt/Skirt_{bottomIndex:D2}.png";

            string shoesPath = $"{clothesBase}Shoes/Shoes_{shoesIndex:D2}.png";
            string neckPath = $"{clothesBase}NeckCollar/NeckCollar_{neckIndex:D2}.png";

            // 배우자별 헤어, 눈, 스킨 경로(폴더/파일명 규칙에 맞게 수정)
            string basePath = $"assets/{spouseName}/Toddler/";
            string hairKey = isMale ? "Short" : selectedHair;

            string hairPath = $"{basePath}Hair/{spouseName}_Toddler_{hairKey}.png";
            string eyePath  = $"{basePath}Eye/{spouseName}_Toddler_Eye.png";
            string skinPath = $"{basePath}Skin/{spouseName}_Toddler_Skin.png";

            // 순서: Hair → Top → Bottom → Eye → Skin → Shoes → Neck
            ApplyIfExist(child,
                hairPath,
                topPath,
                bottomPath,
                eyePath,
                skinPath,
                shoesPath,
                neckPath
            );
        }

        // 계절별 숏/롱 상의 분기
        private static bool IsShortTop()
        {
            var season = Utility.getSeasonNameFromNumber(Game1.seasonIndex).ToLower();
            return season == "spring" || season == "summer";
        }

        // 외형 실제 적용
        private static void ApplyIfExist(Child child, params string[] spritePaths)
        {
            foreach (var path in spritePaths)
            {
                if (File.Exists(path))
                {
                    child.Sprite = new AnimatedSprite(path, 0, 16, 32);
                    break;
                }
            }
        }

        public static string GetSpouseName(Child child)
        {
            try
            {
                if (child?.spouse != null && !string.IsNullOrEmpty(child.spouse.Name)) return child.spouse.Name;
                if (!string.IsNullOrEmpty(child.motherName.Value)) return child.motherName.Value;
                if (!string.IsNullOrEmpty(child.fatherName.Value)) return child.fatherName.Value;
            }
            catch { }
            return "Unknown";
        }
        public static List<Child> GetAllChildren()
            => Utility.getAllCharacters()?.OfType<Child>()?.ToList() ?? new List<Child>();

        // 데이터 저장/불러오기/캐시(캐시 직접 메모리)
        private static List<ChildData> _cache;
        private static string StoragePath => Path.Combine(Constants.CurrentSavePath ?? "", "MyChildData.json");
        public static void SaveChildrenData(List<ChildData> childrenData)
        {
            if (childrenData == null) return;
            File.WriteAllText(StoragePath, JsonConvert.SerializeObject(childrenData, Formatting.Indented));
            _cache = childrenData;
        }
        public static List<ChildData> LoadChildrenData()
        {
            if (_cache != null) return _cache;
            if (!File.Exists(StoragePath)) return new List<ChildData>();
            _cache = JsonConvert.DeserializeObject<List<ChildData>>(File.ReadAllText(StoragePath));
            return _cache;
        }

        // 49개 이벤트 모두 콜백 등록(하드코딩)
        public static void RegisterAllEvents(IModHelper helper, Action syncCallback)
        {
            void OnAnyEvent(object s, EventArgs e) { syncCallback(); }
            helper.Events.GameLoop.SaveLoaded += OnAnyEvent;
            helper.Events.GameLoop.DayStarted += OnAnyEvent;
            helper.Events.GameLoop.DayEnding += OnAnyEvent;
            helper.Events.GameLoop.ReturnedToTitle += OnAnyEvent;
            helper.Events.GameLoop.UpdateTicked += OnAnyEvent;
            helper.Events.GameLoop.OneSecondUpdateTicked += OnAnyEvent;
            helper.Events.GameLoop.TimeChanged += OnAnyEvent;
            helper.Events.GameLoop.Saved += OnAnyEvent;
            helper.Events.GameLoop.Saving += OnAnyEvent;
            helper.Events.GameLoop.GameLaunched += OnAnyEvent;
            helper.Events.GameLoop.UpdateTicking += OnAnyEvent;
            helper.Events.GameLoop.SaveCreated += OnAnyEvent;
            helper.Events.Player.Warped += OnAnyEvent;
            helper.Events.Player.InventoryChanged += OnAnyEvent;
            helper.Events.Player.MoneyChanged += OnAnyEvent;
            helper.Events.Player.LevelChanged += OnAnyEvent;
            helper.Events.Player.HealthChanged += OnAnyEvent;
            helper.Events.Player.EnergyChanged += OnAnyEvent;
            helper.Events.Player.SkillChanged += OnAnyEvent;
            helper.Events.Player.PassiveSkillChanged += OnAnyEvent;
            helper.Events.World.NpcListChanged += OnAnyEvent;
            helper.Events.World.ObjectListChanged += OnAnyEvent;
            helper.Events.World.LocationListChanged += OnAnyEvent;
            helper.Events.World.BuildingsChanged += OnAnyEvent;
            helper.Events.World.DebrisListChanged += OnAnyEvent;
            helper.Events.World.LargeTerrainFeatureListChanged += OnAnyEvent;
            helper.Events.World.ResourceClumpListChanged += OnAnyEvent;
            helper.Events.World.MineChestListChanged += OnAnyEvent;
            helper.Events.World.MineShaftListChanged += OnAnyEvent;
            helper.Events.World.CropsListChanged += OnAnyEvent;
            helper.Events.World.AnimalsChanged += OnAnyEvent;
            helper.Events.Display.MenuChanged += OnAnyEvent;
            helper.Events.Display.RenderedActiveMenu += OnAnyEvent;
            helper.Events.Display.WindowResized += OnAnyEvent;
            helper.Events.Display.RenderedHud += OnAnyEvent;
            helper.Events.Display.RenderingHud += OnAnyEvent;
            helper.Events.Display.RenderingActiveMenu += OnAnyEvent;
            helper.Events.Display.RenderedWorld += OnAnyEvent;
            helper.Events.Display.RenderingWorld += OnAnyEvent;
            helper.Events.Multiplayer.PeerConnected += OnAnyEvent;
            helper.Events.Multiplayer.PeerDisconnected += OnAnyEvent;
            helper.Events.Multiplayer.ModMessageReceived += OnAnyEvent;
            helper.Events.Multiplayer.LobbyChanged += OnAnyEvent;
            helper.Events.Multiplayer.LobbyLeft += OnAnyEvent;
            helper.Events.Input.ButtonPressed += OnAnyEvent;
            helper.Events.Input.ButtonReleased += OnAnyEvent;
            helper.Events.Input.MouseWheelScrolled += OnAnyEvent;
            helper.Events.Input.CursorMoved += OnAnyEvent;
            helper.Events.Input.ButtonHeld += OnAnyEvent;
        }

        // 동기화 진입점 (GMCM에서 DropdownConfig 넘길 때)
public static void SyncAllChildrenAppearance(DropdownConfig config)
{
    foreach (var child in GetAllChildren())
    {
        string spouseName = GetSpouseName(child);
        bool isMale = ((int)child.Gender == 0);

        // 배우자/성별 설정 블록 안전하게 가져오기
        if (!config.SpouseConfigs.TryGetValue(spouseName, out var spouseConfig) || spouseConfig == null)
        {
            CustomLogger.Warn($"[DropdownConfig] '{spouseName}'에 대한 자녀 설정 누락, 기본값 적용");
            spouseConfig = new SpouseChildConfig();
        }

        // 파츠 값(누락시 기본값)
        string hairStyle = isMale ? "Short" : (spouseConfig.GirlHairStyle ?? "CherryTwin");
        string skirt     = spouseConfig.GirlSkirt ?? "Skirt_01";
        string pants     = spouseConfig.BoyPants ?? "Pants_01";
        string shoes     = isMale ? (spouseConfig.BoyShoes ?? "Shoes_01") : (spouseConfig.GirlShoes ?? "Shoes_01");
        string neck      = isMale ? (spouseConfig.BoyNeckCollar ?? "NeckCollar_01") : (spouseConfig.GirlNeckCollar ?? "NeckCollar_01");
        string pajama    = isMale ? (spouseConfig.BoyPajamaStyle ?? "Frog") : (spouseConfig.GirlPajamaStyle ?? "Frog");
        int pajamaColor  = isMale ? spouseConfig.BoyPajamaColorIndex : spouseConfig.GirlPajamaColorIndex;
        if (pajamaColor < 1) pajamaColor = 1;

        // 실제 적용(성별별로 하의/파츠 다르게)
        ApplyToddlerParts(
            child,
            isMale,
            hairStyle,
            isMale ? ToIndex(pants) : ToIndex(skirt),   // 하의 번호 변환
            ToIndex(shoes),                             // 신발 번호 변환
            ToIndex(neck),                              // 넥칼라 번호 변환
            pajama,
            pajamaColor
        );
    }
}

// 파츠 파일명(예: "Shoes_03") → 번호(int) 변환
private static int ToIndex(string partName)
{
    if (string.IsNullOrEmpty(partName)) return 1;
    var num = new string(partName.Where(char.IsDigit).ToArray());
    if (int.TryParse(num, out int idx)) return idx;
    return 1;
}

//
}