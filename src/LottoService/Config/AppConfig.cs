namespace LottoService.Config;

public class AppConfig
{
    // default value for the count of draws
    public int NumberOfDraws { get; set; } = 6;

    // LottoNumber default config
    public MinMaxConfig LottoNumber { get; set; } = new MinMaxConfig() { Min = 1, Max = 49 };

    // SuperNumber default config
    public MinMaxConfig SuperNumber { get; set; } = new MinMaxConfig() { Min = 1, Max = 9 };
}
