using Google.Apis.YouTube.v3;

namespace LofiWebPlayer.Biz.Manager.ServiceLocators
{
    internal interface IYoutubeClientServiceFactory
    {
        YouTubeService CreateYoutubeService(string apikey);
    }
}
