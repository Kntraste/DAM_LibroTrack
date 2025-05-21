using MongoDB.Bson.Serialization.Attributes;

namespace LibraryDomain.Models;

public class User
{
    [BsonId]
    [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
    public string Id { get; set; }

    [BsonRequired]
    [BsonElement("email")]
    public string Email { get; set; }

    [BsonElement("password")]
    [BsonRequired]
    public string Password { get; set; }

    [BsonElement("is_admin")]
    [BsonDefaultValue(false)]
    public bool Is_Admin { get; set; }

    [BsonElement("language")]
    [BsonDefaultValue("english")]
    public string Language { get; set; }

    [BsonElement("theme")]
    [BsonDefaultValue("dark")]
    public string Theme { get; set; }
}
