namespace ROH.Domain.Version
{
    public record GameVersionFile(
        long Id = 0,
        long IdVersion = 0,
        Guid Guid = default,
        long Size = 0,
        string Name = "",
        string Path = "",
        string Format = ""
        )
    {
        public virtual GameVersion? GameVersion { get; set; }
    }
}