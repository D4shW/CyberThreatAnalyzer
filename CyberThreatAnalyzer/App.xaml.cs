using System;
using System.Windows;
using CyberThreatAnalyzer.Views; 

namespace CyberThreatAnalyzer
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            try 
            {
                // On tente d'instancier la fenêtre et de l'afficher
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
            }
            catch (Exception ex)
            {
                // Si ça plante, on attrape l'erreur et on l'affiche dans une popup !
                MessageBox.Show(ex.ToString(), "Erreur fatale au lancement", MessageBoxButton.OK, MessageBoxImage.Error);
                
                // On ferme l'application proprement
                Current.Shutdown();
            }
        }
    }
}