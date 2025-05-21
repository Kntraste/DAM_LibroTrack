namespace LibraryApplication.DTOs;

public class BookDTO
{
    public string Id { get; set; }
    public string ISBN { get; set; }
    public string Title { get; set; }
    public int PublicationYear { get; set; }
    public string? Authors { get; set; } = null;
    public string? Genres { get; set; } = null;
    public decimal Price { get; set; }
    public int NumberOfBooks { get; set; }
    public UserDTO? CreatedBy { get; set; }
}
