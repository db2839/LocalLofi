using System;

namespace LofiWebPlayer.Biz.Manager.Configuration
{
    public abstract class ConfigurationBase
    {
        public string GetYoutubeApiKey() => this.RetrieveAppSettingsThrowIfMissing("YoutubeApiKey");

        private string RetrieveAppSettingsThrowIfMissing(string appSettingKey)
        {
            var appSettingValue = this.RetrieveAppSettings(appSettingKey);
            if(appSettingValue == null)
            {
                // TODO:Add better exceptions
                throw new NotImplementedException("Figure it out");
            }

            if(appSettingValue.Length == 0)
            {
                // TODO:Add better exceptions
                throw new NotImplementedException("Figure it out");
            }
            return appSettingValue;
        }

        protected abstract string RetrieveAppSettings(string appSettingKey);
    }
}
