using UUWatchlist.Models;
namespace UUWatchlist.Services.Interfaces
{
    public interface IYoutubeService
    {
        /// <summary>
        /// Fa una richiesta all'api di yt per ottenere le informazioni dell'ultimo video caricato in un canale, dato l'id della playlist degli upload del canale
        /// </summary>
        /// <param name="uploadPlaylistId"></param>
        /// <returns></returns>
        Task<VideosInfo> GetLatestVideoAsync(string uploadPlaylistId);
    }
}
