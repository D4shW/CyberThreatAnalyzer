using System.Windows;
using CyberThreatAnalyzer.ViewModels;

// LE ".Views" EST OBLIGATOIRE ICI :
namespace CyberThreatAnalyzer.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new MainViewModel();
        }
    }
}