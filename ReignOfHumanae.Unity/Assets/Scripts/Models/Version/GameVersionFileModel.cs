using Assets.Scripts.Models.Version;

namespace ROH.Models.Version
{
    public record GameVersionFileModel
    {
        public string Name { get; set; }
        public long Size { get; set; }
        public string Path { get; set; }
        public string Format { get; set; }
        public string Content { get; set; }
        public virtual GameVersionModel GameVersion { get; set; }
    }
}