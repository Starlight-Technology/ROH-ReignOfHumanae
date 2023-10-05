namespace ROH.Domain.Version
{
    /// <summary>
    /// Version is the version of the game, greater changes like something in history
    /// Release is for changes like events or new items
    /// Review is for fixes
    /// </summary>
    /// <param name="Id"></param>
    /// <param name="Version"></param>
    /// <param name="Release"></param>
    /// <param name="Review"></param>
    /// <param name="Released"></param>
    public record GameVersion(DateTime? ReleaseDate, DateTime? VersionDate, long Id = 0, int Version = 0, int Release = 0, int Review = 0, bool Released = false);
}