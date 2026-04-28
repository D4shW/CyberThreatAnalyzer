using System;
using System.Collections.Generic;

namespace CyberThreatAnalyzer.Models
{
    // Petite classe pour stocker le nom du moteur et le type de menace
    public class ThreatDetail
    {
        public string EngineName { get; set; }
        public string ThreatType { get; set; }
    }

    public class UrlAnalysisResult
    {
        public string Url { get; set; }
        
        public int MaliciousCount { get; set; }
        public int TotalEngines { get; set; }
        
        // La date de la dernière analyse
        public DateTime LastAnalysisDate { get; set; }

        // La liste des moteurs ayant détecté une menace
        public List<ThreatDetail> MaliciousEngines { get; set; } = new List<ThreatDetail>();

        // L'indicateur visuel du niveau de menace (vert / orange / rouge)
        // Vous pourrez l'utiliser dans le XAML pour changer la couleur (ex: Foreground="{Binding Result.ThreatLevelColor}")
        public string ThreatLevelColor 
        {
            get
            {
                if (MaliciousCount == 0) return "Green";
                if (MaliciousCount < 3) return "Orange"; // 1 ou 2 détections = suspect (Orange)
                return "Red"; // 3 ou plus = dangereux (Rouge)
            }
        }
    }
}