namespace LibraryApplication.DTOs;

public class UserUpdateDTO : UserBaseDTO
{
    public string? Language { get; set; }
    public string? Theme { get; set; }
}
