using System.Net.Http;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using LofiWebPlayer.Biz.Manager.Configuration;
using LofiWebPlayer.Biz.Manager.Services.YoutubeService;

namespace LofiWebPlayer.Biz.Manager
{
    internal sealed class ServiceLocator:ServiceLocatorBase
    {
        protected override ConfigurationBase CreateConfigProviderCore()
        {
            return new ConfigurationProvider();
        }

        protected override HttpMessageHandler CreateHttpMessageHandlerCore()
        {
            return new HttpClientHandler();
        }

        protected override YoutubeVideoManager CreateYoutubeVideoManagerCore()
        {
            return new YoutubeVideoManager(this);
        }

        internal override YouTubeService CreateYoutubeServiceCore(string apikey)
        {
            return new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = apikey,
                ApplicationName = this.GetType().ToString(),
            });
        }

        internal override YoutubeServiceGateway CreateYoutubeServiceGatewayCore(string apikey)
        {
            return new YoutubeServiceGateway(apikey,this);
        }
    }
}