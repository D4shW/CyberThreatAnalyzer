using System;
using System.Collections.ObjectModel;
using System.IO;
using Newtonsoft.Json;
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
        
        public RelayCommand SaveCommand { get; }

        public SettingsViewModel()
        {
            _historyService = new HistoryService();
            _configService = new ConfigService();
            
            CurrentSettings = _configService.GetSettings();
            HistoryList = new ObservableCollection<HistoryEntry>(_historyService.GetHistory());
            
            SaveCommand = new RelayCommand(o => SaveSettings());
        }

        private void SaveSettings()
        {
            try
            {
                string optionsFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "options.json");
                File.WriteAllText(optionsFile, JsonConvert.SerializeObject(CurrentSettings, Formatting.Indented));
            }
            catch (Exception ex)
            {
                // Gestion d'erreur silencieuse ou via pop-up
                System.Windows.MessageBox.Show($"Erreur lors de la sauvegarde : {ex.Message}", "Erreur", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }
    }
}