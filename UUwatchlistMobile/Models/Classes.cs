using SQLite;
namespace UUwatchlistMobile.Models;


    

    [Table("tblVideos")]
    public class VideoInfo
    {
        [PrimaryKey]
        public int idVideo { get; set; }
        public string Title { get; set; }
        public string ThumbnailUrl { get; set; }
        public string PublishedDate { get; set; }
        public string VideoUrl {  get; set; }
        public int POVId { get; set; }
        public int? ArcId { get; set; }
        public int season {  get; set; }
        

    }

    [Table("tblCreators")]
    public class ChannelInfo
    {
        [PrimaryKey]
        public int idCreators { get; set; }
        public string name { get; set; }
        public string channelId { get; set; }
        public string uploads { get; set; }
        public DateTime lastChecked { get; set; }
    }
    [Table("tblArcs")]
    public class ArcInfo
    {
        [PrimaryKey]
        public int idArc { get; set; }
        public string NameArc { get; set; }
        public string typeofArc { get; set; }
    }



