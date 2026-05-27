using System;
using System.Threading.Tasks;
using CyberThreatAnalyzer.Models;
using CyberThreatAnalyzer.Services;

namespace CyberThreatAnalyzer.ViewModels
{
    public class FileViewModel : ObservableObject
    {
        private VirusTotalService? _vtService;
        private readonly HashService _hashService = new();
        private readonly HistoryService _historyService = new();
        
        private string? _inputHash;
        public string? InputHash { get => _inputHash; set { _inputHash = value; OnPropertyChanged(); }}

        private FileAnalysisResult? _result;
        public FileAnalysisResult? Result { get => _result; set { _result = value; OnPropertyChanged(); }}

        private bool _isBusy;
        public bool IsBusy { get => _isBusy; set { _isBusy = value; OnPropertyChanged(); }}

        private string? _errorMessage;
        public string? ErrorMessage { get => _errorMessage; set { _errorMessage = value; OnPropertyChanged(); }}

        public RelayCommand BrowseCommand => new(obj => {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            if (dialog.ShowDialog() == true) {
                InputHash = _hashService.ComputeSHA256(dialog.FileName);
            }
        });

        public RelayCommand AnalyzeHashCommand { get; }

        public FileViewModel()
        {
            try
            {
                _vtService = new VirusTotalService();
            }
            catch (Exception ex)
            {
                ErrorMessage = "Erreur de configuration : " + ex.Message;
            }

            AnalyzeHashCommand = new RelayCommand(async (o) => await AnalyzeFileAsync(), (o) => !string.IsNullOrWhiteSpace(InputHash));
        }

        private async Task AnalyzeFileAsync()
        {
            if (_vtService == null)
            {
                ErrorMessage = "Le service VirusTotal n'a pas pu être initialisé.";
                return;
            }

            IsBusy = true;
            ErrorMessage = string.Empty;
            Result = null;

            try
            {
                string cleanHash = InputHash!.Trim().ToLower();
                Result = await _vtService.GetFileReportAsync(cleanHash); 
                _historyService.AddEntry(new HistoryEntry { Type = "HASH", Target = cleanHash, Timestamp = DateTime.Now });
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}