using LibraryDomain.Interfaces;
using LibraryDomain.Models;
using LibraryDomain.Repository;
using LibraryDomain.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace LibraryInfrastructure.Repository;

public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(IOptions<MongoDBSettings> settings) : base(settings)
    {
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _collection.Find(u => u.Email == email).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<User>> GetAdminsAsync(bool role)
    {
        return await _collection.Find(u => u.Is_Admin == role).ToListAsync();
    }
}
