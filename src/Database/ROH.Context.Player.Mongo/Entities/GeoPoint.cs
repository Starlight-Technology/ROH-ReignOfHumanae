//-----------------------------------------------------------------------
// <copyright file="GeoPoint.cs" company="Starlight-Technology">
//     Author:  
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ROH.Context.Player.Mongo.Entities;

public class GeoPoint
{
    [BsonElement("coordinates")]
    public double[] Coordinates { get; set; } = default!;

    [BsonElement("type")]
    public string Type { get; set; } = "Point";
}