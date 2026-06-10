using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection.Metadata;
using System.ServiceModel.Syndication;
using System.Text;
using System.Xml;
using UUwatchlistMobile.Models;
using UUwatchlistMobile.Services;

namespace UUwatchlistMobile.Services
{
    public class YoutubePollingService
    {
        private readonly DatabaseService _databaseService;
        private readonly ApiKeyService _apiKeyService;

        private PeriodicTimer? _timer;
        private CancellationTokenSource _cts;


        public YoutubePollingService(DatabaseService databaseService, ApiKeyService apiKeyService)
        {
            _databaseService = databaseService;
            _apiKeyService = apiKeyService;
        }

        public void Avvia() 
        {
            if (_timer != null)
            {
                Task.Run(async () => await CheckFeedAsync());
                return;
            }
            _cts = new CancellationTokenSource();
            _timer = new PeriodicTimer(TimeSpan.FromMinutes(15));
            Task.Run(async () => await ExecutePollingASync(_cts.Token));
            return;
            
        }

        private async Task ExecutePollingASync(CancellationToken cancellationToken) 
        {
            await CheckFeedAsync();

            try
            {
                while (await _timer!.WaitForNextTickAsync(cancellationToken)) 
                {
                    await CheckFeedAsync();
                }
            }
            catch (OperationCanceledException)
            {

                Debug.WriteLine("Polling interrotto.");
            }
        }

        private async Task CheckFeedAsync() 
        {
            if (!_apiKeyService.HasKey)
            {
                Debug.WriteLine("API key mancante. Impossibile eseguire il polling.");
                return;
            }
            try
            {
                List<ChannelInfo> channels = await _databaseService.GetAllChannelsAsync();

                if (channels == null || channels.Count == 0)
                {
                    Debug.WriteLine("No channel found in database");
                    return;
                }

                using var httpClient = new HttpClient();

                foreach(var c in channels) 
                {   
                    Debug.WriteLine($"Controllo canale: {c.name} ({c.channelId})");

                    string rssUrl = $"https://www.youtube.com/xml/feeds/videos.xml?channel_id={c.channelId}";

                    try
                    {
                        var xmlContent = await httpClient.GetStreamAsync(rssUrl);

                        using var stringReader = new StringReader(Convert.ToString(xmlContent));
                        using var xmlReader = XmlReader.Create(stringReader);

                        var feed = SyndicationFeed.Load(xmlReader);
                        var ultimoVideoFeed = feed.Items.FirstOrDefault();
                        if (ultimoVideoFeed != null)
                        {
                            string videoId = ultimoVideoFeed.Id.Replace("yt:video", "");
                            string titolo = ultimoVideoFeed.Title.Text;

                            var video = new VideoInfo
                            {
                                idVideo = int.Parse(videoId),
                                Title = titolo,
                                ThumbnailUrl = $"https://img.youtube.com/vi/{videoId}/hqdefault.jpg",
                                PublishedDate = ultimoVideoFeed.PublishDate.ToString(),
                                POVId = c.idCreators,
                                ArcId = 0
                            };


                            bool isNuovo = await _databaseService.SaveVideoAsync(video);

                            if (isNuovo == true)
                            {
                                Debug.WriteLine($"Nuovo video trovato: {c.name}:{video.Title}");
                            }
                        }

                        c.lastChecked = DateTime.Now;
                        await _databaseService.UpdateChannelAsync(c);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"Errore durante il controllo del canale {c.name}: {ex.Message}"); Debug.WriteLine(ex.Message);
                        throw;
                    }

                }

            }
            catch (Exception)
            {

                throw;
            }

            Ferma();
        }

        public void Ferma()
        {
            _cts.Cancel();
            _timer.Dispose();
            _timer = null;
            
        }

    }
}
