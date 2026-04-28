using System.Collections.Generic;

namespace CyberThreatAnalyzer.Models
{
    public class IpAnalysisResult
    {
        public string Ip { get; set; }
        public string Country { get; set; }
        public string Asn { get; set; }
        public int ReputationScore { get; set; }
        public List<string> MaliciousUrls { get; set; } = new List<string>();
        public List<string> MaliciousFiles { get; set; } = new List<string>();
        public List<string> HttpsCertificates { get; set; } = new List<string>();
    }
}