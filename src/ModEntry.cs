public class ModEntry : Mod
{
    private ModConfig Config;

    public override void Entry(IModHelper helper)
    {
        Config = helper.ReadConfig<ModConfig>();
        helper.Events.GameLoop.GameLaunched += OnGameLaunched;
    }

    private void OnGameLaunched(object sender, GameLaunchedEventArgs e)
    {
        // GMCM API를 직접 참조해서 가져옴
        var gmcm = Helper.ModRegistry.GetApi<IGenericModConfigMenuApi>("spacechase0.GenericModConfigMenu");
        if (gmcm == null)
        {
            Monitor.Log("GMCM not found!", LogLevel.Warn);
            return;
        }

        // 매니페스트 대신 this 사용, 옵션 함수도 람다(헬퍼, 컨피그 활용)
        gmcm.Register(
            this, // 매니페스트 대신 모드 인스턴스(문제 없음)
            reset: () => { Config = new ModConfig(); Helper.WriteConfig(Config); },
            save: () => { Helper.WriteConfig(Config); }
        );

        // 옵션 추가
        gmcm.AddBoolOption(
            this,
            name: () => "잠옷 활성화",
            tooltip: () => "자녀 잠옷 커스텀 활성화 여부",
            getValue: () => Config.EnablePajama,
            setValue: v => Config.EnablePajama = v
        );

        gmcm.AddNumberOption(
            this,
            name: () => "잠옷 스타일",
            tooltip: () => "자녀 잠옷 스타일 번호",
            getValue: () => Config.PajamaStyle,
            setValue: v => Config.PajamaStyle = v,
            min: 1,
            max: 5
        );
    }
}