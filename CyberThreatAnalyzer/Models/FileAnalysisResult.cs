using System.Collections.Generic;

namespace CyberThreatAnalyzer.Models
{
    public class FileAnalysisResult : BaseAnalysisResult
    {
        public string Hash { get; set; }
        public string FileName { get; set; }
        public long Size { get; set; }
        public string FileType { get; set; }
        public Dictionary<string, string> EnginesAndThreats { get; set; } = new Dictionary<string, string>();
    }
}