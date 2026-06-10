namespace UUwatchlistMobile.Services.Interfaces
{
    using UUwatchlistMobile.Models;
    public interface IDatabaseService
    {
        Task InitializeAsync();
        Task<List<ChannelInfo>> GetAllChannelsAsync();
        Task<bool> AddChannelAsync(ChannelInfo channel);
        Task<bool> UpdateChannelAsync(ChannelInfo channel);
        

        Task<List<VideoInfo>> ObtainAllVideosAsync();
        Task<List<VideoInfo>> ObtainVideosPerChannelAsync(int channelId);
        Task<bool> SaveVideoAsync(VideoInfo video);


        Task<List<ArcInfo>> GetAllArcsAsync();
        Task SaveArcsAsync();

    }
}
