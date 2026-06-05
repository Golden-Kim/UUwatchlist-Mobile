namespace UUWatchlist.Services.Interfaces
{
    using UUWatchlist.Models;
    public interface IDatabaseService
    {
        Task InitializeAsync();
        Task<List<YoutubeChannelInfo>> GetAllChannelsAsync();
        Task<bool> AddChannelAsync(YoutubeChannelInfo channel);
        Task<bool> UpdateChannelAsync(YoutubeChannelInfo channel);
        

        Task<List<VideosInfo>> ObtainAllVideosAsync();
        Task<List<VideosInfo>> ObtainVideosPerChannelAsync(int channelId);
        Task<bool> SaveVideoAsync(VideosInfo video);

    }
}
