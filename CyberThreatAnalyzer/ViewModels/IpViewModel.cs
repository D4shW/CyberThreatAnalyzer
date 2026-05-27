using System;
using System.Net;
using System.Threading.Tasks;
using CyberThreatAnalyzer.Models;
using CyberThreatAnalyzer.Services;

namespace CyberThreatAnalyzer.ViewModels
{
    public class IpViewModel : ObservableObject
    {
        private readonly VirusTotalService _vtService;
        private readonly HistoryService _historyService;

        private string? _inputIp;
        public string? InputIp { get => _inputIp; set { _inputIp = value; OnPropertyChanged(); } }

        private IpAnalysisResult? _result;
        public IpAnalysisResult? Result { get => _result; set { _result = value; OnPropertyChanged(); } }

        private string? _errorMessage;
        public string? ErrorMessage { get => _errorMessage; set { _errorMessage = value; OnPropertyChanged(); } }

        private bool _isBusy;
        public bool IsBusy { get => _isBusy; set { _isBusy = value; OnPropertyChanged(); } }

        public RelayCommand AnalyzeCommand { get; }

        public IpViewModel()
        {
            _vtService = new VirusTotalService();
            _historyService = new HistoryService();
            AnalyzeCommand = new RelayCommand(async (o) => await AnalyzeIpAsync(), (o) => !string.IsNullOrWhiteSpace(InputIp));
        }

        private async Task AnalyzeIpAsync()
        {
            if (!IPAddress.TryParse(InputIp, out _))
            {
                ErrorMessage = "Format d'adresse IP invalide.";
                return;
            }

            IsBusy = true;
            ErrorMessage = string.Empty;
            Result = null;

            try
            {
                Result = await _vtService.GetIpReportAsync(InputIp!);
                _historyService.AddEntry(new HistoryEntry { Type = "IP", Target = InputIp!, Timestamp = DateTime.Now });
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