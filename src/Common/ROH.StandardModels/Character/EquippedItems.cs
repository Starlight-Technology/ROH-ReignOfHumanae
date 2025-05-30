using System.Collections.Generic;

namespace ROH.StandardModels.Character
{
    public class EquippedItems
    {
        public EquippedItems()
        {
            LeftHandRings = new HashSet<HandRing>();
            RightHandRings = new HashSet<HandRing>();
        }

        public long IdCharacter { get; set; }
        public long? IdArmor { get; set; }
        public long? IdHead { get; set; }
        public long? IdBoots { get; set; }
        public long? IdGloves { get; set; }
        public long? IdLegs { get; set; }
        public long? IdLeftBracelet { get; set; }
        public long? IdNecklace { get; set; }
        public long? IdRightBracelet { get; set; }

        public virtual ICollection<HandRing> LeftHandRings { get; set; }
        public virtual ICollection<HandRing> RightHandRings { get; set; }
    }
}
