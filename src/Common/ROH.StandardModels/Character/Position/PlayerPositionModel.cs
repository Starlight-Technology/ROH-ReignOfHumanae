namespace ROH.StandardModels.Character.Position
{
    public class PlayerPositionModel
    {
        public virtual PositionModel? Position { get; set; }
        public virtual RotationModel? Rotation { get; set; }
    }
}