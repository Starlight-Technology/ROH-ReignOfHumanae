using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROH.Context.Player.Mongo.Entities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class PlayerPosition
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    public string PlayerId { get; set; }
    public Position Position { get; set; }
    public Rotation Rotation { get; set; }
}
