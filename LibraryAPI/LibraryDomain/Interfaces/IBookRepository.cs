using LibraryDomain.Models;
using LibraryDomain.Repository;

namespace LibraryDomain.Interfaces;

public interface IBookRepository : IRepository<Book>
{
    Task<IEnumerable<Book>> GetByTitleAsync(string title);

    Task<Book?> GetByISBNAsync(string isbn);

    Task<IEnumerable<Book>> GetByAuthorAsync(string author);

    Task<IEnumerable<Book>> GetByGenreAsync(string genre);

    Task<IEnumerable<Book>> GetByPriceRangeAsync(decimal minimum, decimal maximum);

    Task<IEnumerable<Book>> GetByYearRangeAsync(int afterYear, int beforeYear);
}
