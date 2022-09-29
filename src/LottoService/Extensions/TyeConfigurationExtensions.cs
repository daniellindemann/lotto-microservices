namespace LottoService.Extensions;

public static class TyeConfigurationExtensions
{
    public static bool IsTye(this IConfiguration configuration)
    {
        var appInstanceVariableAvailable = !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("APP_INSTANCE"));
        var serviceVariablesAvailable = Environment.GetEnvironmentVariables().Keys.Cast<string>().Count(k => k.StartsWith("SERVICE__", true, System.Globalization.CultureInfo.InvariantCulture)) > 0;

        return appInstanceVariableAvailable && serviceVariablesAvailable;
    }
}