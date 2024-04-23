using System;

namespace Assets.Scripts.Models.Version
{
    /// <summary>
    /// Version is the version of the game, greater changes like something in history
    /// Release is for changes like events or new items
    /// Review is for fixes
    /// </summary>
    [Serializable]
    public class GameVersionModel
    {
        public Guid Guid { get; set; }
        public int Version { get; set; }
        public int Release { get; set; }
        public int Review { get; set; }
        public bool Released { get; set; }
        public DateTime ReleaseDate { get; set; }
        public DateTime VersionDate { get; set; }
    }
}