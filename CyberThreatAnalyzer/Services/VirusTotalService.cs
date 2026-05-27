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
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    throw new Exception("Cette URL n'est pas connue de VirusTotal (jamais analysée).");
                throw new Exception($"Erreur de l'API VirusTotal : Code {response.StatusCode}.");
            }

            string jsonResponse = await response.Content.ReadAsStringAsync();
            JObject json = JObject.Parse(jsonResponse);

            var attributes = json["data"]?["attributes"];
            var stats = attributes?["last_analysis_stats"];
            
            int malicious = stats?["malicious"]?.Value<int>() ?? 0;
            int totalEngines = malicious + (stats?["undetected"]?.Value<int>() ?? 0) + 
                               (stats?["harmless"]?.Value<int>() ?? 0) + (stats?["suspicious"]?.Value<int>() ?? 0) + 
                               (stats?["timeout"]?.Value<int>() ?? 0);

            long unixDate = attributes?["last_analysis_date"]?.Value<long>() ?? 0;
            DateTime lastDate = DateTimeOffset.FromUnixTimeSeconds(unixDate).LocalDateTime;

            var maliciousEngines = new List<ThreatDetail>();
            var results = attributes?["last_analysis_results"] as JObject;
            
            if (results != null)
            {
                foreach (var property in results.Properties())
                {
                    var engineName = property.Name;
                    var category = property.Value["category"]?.ToString();
                    var threatType = property.Value["result"]?.ToString();

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

            return new UrlAnalysisResult
            {
                Url = url,
                MaliciousCount = malicious,
                TotalEngines = totalEngines,
                LastAnalysisDate = lastDate,
                MaliciousEngines = maliciousEngines
            };
        }

        public async Task<FileAnalysisResult> GetFileReportAsync(string hash)
        {
            string apiUrl = $"https://www.virustotal.com/api/v3/files/{hash}";
            HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);
            
            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    throw new Exception("Ce fichier ou hash n'est pas connu de VirusTotal. Il n'a jamais été analysé.");
                throw new Exception($"Erreur de l'API VirusTotal : Code {response.StatusCode}.");
            }

            string jsonResponse = await response.Content.ReadAsStringAsync();
            JObject json = JObject.Parse(jsonResponse);
            var attributes = json["data"]?["attributes"];
            var stats = attributes?["last_analysis_stats"];
            
            int malicious = stats?["malicious"]?.Value<int>() ?? 0;
            int totalEngines = malicious + (stats?["undetected"]?.Value<int>() ?? 0) + 
                               (stats?["harmless"]?.Value<int>() ?? 0) + (stats?["suspicious"]?.Value<int>() ?? 0) + 
                               (stats?["timeout"]?.Value<int>() ?? 0);

            var enginesAndThreats = new Dictionary<string, string>();
            var results = attributes?["last_analysis_results"] as JObject;
            if (results != null)
            {
                foreach (var property in results.Properties())
                {
                    var category = property.Value["category"]?.ToString();
                    if (category == "malicious" || category == "suspicious")
                    {
                        enginesAndThreats.Add(property.Name, property.Value["result"]?.ToString() ?? "Menace inconnue");
                    }
                }
            }

            return new FileAnalysisResult
            {
                Hash = hash,
                FileName = attributes?["meaningful_name"]?.ToString() ?? "Nom inconnu",
                Size = attributes?["size"]?.Value<long>() ?? 0,
                FileType = attributes?["type_description"]?.ToString() ?? "Inconnu",
                Positives = malicious,
                TotalEngines = totalEngines,
                EnginesAndThreats = enginesAndThreats
            };
        }

        public async Task<IpAnalysisResult> GetIpReportAsync(string ip)
        {
            string apiUrl = $"https://www.virustotal.com/api/v3/ip_addresses/{ip}";
            HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);
            
            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    throw new Exception("Cette adresse IP n'est pas connue de VirusTotal.");
                throw new Exception($"Erreur de l'API VirusTotal : Code {response.StatusCode}.");
            }

            string jsonResponse = await response.Content.ReadAsStringAsync();
            JObject json = JObject.Parse(jsonResponse);
            var attributes = json["data"]?["attributes"];

            return new IpAnalysisResult
            {
                Ip = ip,
                Country = attributes?["country"]?.ToString() ?? "Inconnu",
                Asn = attributes?["as_owner"]?.ToString() ?? "Inconnu",
                ReputationScore = attributes?["reputation"]?.Value<int>() ?? 0
            };
        }
    }
}