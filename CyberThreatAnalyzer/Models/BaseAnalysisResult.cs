namespace CyberThreatAnalyzer.Models
{
    public class BaseAnalysisResult
    {
        public int Positives { get; set; }
        public int TotalEngines { get; set; }
        
        // Indicateur visuel demandé dans le TP (Vert, Orange, Rouge)
        public string RiskColor => Positives == 0 ? "Green" : (Positives < 5 ? "Orange" : "Red");
    }
}