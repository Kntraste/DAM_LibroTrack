using MongoDB.Bson.Serialization.Attributes;

namespace LibraryDomain.Models;

public class Book
{
    [BsonId]
    [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
    [BsonRequired]
    public string Id { get; set; }

    [BsonRequired]
    public string ISBN { get; set; }

    [BsonRequired]
    [BsonElement("title")]
    public string Title { get; set; }

    [BsonElement("publication_year")]
    public int PublicationYear { get; set; }

    [BsonElement("authors")]
    [BsonIgnoreIfNull]
    public List<string> Authors { get; set; } = [];

    [BsonElement("genres")]
    [BsonIgnoreIfNull]
    public List<string> Genres { get; set; } = [];

    [BsonElement("price")]
    [BsonRequired]
    public decimal Price { get; set; }

    [BsonElement("items")]
    [BsonRequired]
    public int NumberOfBooks { get; set; }

    [BsonElement("created_by")]
    [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
    public string? CreatedBy { get; set; }
}
