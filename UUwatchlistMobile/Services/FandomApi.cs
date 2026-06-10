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
        
        
        public string ObtainApiWikiPage(string pageName) 
        {
            string apiUrl = $"https://{domainName}/api.php?action=parse&page={Uri.EscapeDataString(pageName)}&format=json&prop=text";
            return apiUrl;
            
        }

        public async Task<List<ArcInfo>> GetArcPagesAsync()
        {
            List<ArcInfo> arcInfos = new List<ArcInfo>();
            try
            {
                string url = "https://unstable-universe-mc.fandom.com/api.php?action=parse&page=Arcs&format=json&prop=text";

                using var client = new HttpClient();
                client.DefaultRequestHeaders.Add("User-Agent", "UUwatchlistMobile/1.0");

                string jsonResponse = await client.GetStringAsync(url);

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
                    

                    //if (r.ParentNode != table && r.ParentNode.ParentNode != table) continue;

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


    }
}
