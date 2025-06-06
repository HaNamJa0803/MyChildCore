using StardewModdingAPI;
using StardewModdingAPI.Events;
using GenericModConfigMenu;
using StardewValley;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MyChildCore
{
    public class ModEntry : Mod
    {
        public static ModConfig Config;
        private static int lastSyncedDay = -1;

        public override void Entry(IModHelper helper)
        {
            Config = new ModConfig();
            RegisterGameEvents(helper);
            RegisterConsoleCommands(helper);
            helper.Events.GameLoop.GameLaunched += OnGameLaunched;
        }

        // ────────── GMCM 등록/옵션/커스텀UI 호출 ──────────
        private void OnGameLaunched(object sender, GameLaunchedEventArgs e)
        {
            var gmcm = Helper.ModRegistry.GetApi<IGenericModConfigMenuApi>("spacechase0.GenericModConfigMenu");
            if (gmcm == null)
            {
                Monitor.Log("GMCM 연동 실패! GMCM 설치 필요.", LogLevel.Warn);
                return;
            }
            gmcm.Register(
                mod: ModManifest,
                reset: delegate { Config = new ModConfig(); },
                save: delegate { }
            );
            RegisterGMCMGlobalOptions(gmcm);
            RegisterGMCMSpouseOptions(gmcm);
        }

        // ────────── GMCM 글로벌 옵션 등록 ──────────
        private void RegisterGMCMGlobalOptions(IGenericModConfigMenuApi gmcm)
        {
            gmcm.AddBoolOption(
                mod: ModManifest,
                name: delegate { return "모드 활성화"; },
                tooltip: delegate { return "모드를 켭니다."; },
                getValue: delegate { return Config.EnableMod; },
                setValue: delegate (bool v) { Config.EnableMod = v; }
            );
            gmcm.AddBoolOption(
                mod: ModManifest,
                name: delegate { return "잠옷 활성화"; },
                tooltip: delegate { return "잠옷 기능을 켭니다."; },
                getValue: delegate { return Config.EnablePajama; },
                setValue: delegate (bool v) { Config.EnablePajama = v; }
            );
            gmcm.AddBoolOption(
                mod: ModManifest,
                name: delegate { return "축제복 활성화"; },
                tooltip: delegate { return "축제복 기능을 켭니다."; },
                getValue: delegate { return Config.EnableFestival; },
                setValue: delegate (bool v) { Config.EnableFestival = v; }
            );
            gmcm.AddSectionTitle(
                mod: ModManifest,
                text: delegate { return "──────────────"; },
                tooltip: null
            );
            gmcm.AddSectionTitle(
                mod: ModManifest,
                text: delegate { return "배우자 목록"; },
                tooltip: null
            );
            gmcm.AddSectionTitle(
                mod: ModManifest,
                text: delegate { return "──────────────"; },
                tooltip: null
            );
        }

        // ────────── GMCM 배우자 목록+커스텀UI 이동 옵션 등록 ──────────
        private void RegisterGMCMSpouseOptions(IGenericModConfigMenuApi gmcm)
        {
            string[] spouseArray = DropdownConfig.SpouseNames;
            for (int i = 0; i < spouseArray.Length; i++)
            {
                string spouse = spouseArray[i]; // 지역 변수 캡처
                gmcm.AddComplexOption(
                    mod: ModManifest,
                    name: delegate { return spouse; },
                    tooltip: delegate { return spouse + " 자녀 설정 창으로 이동"; },
                    draw: delegate (SpriteBatch b, Vector2 pos)
                    {
                        // 기본 폰트(색상: 검정/White)로 배우자 이름만 단일 출력
                        b.DrawString(Game1.smallFont, spouse, pos, Color.Black);
                        // 클릭 감지: 이름 클릭 시 커스텀 UI 호출
                        var btnRect = new Rectangle((int)pos.X, (int)pos.Y, 220, 36);
                        if (btnRect.Contains(Game1.getMouseX(), Game1.getMouseY()) &&
                            Game1.input.GetMouseState().LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                        {
                            List<string> spouseList = new List<string>(DropdownConfig.SpouseNames);
                            Game1.playSound("smallSelect");
                            Game1.activeClickableMenu = new MyChildCore.UI.ChildConfigMenu(
                                spouse,
                                OnCustomMenuSave // 엔트리 콜백
                            );
                        }
                    }
                );
            }
        }

        // ────────── 커스텀 UI 콜백: 저장/적용(엔트리 경유 必) ──────────
        public static void OnCustomMenuSave(string spouseName, SpouseChildConfig config, bool isBoy)
        {
            if (Config.SpouseConfigs.ContainsKey(spouseName))
                Config.SpouseConfigs[spouseName] = config;

            // 모든 반영은 반드시 엔트리에서!
            AppearanceManager.ApplyForGMCMChange(spouseName, isBoy, Config);
            DataManager.SaveData(CacheManager.GetChildCache());
            CustomLogger.Info($"[{spouseName}] 자녀 설정 저장 완료");
        }

        // ────────── 콘솔 명령어 등록 ──────────
        private void RegisterConsoleCommands(IModHelper helper)
        {
            helper.ConsoleCommands.Add("applychild", "모든 자녀 외형 일괄 적용", ApplyAllChildren);
            helper.ConsoleCommands.Add("backupchild", "자녀 데이터 백업", BackupChildren);
            helper.ConsoleCommands.Add("restorechild", "자녀 백업 복구", RestoreChildren);
        }

        // ────────── 게임 이벤트 등록 ──────────
        private void RegisterGameEvents(IModHelper helper)
        {
            helper.Events.GameLoop.SaveLoaded   += OnSaveLoaded;
            helper.Events.GameLoop.DayStarted   += OnDayStarted;
            helper.Events.Player.Warped         += OnWarped;
            helper.Events.Display.MenuChanged   += OnMenuChanged;
            helper.Events.GameLoop.Saved        += OnSaved;
            helper.Events.GameLoop.OneSecondUpdateTicked += OnOneSecondUpdateTicked;
        }

        // ────────── GMCM 옵션 변경 콜백(기존 구조와 호환) ──────────
        public static void OnGMCMChanged(string spouse, bool isBoy)
        {
            AppearanceManager.ApplyForGMCMChange(spouse, isBoy, Config);
            DataManager.SaveData(CacheManager.GetChildCache());
        }

        // ────────── 게임 이벤트 처리(외형, 데이터 적용/백업/동기화) ──────────
        private void OnSaveLoaded(object sender, SaveLoadedEventArgs e)
        {
            DataManager.ApplyAllAppearances(Config);
        }
        private void OnDayStarted(object sender, DayStartedEventArgs e)
        {
            DataManager.ApplyAllAppearances(Config);
            lastSyncedDay = Game1.Date.TotalDays;
        }
        private void OnWarped(object sender, WarpedEventArgs e)
        {
            if (Context.IsMainPlayer && e.IsLocalPlayer)
                DataManager.ApplyAllAppearances(Config);
        }
        private void OnMenuChanged(object sender, MenuChangedEventArgs e)
        {
            if (Context.IsWorldReady)
                DataManager.ApplyAllAppearances(Config);
        }
        private void OnSaved(object sender, SavedEventArgs e)
        {
            DataManager.ApplyAllAppearances(Config);
        }
        private void OnOneSecondUpdateTicked(object sender, EventArgs e)
        {
            if (Game1.Date.TotalDays != lastSyncedDay)
            {
                DataManager.SyncFromDisk();
                lastSyncedDay = Game1.Date.TotalDays;
                CustomLogger.Info("외부 저장소와 동기화 완료");
            }
        }

        // ────────── 콘솔 명령어 실행 ──────────
        private static void ApplyAllChildren(string cmd, string[] args)
            => DataManager.ApplyAllAppearances(Config);

        private static void BackupChildren(string cmd, string[] args)
            => DataManager.Backup();

        private static void RestoreChildren(string cmd, string[] args)
        {
            DataManager.RestoreLatestBackup();
            DataManager.ApplyAllAppearances(Config);
        }
    }
}