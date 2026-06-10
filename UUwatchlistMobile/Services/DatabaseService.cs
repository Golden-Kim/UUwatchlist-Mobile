namespace UUwatchlistMobile.Services
{
    using UUwatchlistMobile.Services.Interfaces;
    using UUwatchlistMobile.Models;
    using SQLite;
    using System.Diagnostics;
    

    public class DatabaseService : IDatabaseService
    {
        private SQLiteAsyncConnection _database;
        private readonly string _dbPath;
        FandomApi _FandomApi;
        public DatabaseService()
        {
            _dbPath = Path.Combine(FileSystem.AppDataDirectory, "UU.db");
            _FandomApi = new FandomApi();
        }

        public async Task InitializeAsync()
        {
            if (_database != null) return;

            try
            {
                if (!File.Exists(_dbPath))
                {
                    Debug.WriteLine("Database file not found in local directory, copying the pre-populated database");

                    using Stream assetStream = await FileSystem.OpenAppPackageFileAsync("UU.db");

                    using FileStream outputStream = File.Create(_dbPath);

                    await assetStream.CopyToAsync(outputStream);

                    Debug.WriteLine("Database copied successfully");
                }

                _database = new SQLiteAsyncConnection(_dbPath);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error initializing database: {ex.Message}");
            }
        }



        // Ottiene la lista di tutti i canali salvati nel database
        public async Task<List<ChannelInfo>> GetAllChannelsAsync()
        {
            await InitializeAsync();
            try
            {
                return await _database!.Table<ChannelInfo>().ToListAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in reading the channels: {ex.Message}");
                return new List<ChannelInfo>();
            }
        }

        // Aggiunge un nuovo canale, controllando prima se l'ID esiste già
        public async Task<bool> AddChannelAsync(ChannelInfo channel)
        {
            await InitializeAsync();
            try
            {
                var DoesItExist = await _database!.Table<ChannelInfo>()
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
        public async Task<bool> UpdateChannelAsync(ChannelInfo canale)
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
        public async Task<List<VideoInfo>> ObtainAllVideosAsync()
        {
            await InitializeAsync();
            try
            {
                return await _database!.Table<VideoInfo>()
                                       .OrderByDescending(v => v.PublishedDate)
                                       .ToListAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in recovering video info: {ex.Message}");
                return new List<VideoInfo>();
            }
        }

        public async Task<List<VideoInfo>> ObtainVideosPerChannelAsync(int channelId)
        {
            await InitializeAsync();
            try
            {
                return await _database!.Table<VideoInfo>()
                                       .Where(v => v.POVId == channelId)
                                       .OrderByDescending(v => v.PublishedDate)
                                       .ToListAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in recovering video info for channel {channelId}: {ex.Message}");
                return new List<VideoInfo>();
            }
        }

        // Salva un video se non esiste già. Ritorna TRUE se il video è nuovo, FALSE se esisteva.
        



        public async Task<List<ArcInfo>> GetAllArcsAsync()
        {
            await InitializeAsync();
            try
            {
                return await _database!.Table<ArcInfo>().ToListAsync();
                
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in recovering arcs info: {ex.Message}");
                return new List<ArcInfo>();
            }




        }
        public async Task<bool> SaveVideoAsync(VideoInfo video)
        {
            await InitializeAsync();
            try
            {
                // Controlliamo se l'ID del video è già presente nella tabella
                var esistente = await _database!.Table<VideoInfo>()
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
        public async Task SaveArcsAsync() 
        {
            List<ArcInfo> ArcsToSave = new List<ArcInfo>();
            await InitializeAsync();
            try
            {
                ArcsToSave = await _FandomApi.GetArcPagesAsync();
                foreach (var a in ArcsToSave) 
                {
                    bool exists = await _database!.Table<ArcInfo>()
                                                 .Where(ar => ar.idArc == a.idArc)
                                                 .FirstOrDefaultAsync() != null;
                    if (exists == false)
                    {
                        await _database.InsertAsync(a);
                    }

                    exists = true;
                }


            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in recovering arcs info: {ex.Message}");
            }
        
        }
    }
}
