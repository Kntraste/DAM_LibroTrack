using System.Text.RegularExpressions;
using LibraryDomain.Interfaces;
using LibraryDomain.Models;
using LibraryDomain.Repository;
using LibraryDomain.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace LibraryInfrastructure.Repository;

public class BookRepository : Repository<Book>, IBookRepository
{
    protected readonly IMongoCollection<User> _userCollection;

    public BookRepository(IOptions<MongoDBSettings> settings) : base(settings)
    {
    }

    public async Task<Book?> GetByISBNAsync(string isbn) =>
        await _collection.Find(b => b.ISBN == isbn.Replace("-", "").Replace(" ", "")).FirstOrDefaultAsync();

    public async Task<IEnumerable<Book>> GetByTitleAsync(string title)
    {
        var filter = Builders<Book>.Filter.Regex(b => b.Title, new BsonRegularExpression($".*{Regex.Escape(title)}.*", "i"));

        return await _collection.Find(filter).ToListAsync();
    }

    public async Task<IEnumerable<Book>> GetByAuthorAsync(string author)
    {
        var filter = Builders<Book>.Filter.Regex(b => b.Authors, new BsonRegularExpression($".*{Regex.Escape(author)}.*", "i"));

        return await _collection.Find(filter).ToListAsync();
    }

    public async Task<IEnumerable<Book>> GetByGenreAsync(string genre)
    {
        var filter = Builders<Book>.Filter.Regex(b => b.Genres, new BsonRegularExpression($".*{Regex.Escape(genre)}.*", "i"));

        return await _collection.Find(filter).ToListAsync();
    }

    public async Task<IEnumerable<Book>> GetByPriceRangeAsync(decimal minimum, decimal maximum) =>
        await _collection.Find(b => b.Price <= maximum && b.Price >= minimum).ToListAsync();

    public async Task<IEnumerable<Book>> GetByYearRangeAsync(int afterYear, int beforeYear) =>
        await _collection.Find(b => b.PublicationYear <= beforeYear && b.PublicationYear >= afterYear).ToListAsync();
}
