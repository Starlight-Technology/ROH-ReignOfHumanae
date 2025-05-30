namespace ROH.Context.Player.Entities.Characters;

public record PlayerPosition(long Id = 0, long IdPlayer = 0, long PositionId = 0, long RotationId = 0)
{
    public virtual Character? Player { get; set; }
    public virtual Position? Position { get; set; }
    public virtual Rotation? Rotation { get; set; }
}
