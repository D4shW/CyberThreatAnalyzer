using System;
using System.Threading.Tasks;
using CyberThreatAnalyzer.Models;
using CyberThreatAnalyzer.Services;

namespace CyberThreatAnalyzer.ViewModels
{
    public class UrlViewModel : ObservableObject
    {
        private readonly VirusTotalService _vtService;
        private readonly HistoryService _historyService;

        private string? _inputUrl;
        public string? InputUrl
        {
            get => _inputUrl;
            set { _inputUrl = value; OnPropertyChanged(); }
        }

        private UrlAnalysisResult? _result;
        public UrlAnalysisResult? Result
        {
            get => _result;
            set { _result = value; OnPropertyChanged(); }
        }

        private string? _errorMessage;
        public string? ErrorMessage
        {
            get => _errorMessage;
            set { _errorMessage = value; OnPropertyChanged(); }
        }

        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set { _isBusy = value; OnPropertyChanged(); }
        }

        public RelayCommand AnalyzeCommand { get; }

        public UrlViewModel()
        {
            _vtService = new VirusTotalService();
            _historyService = new HistoryService();
            AnalyzeCommand = new RelayCommand(async (o) => await AnalyzeUrlAsync(), (o) => !string.IsNullOrWhiteSpace(InputUrl));
        }

        private async Task AnalyzeUrlAsync()
        {
            if (!Uri.IsWellFormedUriString(InputUrl, UriKind.Absolute))
            {
                ErrorMessage = "Format d'URL invalide.";
                return;
            }

            IsBusy = true;
            ErrorMessage = string.Empty;
            Result = null;

            try
            {
                Result = await _vtService.GetUrlReportAsync(InputUrl!);
                _historyService.AddEntry(new HistoryEntry { Type = "URL", Target = InputUrl!, Timestamp = DateTime.Now });
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