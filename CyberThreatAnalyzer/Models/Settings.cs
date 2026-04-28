namespace CyberThreatAnalyzer.Models
{
    public class Settings
    {
        public string DefaultHashAlgorithm { get; set; } = "SHA-256";
        public int MaxResults { get; set; } = 50;
        public bool HistoryEnabled { get; set; } = true;
    }
}