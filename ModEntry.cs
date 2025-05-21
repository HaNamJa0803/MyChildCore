using System;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;

namespace MyChildCore
{
    public class ModEntry : Mod
    {
        public static ModEntry Instance;

        public override void Entry(IModHelper helper)
        {
            Instance = this;
            helper.Events.GameLoop.GameLaunched += OnGameLaunched;
            helper.Events.GameLoop.SaveLoaded += OnSaveLoaded;
        }

        private void OnGameLaunched(object sender, GameLaunchedEventArgs e)
        {
            ModConfigManager.Register();
        }

        private void OnSaveLoaded(object sender, SaveLoadedEventArgs e)
        {
            ChildAppearanceApplier.Apply();
        }
    }
}
