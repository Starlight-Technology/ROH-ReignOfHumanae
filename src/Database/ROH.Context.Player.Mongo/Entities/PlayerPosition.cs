using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ROH.Context.Player.Mongo.Entities;

public class PlayerPosition
{
    [BsonId]
    public ObjectId Id { get; set; }

    public string PlayerId { get; set; } = "";

    public float PositionX { get; set; }
    public float PositionY { get; set; }
    public float PositionZ { get; set; }

    public float RotationX { get; set; }
    public float RotationY { get; set; }
    public float RotationZ { get; set; }
    public float RotationW { get; set; }

    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}
