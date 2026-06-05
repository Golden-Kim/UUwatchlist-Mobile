using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using UUWatchlist.Models;
using UUWatchlist.Services.Interfaces;
using Microsoft.Extensions.Configuration;
namespace UUWatchlist.Services
{
    public class YoutubeService
    {

        /*private readonly string _apyKey = "";
        
        public YoutubeService(IConfiguration configuration) 
        {

            _apyKey = configuration["YouTube:ApiKey"] ?? string.Empty;
        }

        public Task<VideosInfo> GetLatestVideoAsync(string uploadPlaylistId)
        {

            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = _apyKey,
                ApplicationName = "UUWatchlist"

            });
            var request = youtubeService.PlaylistItems.List("snippet");
            request.PlaylistId = uploadPlaylistId;
            request.MaxResults = 1;


            var response = request.ExecuteAsync();

            VideosInfo videoInfo = new VideosInfo();

            if(response != null) 
            {
                videoInfo.Title = response.Result.Items[0].Snippet.Title;
                videoInfo.ThumbnailUrl = response.Result.Items[0].Snippet.Thumbnails.Default__.Url;
                videoInfo.PublishedDate = response.Result.Items[0].Snippet.PublishedAtDateTimeOffset.Value;
                videoInfo.Description = response.Result.Items[0].Snippet.Description;
                videoInfo.Id = response.Result.Items[0].Snippet.ResourceId.VideoId;
                videoInfo.POV = response.Result.Items[0].Snippet.VideoOwnerChannelTitle;


            }
            return Task.FromResult(videoInfo); 



        }*/
    }
}
