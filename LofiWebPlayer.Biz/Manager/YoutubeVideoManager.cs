using System.Collections.Generic;
using System.Threading.Tasks;
using LofiWebPlayer.Biz.Manager.Configuration;
using LofiWebPlayer.Biz.Manager.Services.YoutubeService;

namespace LofiWebPlayer.Biz.Manager
{
    internal sealed class YoutubeVideoManager
    {
        private ServiceLocator _serviceLocator;
        private YoutubeServiceGateway _youtubeServiceGateway;
        private ConfigurationBase _configurationProvider;

        internal YoutubeServiceGateway YoutubeServiceGateway { get { return _youtubeServiceGateway ?? (_youtubeServiceGateway = _serviceLocator.CreateYoutubeServiceGateway(ConfigurationProvider.GetYoutubeApiKey())); } }

        internal ConfigurationBase ConfigurationProvider { get { return _configurationProvider ?? (_configurationProvider = _serviceLocator.CreateConfigProvider()); } }

        public YoutubeVideoManager(ServiceLocator serviceLocator)
        {
            this._serviceLocator = serviceLocator;
        }

        public async Task<IEnumerable<Manager.Services.YoutubeService.ResourceModel.Video>> GetVideos(string query)
        {
            return await this.YoutubeServiceGateway.GetVideos(query).ConfigureAwait(false);
        }
    }
}