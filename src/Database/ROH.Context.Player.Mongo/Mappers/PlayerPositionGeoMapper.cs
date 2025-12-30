//-----------------------------------------------------------------------
// <copyright file="PlayerPositionGeoMapper.cs" company="Starlight-Technology">
//     Author:  
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using ROH.Context.Player.Mongo.Entities;

namespace ROH.Context.Player.Mongo.Mappers;

public static class PlayerPositionGeoMapper
{
    public static PlayerPositionGeo ToGeo(this PlayerPosition legacy)
    {
        var (lng, lat) = WorldProjection.Project(legacy.PositionX, legacy.PositionZ);

        return new PlayerPositionGeo
        {
            PlayerId = legacy.PlayerId,

            Position = new GeoPoint { Coordinates = new[] { lng, lat } },

            PositionY = legacy.PositionY,
            RotationX = legacy.RotationX,
            RotationY = legacy.RotationY,
            RotationZ = legacy.RotationZ,
            RotationW = legacy.RotationW,
            Timestamp = legacy.Timestamp
        };
    }

    public static PlayerPosition ToLegacy(this PlayerPositionGeo geo)
    {
        var (x, z) = WorldProjection.Unproject(geo.Position.Coordinates[0], geo.Position.Coordinates[1]);

        return new PlayerPosition
        {
            Id = geo.Id,
            PlayerId = geo.PlayerId,
            PositionX = x,
            PositionZ = z,
            PositionY = geo.PositionY,
            RotationX = geo.RotationX,
            RotationY = geo.RotationY,
            RotationZ = geo.RotationZ,
            RotationW = geo.RotationW,
            Timestamp = geo.Timestamp
        };
    }
}

public static class WorldProjection
{
    // Raio médio da Terra em metros (WGS84)
    const double EarthRadiusMeters = 6378137.0;
    const double RadToDeg = 180.0 / Math.PI;

    /// <summary>
    /// Converte posição X/Z do mundo (em metros) para coordenadas GeoJSON
    /// </summary>
    public static (double lng, double lat) Project(float worldX, float worldZ)
    {
        double lng = (worldX / EarthRadiusMeters) * RadToDeg;
        double lat = (worldZ / EarthRadiusMeters) * RadToDeg;

        return (lng, lat);
    }

    /// <summary>
    /// Conversão reversa (GeoJSON → mundo Unity)
    /// </summary>
    public static (float x, float z) Unproject(double lng, double lat)
    {
        float x = (float)(((lng * Math.PI) / 180.0) * EarthRadiusMeters);
        float z = (float)(((lat * Math.PI) / 180.0) * EarthRadiusMeters);

        return (x, z);
    }
}