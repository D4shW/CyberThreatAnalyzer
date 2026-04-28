namespace CyberThreatAnalyzer.ViewModels
{
    public class MainViewModel : ObservableObject
    {
        // Propriétés exposées à la vue (MainWindow.xaml) pour les bindings
        public UrlViewModel UrlVM { get; } = new UrlViewModel();
        public FileViewModel FileVM { get; } = new FileViewModel();
        public IpViewModel IpVM { get; } = new IpViewModel();
        public SettingsViewModel SettingsVM { get; } = new SettingsViewModel();
    }
}