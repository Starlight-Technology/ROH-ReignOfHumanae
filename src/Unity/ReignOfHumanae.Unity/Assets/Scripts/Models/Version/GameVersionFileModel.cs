using Assets.Scripts.Models.Version;

using System;

namespace ROH.Models.Version
{
    [Serializable]
    public record GameVersionFileModel
    {
        public Guid Guid { get; set; }
        public string Name { get; set; }
        public long Size { get; set; }
        public string Path { get; set; }
        public string Format { get; set; }
        public byte[] Content { get; set; }
        public virtual GameVersionModel GameVersion { get; set; }
    }
}