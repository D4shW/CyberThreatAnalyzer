using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq; 
using CyberThreatAnalyzer.Models;

namespace CyberThreatAnalyzer.Services
{
    public class VirusTotalService
    {
        private readonly string _apiKey;
        private readonly HttpClient _httpClient;

        public VirusTotalService()
        {
            var configService = new ConfigService();
            _apiKey = configService.GetApiKey();
            
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("x-apikey", _apiKey);
        }

        public async Task<FileAnalysisResult> GetFileReportAsync(string hash)
        {
            await Task.Delay(1);
            throw new NotImplementedException("Appel API pour les fichiers non implémenté");
        }

        public async Task<UrlAnalysisResult> GetUrlReportAsync(string url)
        {
            var urlBytes = System.Text.Encoding.UTF8.GetBytes(url);
            var base64Url = Convert.ToBase64String(urlBytes)
                .Replace('+', '-')
                .Replace('/', '_')
                .TrimEnd('=');

            string apiUrl = $"https://www.virustotal.com/api/v3/urls/{base64Url}";

            HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);
            
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Erreur de l'API VirusTotal : Code {response.StatusCode}. L'URL n'a peut-être jamais été analysée.");
            }

            string jsonResponse = await response.Content.ReadAsStringAsync();
            JObject json = JObject.Parse(jsonResponse);

            // On récupère le bloc "attributes" qui contient tout
            var attributes = json["data"]?["attributes"];
            var stats = attributes?["last_analysis_stats"];
            
            // --- 1. Les compteurs ---
            int malicious = stats?["malicious"]?.Value<int>() ?? 0;
            int undetected = stats?["undetected"]?.Value<int>() ?? 0;
            int harmless = stats?["harmless"]?.Value<int>() ?? 0;
            int suspicious = stats?["suspicious"]?.Value<int>() ?? 0;
            int timeout = stats?["timeout"]?.Value<int>() ?? 0;

            int totalEngines = malicious + undetected + harmless + suspicious + timeout;

            // --- 2. La date de dernière analyse ---
            long unixDate = attributes?["last_analysis_date"]?.Value<long>() ?? 0;
            // Conversion du format Unix (secondes) vers un vrai DateTime lisible
            DateTime lastDate = DateTimeOffset.FromUnixTimeSeconds(unixDate).LocalDateTime;

            // --- 3. La liste des moteurs ---
            var maliciousEngines = new List<ThreatDetail>();
            var results = attributes?["last_analysis_results"] as JObject;
            
            if (results != null)
            {
                // On boucle sur chaque moteur d'antivirus
                foreach (var property in results.Properties())
                {
                    var engineName = property.Name;
                    var category = property.Value["category"]?.ToString();
                    var threatType = property.Value["result"]?.ToString();

                    // Si le moteur dit que c'est malveillant ou suspect, on l'ajoute à notre liste
                    if (category == "malicious" || category == "suspicious")
                    {
                        maliciousEngines.Add(new ThreatDetail 
                        { 
                            EngineName = engineName, 
                            ThreatType = threatType ?? "Menace inconnue" 
                        });
                    }
                }
            }

            // --- 4. Création du résultat final ---
            var result = new UrlAnalysisResult
            {
                Url = url,
                MaliciousCount = malicious,
                TotalEngines = totalEngines,
                LastAnalysisDate = lastDate,
                MaliciousEngines = maliciousEngines
            };

            return result;
        }
    }
}