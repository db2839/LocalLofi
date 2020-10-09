using System.Net.Http;
using Google.Apis.YouTube.v3;
using LofiWebPlayer.Biz.Manager.Configuration;
using LofiWebPlayer.Biz.Manager.ServiceLocators;
using LofiWebPlayer.Biz.Manager.Services.YoutubeService;

namespace LofiWebPlayer.Biz.Manager
{
    internal abstract class ServiceLocatorBase:IHttpMessageHandlerFactory, IYoutubeClientServiceFactory
    {
        public YoutubeServiceGateway CreateYoutubeServiceGateway(string apikey)
        {
            return this.CreateYoutubeServiceGatewayCore(apikey);
        }

        public YouTubeService CreateYoutubeService(string apikey)
        {
            return this.CreateYoutubeServiceCore(apikey);
        }

        public HttpMessageHandler CreateHttpMessageHandler()
        {
            return this.CreateHttpMessageHandlerCore();
        }

        public YoutubeVideoManager CreateYoutubeVideoManager()
        {
            return this.CreateYoutubeVideoManagerCore();
        }

        public ConfigurationBase CreateConfigProvider()
        {
            return this.CreateConfigProviderCore();
        }

        internal abstract YoutubeServiceGateway CreateYoutubeServiceGatewayCore(string apikey);

        internal abstract YouTubeService CreateYoutubeServiceCore(string apikey);

        protected abstract HttpMessageHandler CreateHttpMessageHandlerCore();

        protected abstract YoutubeVideoManager CreateYoutubeVideoManagerCore();

        protected abstract ConfigurationBase CreateConfigProviderCore();
    }
}