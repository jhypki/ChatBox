using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Models;

public class Chat
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = null!;
    // public User[] Users { get; set; } = null!;
    public List<User> Users { get; set; } = null!;
    public List<Message> Messages { get; set; } = new List<Message>();
}