//-----------------------------------------------------------------------
// <copyright file="PlayerPositionGeo.cs" company="Starlight-Technology">
//     Author:  
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ROH.Context.Player.Mongo.Entities;

public class PlayerPositionGeo
{
    public string AccountId { get; set; } = string.Empty;

    public string CharacterId { get; set; } = string.Empty;

    [BsonElement("distance")]
    public double DistanceMeters { get; set; }

    [BsonId]
    public ObjectId Id { get; set; }

    public string PlayerId { get; set; } = default!;

    /// <summary>
    /// GeoJSON Point X = longitude (PositionX) Z = latitude  (PositionZ)
    /// </summary>
    public GeoPoint Position { get; set; } = default!;

    /// <summary>
    /// Altura tratada separadamente
    /// </summary>
    public float PositionY { get; set; }

    public float RotationW { get; set; }

    public float RotationX { get; set; }

    public float RotationY { get; set; }

    public float RotationZ { get; set; }

    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAtUtc { get; set; } = DateTime.UtcNow;

    public string WorldId { get; set; } = string.Empty;
}
