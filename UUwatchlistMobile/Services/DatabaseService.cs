namespace UUWatchlist.Services
{
    using UUWatchlist.Services.Interfaces;
    using UUWatchlist.Models;
    using SQLite;
    using System.Diagnostics;

    public class DatabaseService : IDatabaseService
    {
        private SQLiteAsyncConnection _database;
        private readonly string _dbPath;
        public DatabaseService() 
        {
            _dbPath = Path.Combine(FileSystem.AppDataDirectory, "UU.db");

        }

        public async Task InitializeAsync() 
        {
            if (_database != null) return;

            try
            {
                _database = new SQLiteAsyncConnection(_dbPath);

                await _database.CreateTableAsync<YoutubeChannelInfo>();
                await _database.CreateTableAsync<VideosInfo>();

                Debug.WriteLine($"Database initialized at {_dbPath}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error initializing database: {ex.Message}");
                throw;
            }
        }

        

        // Ottiene la lista di tutti i canali salvati nel database
        public async Task<List<YoutubeChannelInfo>> GetAllChannelsAsync()
        {
            await InitializeAsync();
            try
            {
                return await _database!.Table<YoutubeChannelInfo>().ToListAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in reading the channels: {ex.Message}");
                return new List<YoutubeChannelInfo>();
            }
        }

        // Aggiunge un nuovo canale, controllando prima se l'ID esiste già
        public async Task<bool> AddChannelAsync(YoutubeChannelInfo channel)
        {
            await InitializeAsync();
            try
            {
                var DoesItExist = await _database!.Table<YoutubeChannelInfo>()
                                               .Where(c => c.idCreators == channel.idCreators)
                                               .FirstOrDefaultAsync();
                if (DoesItExist != null)
                {
                    Debug.WriteLine($"The channel with id {channel.idCreators} is already monitored.");
                    return false; 
                }

                await _database.InsertAsync(channel);
                return true; 
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error inserting channel: {ex.Message}");
                return false;
            }
        }

        // Aggiorna i dati di un canale esistente (es. la proprietà LastChecked)
        public async Task<bool> UpdateChannelAsync(YoutubeChannelInfo canale)
        {
            await InitializeAsync();
            try
            {
                int ModifiedRows = await _database!.UpdateAsync(canale);
                return ModifiedRows > 0;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error updating channel: {ex.Message}");
                return false;
            }
        }


        

        // Ottiene tutti i video salvati ordinati dal più recente
        public async Task<List<VideosInfo>> ObtainAllVideosAsync()
        {
            await InitializeAsync();
            try
            {
                return await _database!.Table<VideosInfo>()
                                       .OrderByDescending(v => v.PublishedDate)
                                       .ToListAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in recovering video info: {ex.Message}");
                return new List<VideosInfo>();
            }
        }

        public async Task<List<VideosInfo>> ObtainVideosPerChannelAsync(int channelId)
        {
            await InitializeAsync();
            try
            {
                return await _database!.Table<VideosInfo>()
                                       .Where(v => v.POVId == channelId)
                                       .OrderByDescending(v => v.PublishedDate)
                                       .ToListAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in recovering video info for channel {channelId}: {ex.Message}");
                return new List<VideosInfo>();
            }
        }

        // Salva un video se non esiste già. Ritorna TRUE se il video è nuovo, FALSE se esisteva.
        public async Task<bool> SaveVideoAsync(VideosInfo video)
        {
            await InitializeAsync();
            try
            {
                // Controlliamo se l'ID del video è già presente nella tabella
                var esistente = await _database!.Table<VideosInfo>()
                                               .Where(v => v.idVideo == video.idVideo)
                                               .FirstOrDefaultAsync();

                if (esistente == null)
                {
                    // Se il video non c'è, lo inseriamo (è una novità!)
                    await _database.InsertAsync(video);
                    return true;
                }

                return false; // Video già presente nel DB, nessuna azione necessaria
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error saving video: {ex.Message}");
                return false;
            }
        }



        


    }
}
