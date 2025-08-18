using System;

namespace BE
{
    public class RestoreAnalysisData
    {
        public DateTime? BackupStartDate { get; set; }
        public string SqlVersion { get; set; }
        public string LogicalDataName { get; set; }
        public string LogicalLogName { get; set; }
        public string BackupFullPath { get; set; }
        public double SizeMb { get; set; }
    }
}
