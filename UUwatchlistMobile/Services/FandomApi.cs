using Microsoft.Extensions.Validation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using HtmlAgilityPack;
using System.Windows.Markup;
using UUwatchlistMobile.Models;


namespace UUwatchlistMobile.Services
{
    public class FandomApi  
    {
        string domainName = "unstable-universe-mc.fandom.com";
        
        public FandomApi()
        {
            
            
        }

        public string ObtainApiWikiPage(string pageName) 
        {
            string apiUrl = $"https://{domainName}/api.php?action=parse&page={Uri.EscapeDataString(pageName)}&format=json&prop=text";
            return apiUrl;
            
        }




        public async Task<List<ArcInfo>> GetArcPagesAsync()
        {
            string url = "https://unstable-universe-mc.fandom.com/api.php?action=parse&page=Arcs&format=json&prop=text";
            List<ArcInfo> arcInfos = new List<ArcInfo>();
            try
            {

                using var _htmlClient = new HttpClient();
                _htmlClient.DefaultRequestHeaders.Add("User-Agent", "UUwatchlistMobile/1.0");

                string jsonResponse = await _htmlClient.GetStringAsync(url);

                using JsonDocument doc = JsonDocument.Parse(jsonResponse);
                JsonElement root = doc.RootElement;


                JsonElement HtmlElement = new JsonElement();
                HtmlElement = root.GetProperty("parse").GetProperty("text").GetProperty("*");


                string htmlContent = HtmlElement.GetString() ?? string.Empty;

                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(htmlContent);

                var table = htmlDoc.DocumentNode.SelectSingleNode("//table[contains(@class, 'wikitable')]");

                if (table == null)
                {
                    Debug.WriteLine("Table not found in the HTML content."); return null;
                }

                var rows = table.SelectNodes(".//tr");
                if (rows == null) return null;


                int lastId = 0;
                string lastName = string.Empty;
                string lastType = string.Empty;


                foreach (var r in rows)
                {

                    var intestationCells = r.SelectNodes(".//th");
                    if (intestationCells != null && intestationCells.Count > 0) continue;

                    var dataCell = r.SelectNodes(".//td");
                    if (dataCell == null || dataCell.Count < 3) continue;


                    if (dataCell.Count >= 5)
                    {
                        lastId = int.Parse(dataCell[0].InnerText.Trim());
                        lastName = dataCell[1].InnerText.Trim();
                        lastType = dataCell[2].InnerText.Trim();
                    }
                    else if (dataCell.Count >= 1) continue;
                    

                    

                    if (string.IsNullOrWhiteSpace(lastName)) continue;

                     

                    ArcInfo Arc = new ArcInfo
                    { 

                        idArc = lastId,
                        NameArc = lastName,
                        typeofArc = lastType,
                    };

                    arcInfos.Add(Arc);

                    continue;
                }



            }
            catch (Exception ex)
            {

                Debug.WriteLine($"Error fetching or parsing data: {ex.Message}");
            }

            return arcInfos;

        }

        public async Task<List<VideoInfo>> GetVideosAsync() 
        {
            string url = "https://unstable-universe-mc.fandom.com/api.php?action=parse&page=Episode_Order&format=json&prop=text";
            List<VideoInfo> newVideos = new List<VideoInfo>();
            try
            {
                using var _htmlClient = new HttpClient();
                _htmlClient.DefaultRequestHeaders.Add("User-Agent", "UUwatchlistMobile/1.0");

                string jsonResponse = await _htmlClient.GetStringAsync(url);

                using JsonDocument doc = JsonDocument.Parse(jsonResponse);
                JsonElement root = doc.RootElement;

                if (!root.TryGetProperty("parse", out JsonElement parseElement) ||
                !parseElement.TryGetProperty("text", out JsonElement textElement) ||
                !textElement.TryGetProperty("*", out JsonElement htmlElement))
                {
                    Debug.WriteLine("Impossibile trovare il contenuto della pagina nel JSON. Verifica il nome della pagina.");
                    return newVideos;
                }

                string htmlInsert = htmlElement.GetString() ?? string.Empty;

                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(htmlInsert);

                var allTables = htmlDoc.DocumentNode.SelectNodes("//table[contains(@class, 'wikitable') or contains(@class, 'article-table')]");

                if (allTables != null)
                {
                    for (int i = 0; i < Math.Min(allTables.Count, 3); i++)
                    {
                        var currentTable = allTables[i];
                        int seasonNumber = i + 1;

                        int lastId = 0;
                        string lastName = string.Empty;

                        var rows = currentTable.SelectNodes(".//tr");

                        if (rows == null) continue;
                        

                        foreach (var r in rows)
                        {
                            if (r.SelectNodes(".//th") != null) continue;
                            
                            var dataCell = r.SelectNodes(".//td");
                            if (dataCell != null || dataCell.Count == 0)
                            {
                                int UploaderId = 0;
                                switch (dataCell[2].InnerText.Trim())
                                {
                                    case "SpokeIsHere":
                                        UploaderId = 1; break;
                                    case "ParrotX2":
                                        UploaderId = 2; break;
                                    case "Wemmbu":
                                        UploaderId = 3; break;
                                    case "FlameFrags":
                                        UploaderId = 4; break;
                                    default:
                                        break;
                                }

                                VideoInfo newVideo = new VideoInfo 
                                {
                                    idVideo = int.Parse(dataCell[0].InnerText.Trim()),
                                    Title = dataCell[1].InnerText.Trim(),
                                    PublishedDate = dataCell[3].InnerText.Trim(),
                                    POVId = UploaderId,
                                    VideoUrl = dataCell[5].InnerText.Trim(),
                                    season = seasonNumber,
                                    
                                };

                                newVideos.Add(newVideo);
                            }
                                
                           

                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error fetching or parsing data: {ex.Message}");
            }
            return newVideos;
        }


        


    }
}
