using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Models;

public class Message
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [BsonIgnoreIfDefault]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    public string User { get; set; } = null!;
    public string Text { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
}