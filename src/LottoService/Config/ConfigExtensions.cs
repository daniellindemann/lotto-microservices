using Microsoft.Extensions.Configuration;

namespace LottoService.Config
{
    public static class ConfigExtensions
    {
        public static TConfigObj Bind<TConfigObj>(this IConfigurationSection configurationSection) where TConfigObj: new()
        {
            var tConfigObj = new TConfigObj();
            configurationSection.Bind(tConfigObj);
            return tConfigObj;
        }
    }
}
