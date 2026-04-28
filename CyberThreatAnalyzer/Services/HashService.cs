using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace CyberThreatAnalyzer.Services
{
    public class HashService
    {
        public string ComputeSHA256(string filePath)
        {
            using (var sha256 = SHA256.Create())
            {
                using (var stream = File.OpenRead(filePath))
                {
                    var hashBytes = sha256.ComputeHash(stream);
                    return BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
                }
            }
        }
    }
}