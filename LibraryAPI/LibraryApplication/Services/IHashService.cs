namespace LibraryApplication.Services;

public interface IHashService
{
    string HashPassword(string password);

    bool VerifyHashedPassword(string password, string hashedPassword);
}
