using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using LofiWebPlayer.Biz.Manager.ServiceLocators;

namespace LofiWebPlayer.Biz.Manager.Services.YoutubeService
{
    internal sealed class YoutubeServiceGateway
    {
        private YouTubeService _youTubeService;

        public YoutubeServiceGateway(string apiKey,IYoutubeClientServiceFactory youtubeServiceHandlerFactory)
        {
            this._youTubeService = CreateYoutubeService(apiKey,youtubeServiceHandlerFactory);
        }

        public async Task<IEnumerable<YoutubeService.ResourceModel.Video>> GetVideos(string queryKey)
        {
            var videoResponse = this.GetVideoUrlsData(queryKey);

            await Task.WhenAll(videoResponse);

            return this.MapToVideos(videoResponse.Result);
        }

        private static YouTubeService CreateYoutubeService(string apiKey,IYoutubeClientServiceFactory youtubeClientServiceFactory)
        {
            return youtubeClientServiceFactory.CreateYoutubeService(apiKey);
        }

        private static void ThrowExceptionMessages(AggregateException ex)
        {
            StringBuilder errorMessages = new StringBuilder();

            foreach(var e in ex.InnerExceptions)
            {
                errorMessages.AppendLine("Error: " + e.Message);
            }

            throw new Exception(errorMessages.ToString());
        }

        private async Task<SearchListResponse> GetVideoUrlsData(string queryKey)
        {
            var searchListRequest = _youTubeService.Search.List("snippet");

            SearchListResponse searchListResponse = null;

            try
            {
                searchListRequest.Q = queryKey; // Replace with your search term.
                searchListRequest.MaxResults = 50;
                searchListRequest.SafeSearch = SearchResource.ListRequest.SafeSearchEnum.Moderate;
                searchListRequest.VideoDuration = SearchResource.ListRequest.VideoDurationEnum.Long__;
                searchListRequest.VideoDefinition = SearchResource.ListRequest.VideoDefinitionEnum.High;
                searchListRequest.Type = "video";
                searchListResponse = await searchListRequest.ExecuteAsync().ConfigureAwait(false);
            }
            catch(Exception ex)
            {
                throw new Exception("Error: " + ex.Message);
            }

            return searchListResponse;
        }

        private IEnumerable<YoutubeService.ResourceModel.Video> MapToVideos(SearchListResponse result)
        {
            foreach(var searchResult in result.Items)
            {
                if(!this.VerifySearchResultIsValid(searchResult))
                {
                    continue;
                }

                yield return new YoutubeService.ResourceModel.Video()
                {
                    Ttile = searchResult.Snippet.Title.Substring(0,10),
                    Url = $"https://www.youtube.com/embed/{searchResult.Id.VideoId}"
                };
            }
        }

        private bool VerifySearchResultIsValid(SearchResult searchResult) => searchResult.Id.Kind.Equals("youtube#video") && searchResult.Snippet.Title != null && searchResult.Id.VideoId != null;
    }
}
