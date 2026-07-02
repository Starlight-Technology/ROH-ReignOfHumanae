using System;
using System.Collections.Generic;

namespace ROH.StandardModels.Version
{
    public class FileEntry
    {
        public string RelativePath { get; set; } = string.Empty;
        public long Size { get; set; }
        public string Format { get; set; } = string.Empty;
    }

    public class BuildUploadAnalysis
    {
        public Guid AnalysisId { get; set; }
        public List<FileEntry> NewFiles { get; set; } = new List<FileEntry>();
        public List<FileEntry> UpdatedFiles { get; set; } = new List<FileEntry>();
        public List<FileEntry> IdenticalFiles { get; set; } = new List<FileEntry>();
        public List<FileEntry> DeactivatedFiles { get; set; } = new List<FileEntry>();
        public List<string> Errors { get; set; } = new List<string>();
    }

    public class BuildUploadConfirmation
    {
        public Guid AnalysisId { get; set; }
        public bool ReplaceIdentical { get; set; }
    }

    public class BuildUploadResult
    {
        public bool Success { get; set; }
        public int Added { get; set; }
        public int Updated { get; set; }
        public int Skipped { get; set; }
        public int Deactivated { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
    }
}
