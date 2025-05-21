using MongoDB.Bson.Serialization.Attributes;

namespace LibraryApplication.DTOs;

public class BookCreationDTO
{
    [BsonRequired]
    public string ISBN { get; set; }

    [BsonRequired]
    public string Title { get; set; }
    public int PublicationYear { get; set; }
    public List<string> Authors { get; set; } = [];
    public List<string> Genres { get; set; } = [];

    [BsonRequired]
    public decimal Price { get; set; }

    [BsonRequired]
    public int NumberOfBooks { get; set; }
    public string? CreatedBy { get; set; }
}
