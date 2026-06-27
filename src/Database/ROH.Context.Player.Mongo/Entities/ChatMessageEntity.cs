using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ROH.Context.Player.Mongo.Entities;

public class ChatMessageEntity
{
    [BsonId]
    public ObjectId Id { get; set; }

    public string Channel { get; set; } = "Global";

    public DateTime CreatedAtUtc { get; set; }

    public string Message { get; set; } = string.Empty;

    public string SenderCharacterId { get; set; } = string.Empty;

    public string SenderDisplayName { get; set; } = string.Empty;

    public string[] Tags { get; set; } = [];

    public string? TargetCharacterId { get; set; }
}
