using LibraryDomain.Models;
using LibraryDomain.Repository;

namespace LibraryDomain.Interfaces;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByEmailAsync(string email);

    Task<IEnumerable<User>> GetAdminsAsync(bool role);
}
