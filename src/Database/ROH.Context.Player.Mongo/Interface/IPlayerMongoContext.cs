//-----------------------------------------------------------------------
// <copyright file="IPlayerMongoContext.cs" company="Starlight-Technology">
//     Author:  
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using MongoDB.Driver;

using ROH.Context.Player.Mongo.Entities;

namespace ROH.Context.Player.Mongo.Interface;

public interface IPlayerMongoContext
{
    IMongoCollection<PlayerPosition> PlayerPositionCollection { get; }

    IMongoCollection<PlayerPositionGeo> PlayerPositionGeoCollection { get; }
}