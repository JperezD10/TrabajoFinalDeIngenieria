using System;

namespace BE
{
    public class BackupResult
    {
        public string FileName { get; set; }
        public string FullPath { get; set; }
        public long SizeBytes { get; set; }
        public DateTime BackupDate { get; set; }
    }
}
