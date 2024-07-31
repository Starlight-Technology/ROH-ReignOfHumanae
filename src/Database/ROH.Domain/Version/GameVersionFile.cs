using ROH.Domain.GameFiles;

namespace ROH.Domain.Version;

public record GameVersionFile(
    long Id = 0,
    long IdVersion = 0,
    long IdGameFile = 0,
    Guid Guid = default
    )
{
    public virtual GameVersion? GameVersion { get; set; }
    public virtual GameFile? GameFile { get; set; }
}