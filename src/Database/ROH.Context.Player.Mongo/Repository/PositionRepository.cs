using MongoDB.Driver;

using ROH.Context.Player.Mongo.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROH.Context.Player.Mongo.Repository;
public class PositionRepository
{
    private readonly IMongoCollection<PlayerPosition> _collection;

    public PositionRepository(PlayerMongoContext context)
    {
        _collection = context.PlayerPositionCollection;
    }

    public async Task SavePlayerPosition(PlayerPosition data)
    {
        await _collection.InsertOneAsync(data);
    }

    public async Task<List<PlayerPosition>> GetAllPlayers()
    {
        return await _collection.Find(_ => true).ToListAsync();
    }

}
