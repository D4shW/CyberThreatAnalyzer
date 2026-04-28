using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using CyberThreatAnalyzer.Models;

namespace CyberThreatAnalyzer.Services
{
    public class HistoryService
    {
        private const string HistoryFile = "history.json";

        public List<HistoryEntry> GetHistory()
        {
            if (!File.Exists(HistoryFile))
            {
                return new List<HistoryEntry>();
            }
            string json = File.ReadAllText(HistoryFile);
            return JsonConvert.DeserializeObject<List<HistoryEntry>>(json) ?? new List<HistoryEntry>();
        }

        public void AddEntry(HistoryEntry entry)
        {
            var configService = new ConfigService();
            var settings = configService.GetSettings();
            
            if (!settings.HistoryEnabled) return;

            var history = GetHistory();
            history.Insert(0, entry); // Ajoute la recherche la plus récente en haut

            // Limite la taille de l'historique en fonction des paramètres
            if (history.Count > settings.MaxResults)
            {
                history = history.GetRange(0, settings.MaxResults);
            }

            File.WriteAllText(HistoryFile, JsonConvert.SerializeObject(history, Formatting.Indented));
        }
    }
}