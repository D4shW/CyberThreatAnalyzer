using System.Threading.Tasks;

namespace CyberThreatAnalyzer.ViewModels
{
    public class IpViewModel : ObservableObject
    {
        private string _inputIp;
        public string InputIp
        {
            get => _inputIp;
            set { _inputIp = value; OnPropertyChanged(); }
        }

        // Ajoute ici Result, IsBusy, ErrorMessage et la méthode AnalyzeIpAsync
    }
}