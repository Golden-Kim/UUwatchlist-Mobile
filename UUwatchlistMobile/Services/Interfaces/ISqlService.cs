namespace UUWatchlist.Services.Interfaces
{
    
    using UUWatchlist.Models;
    public interface ISqlService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="namePov"></param>
        /// <returns></returns>
        Task<string> getPlaylistId(string namePov);
        /// <summary>
        /// Effettua la scrittura nel database del nuovo video di uno qualsiasi dei 4 protagonisti
        /// </summary>
        /// <param name="VIdeoToReturn">Video che bisognera mandare al db</param>
        /// <returns></returns>
        public void AddVideos(VIdeosInfo VIdeoToReturn);


    }
}
