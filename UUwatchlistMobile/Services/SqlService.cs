namespace UUWatchlist.Services
{
    using UUWatchlist.Services.Interfaces;
    using UUWatchlist.Models;
    using Microsoft.Data.Sqlite;
    public class SqlService : ISqlService
    {
        public SqlService() { }

        public Task<string> getPlaylistId(string namePov)
        {
            throw new NotImplementedException();
        }

        

        public void AddVideos(VIdeosInfo VideoToAdd)
        {
            int POVid = 0;
            switch (VideoToAdd.POV)
            {
                case "ParrotX2":
                    POVid = 0;
                    break;
                case "Spoke":
                    POVid = 1;
                    break;
                case "Wemmbu":
                    POVid = 2;
                    break;
                case "FlameFrags":
                    POVid = 3;
                    break;
            }


            string connectionString = "Data Source=Data/UU.db";


            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = @"INSERT INTO tblVideos (Title, ThumbnailUrl, PublishedDate, Description, POVId, ArcId)               
                VALUES ($title, $thumbnail, $published, $description, $pov, $arc)";

                command.Parameters.AddWithValue("$id", VideoToAdd.Id);
                command.Parameters.AddWithValue("$title", VideoToAdd.Title);
                command.Parameters.AddWithValue("$thumbnail", VideoToAdd.ThumbnailUrl);
                command.Parameters.AddWithValue("$published", VideoToAdd.PublishedDate);
                command.Parameters.AddWithValue("$description", VideoToAdd.Description);
                command.Parameters.AddWithValue("$pov", POVid);
                command.Parameters.AddWithValue("$arc", VideoToAdd.Arc);


                command.ExecuteNonQuery();

            }
            






        }
    }
}
