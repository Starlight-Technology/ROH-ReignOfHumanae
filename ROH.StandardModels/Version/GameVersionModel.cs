using System;

namespace ROH.StandardModels.Version
{
    public class GameVersionModel
    {
        public Guid Guid { get; set; }
        public int Version { get; set; }
        public int Release { get; set; }
        public int Review { get; set; }
        public bool Released { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public DateTime VersionDate { get; set; }

        public GameVersionModel()
        {
        }

        public GameVersionListModel ToListModel() => new GameVersionListModel(Version, Release, Review, ReleaseDate, VersionDate, Guid);
    }

    public class GameVersionListModel
    {
        public int Version { get; set; }
        public int Release { get; set; }
        public int Review { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public DateTime VersionDate { get; set; }
        public string DetailsLink { get; set; }

        public GameVersionListModel(int version, int release, int review, DateTime? releaseDate, DateTime versionDate, Guid guid)
        {
            Version = version;
            Release = release;
            Review = review;
            ReleaseDate = releaseDate;
            VersionDate = versionDate;
            DetailsLink = $"/Manager/Version/VersionDetails/{guid}";
        }
    }
}