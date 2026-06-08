using SQLite;
namespace UUwatchlistMobile.Models;


    public enum ArcType
    {
        Individual,
        Major
    }

    [Table("tblVideos")]
    public class VideoInfo
    {
        [PrimaryKey]
        public int idVideo { get; set; }
        public string Title { get; set; }
        public string ThumbnailUrl { get; set; }
        public DateTime PublishedDate { get; set; }
        public string Description { get; set; }
        public int POVId { get; set; }
        public int? ArcId { get; set; }
        

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
        public ArcType typeofArc { get; set; }
    }



