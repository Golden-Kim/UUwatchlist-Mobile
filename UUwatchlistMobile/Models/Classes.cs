using SQLite;
namespace UUWatchlist.Models;

    [Table("tblVideos")]
    public class VideosInfo
    {
        [PrimaryKey]
        public int idVideo { get; set; }
        public string Title { get; set; }
        public string ThumbnailUrl { get; set; }
        public DateTime PublishedDate { get; set; }
        public string Description { get; set; }
        public int POVId { get; set; }
        public int ArcId { get; set; }
        

    }
    
    public class YoutubeChannelInfo
    {
        [PrimaryKey]
        public int idCreators { get; set; }
        public string name { get; set; }
        public string channelId { get; set; }
        public string uploads { get; set; }
        public DateTime lastChecked { get; set; }
}



