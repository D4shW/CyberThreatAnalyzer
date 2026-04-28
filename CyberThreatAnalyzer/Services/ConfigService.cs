using Newtonsoft.Json;
using System.IO;
using System;
using CyberThreatAnalyzer.Models;

namespace CyberThreatAnalyzer.Services
{
    public class ConfigService
    {
        // On force le programme à chercher dans le vrai dossier d'exécution pour éviter les bugs de dotnet run
        private string ConfigFile => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.json");
        private string OptionsFile => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "options.json");

        public class ApiConfig { public string ApiKey { get; set; } }

        public string GetApiKey()
        {
            if (!File.Exists(ConfigFile))
            {
                var defaultConfig = new ApiConfig { ApiKey = "VOTRE_CLE_API_ICI" };
                File.WriteAllText(ConfigFile, JsonConvert.SerializeObject(defaultConfig, Formatting.Indented));
                
                // ON A SUPPRIMÉ LE THROW ICI POUR EMPÊCHER LE CRASH DE L'INTERFACE
                return defaultConfig.ApiKey;
            }
            
            var config = JsonConvert.DeserializeObject<ApiConfig>(File.ReadAllText(ConfigFile));
            return config?.ApiKey ?? "VOTRE_CLE_API_ICI";
        }

        public Settings GetSettings()
        {
            if (!File.Exists(OptionsFile))
            {
                var defaultSettings = new Settings();
                File.WriteAllText(OptionsFile, JsonConvert.SerializeObject(defaultSettings, Formatting.Indented));
                return defaultSettings;
            }
            return JsonConvert.DeserializeObject<Settings>(File.ReadAllText(OptionsFile));
        }
    }
}