using System;

namespace CyberThreatAnalyzer.Models
{
    public class HistoryEntry
    {
        public DateTime Timestamp { get; set; }
        public string Type { get; set; } // "URL", "HASH", "IP"
        public string Target { get; set; }
    }
}