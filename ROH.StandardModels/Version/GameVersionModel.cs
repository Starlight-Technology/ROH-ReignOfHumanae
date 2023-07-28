using System;

namespace ROH.StandardModels.Version
{
    public class GameVersionModel
    {
        public int Version { get; }
        public int Release { get; }
        public int Review { get; }
        public bool Released { get; }
        public DateTime ReleaseDate { get; }
        public DateTime VersionDate { get; }

        public GameVersionModel(int version, int release, int review, bool released, DateTime releaseDate, DateTime versionDate)
        {
            Version = version;
            Release = release;
            Review = review;
            Released = released;
            ReleaseDate = releaseDate;
            VersionDate = versionDate;
        }
    }



}