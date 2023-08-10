using System;

namespace ROH.StandardModels.Version
{    
    public class GameVersionModel
    {
        public int Version { get; set; }
        public int Release { get; set; }
        public int Review { get; set; }
        public bool Released { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public DateTime VersionDate { get; set; }

        public GameVersionModel()
        {
                
        }

    }



}