using System.Collections.ObjectModel;
using CyberThreatAnalyzer.Models;
using CyberThreatAnalyzer.Services;

namespace CyberThreatAnalyzer.ViewModels
{
    public class SettingsViewModel : ObservableObject
    {
        private readonly HistoryService _historyService;
        private readonly ConfigService _configService;

        public Settings CurrentSettings { get; set; }
        public ObservableCollection<HistoryEntry> HistoryList { get; set; }

        public SettingsViewModel()
        {
            _historyService = new HistoryService();
            _configService = new ConfigService();
            
            CurrentSettings = _configService.GetSettings();
            HistoryList = new ObservableCollection<HistoryEntry>(_historyService.GetHistory());
        }

        // Ajouter une commande pour sauvegarder les paramètres si nécessaire
    }
}