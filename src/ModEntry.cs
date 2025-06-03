using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using MyChildCore;
using System;

namespace MyChildCore
{
    public class ModEntry : Mod
    {
        public static DropdownConfig Config;
        private int lastSyncedDay = -1;

        public override void Entry(IModHelper helper)
        {
            Config = new DropdownConfig();

            helper.Events.GameLoop.GameLaunched += OnGameLaunched;
            helper.Events.GameLoop.SaveLoaded += OnSaveLoaded;
            helper.Events.GameLoop.DayStarted += OnDayStarted;
            helper.Events.Player.Warped += OnWarped;
            helper.Events.Display.MenuChanged += OnMenuChanged;
            helper.Events.GameLoop.Saved += OnSaved;
            helper.Events.GameLoop.OneSecondUpdateTicked += OnOneSecondUpdateTicked;

            helper.ConsoleCommands.Add("applychild", "모든 자녀 외형 일괄 적용", ApplyAllChildren);
            helper.ConsoleCommands.Add("backupchild", "자녀 데이터 백업", BackupChildren);
            helper.ConsoleCommands.Add("restorechild", "자녀 백업 복구", RestoreChildren);
        }

        private void OnGameLaunched(object sender, GameLaunchedEventArgs e)
        {
            var gmcm = Helper.ModRegistry.GetApi("spacechase0.GenericModConfigMenu");
            if (gmcm != null)
            {
                var gmcmType = gmcm.GetType();
                var register = gmcmType.GetMethod("Register");
                if (register != null)
                    register.Invoke(gmcm, new object[] { this, (Action)(() => Config = new DropdownConfig()), (Action)(() => { }), false });

                GMCMSectionBuilder.BuildSections_Dynamic(gmcm, this, Helper.Translation, Config, OnGMCMChanged);
            }
        }

        private void OnSaveLoaded(object sender, SaveLoadedEventArgs e) => DataManager.ApplyAllAppearances(Config);
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
        private void OnSaved(object sender, SavedEventArgs e) => DataManager.ApplyAllAppearances(Config);

        private void OnOneSecondUpdateTicked(object sender, EventArgs e)
        {
            if (Game1.Date.TotalDays != lastSyncedDay)
            {
                DataManager.SyncFromDisk();
                lastSyncedDay = Game1.Date.TotalDays;
                CustomLogger.Info("외부 저장소와 동기화 완료");
            }
        }

        public static void OnGMCMChanged(string spouse, bool isMale)
        {
            AppearanceManager.ApplyForGMCMChange(spouse, isMale, Config);
            CustomLogger.Info($"GMCM 변경: {spouse}, {(isMale ? "남" : "여")} 즉시 반영");
        }

        private void ApplyAllChildren(string cmd, string[] args)
        {
            DataManager.ApplyAllAppearances(Config);
            CustomLogger.Info("모든 자녀 외형 일괄 적용 완료!");
        }
        private void BackupChildren(string cmd, string[] args)
        {
            DataManager.Backup();
            CustomLogger.Info("자녀 데이터 백업 완료!");
        }
        private void RestoreChildren(string cmd, string[] args)
        {
            DataManager.RestoreLatestBackup();
            DataManager.ApplyAllAppearances(Config);
            CustomLogger.Info("자녀 데이터 백업 복구 및 적용 완료!");
        }
    }
}
