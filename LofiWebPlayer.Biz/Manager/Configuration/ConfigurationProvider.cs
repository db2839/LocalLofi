using Microsoft.Extensions.Configuration;

namespace LofiWebPlayer.Biz.Manager.Configuration
{
    internal sealed class ConfigurationProvider:ConfigurationBase
    {
        private readonly IConfigurationRoot _configurationRoot;

        public ConfigurationProvider()
        {
            var configBuilder = new ConfigurationBuilder();
            configBuilder.AddJsonFile("appsettings.Json");
            this._configurationRoot = configBuilder.Build();
        }

        public ConfigurationProvider(IConfigurationRoot configurationRoot)
        {
            this._configurationRoot = configurationRoot;
        }

        protected override string RetrieveAppSettings(string appSettingKey)
        {
            return this._configurationRoot["AppSettings:" + appSettingKey];
        }
    }
}
