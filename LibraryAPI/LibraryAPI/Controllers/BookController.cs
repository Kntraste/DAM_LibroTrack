using AutoMapper;
using FluentValidation;
using LibraryApplication.DTOs;
using LibraryDomain.Interfaces;
using LibraryDomain.Models;
using Microsoft.AspNetCore.Mvc;

namespace LibraryAPI.Controllers;

[Route("api/book")]
[ApiController]
public class BookController : ControllerBase
{
    private readonly IBookRepository _bookRepository;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IValidator<BookCreationDTO> _validator;

    public BookController(IBookRepository bookRepository, IMapper mapper, IValidator<BookCreationDTO> validator, IUserRepository userRepository)
    {
        this._bookRepository = bookRepository;
        this._mapper = mapper;
        this._validator = validator;
        _userRepository = userRepository;
    }

    [HttpGet("books")]
    public async Task<IEnumerable<BookDTO>> GetAllBooks()
    {
        var books = await _bookRepository.GetAllAsync();

        return _mapper.Map<IEnumerable<BookDTO>>(books);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<BookDTO>> GetBookById(string id)
    {
        var book = await _bookRepository.GetByIdAsync(id);

        if (book == null)
        {
            return NotFound();
        }

        var bookDTO = _mapper.Map<BookDTO>(book);

        if (!string.IsNullOrEmpty(book.CreatedBy))
        {
            var user = await _userRepository.GetByIdAsync(book.CreatedBy);
            if (user != null)
                bookDTO.CreatedBy = _mapper.Map<UserDTO>(user);
        }

        return Ok(bookDTO);
    }

    [HttpGet("isbn/{isbn}")]
    public async Task<IEnumerable<BookDTO>> GetBookByISBN(string isbn)
    {
        var book = await _bookRepository.GetByISBNAsync(isbn);

        if (book == null)
        {
            return Enumerable.Empty<BookDTO>();
        }

        List<Book> books = new List<Book> { book };

        return _mapper.Map<List<BookDTO>>(books);
    }

    [HttpGet("title/{title}")]
    public async Task<IEnumerable<BookDTO>> GetBookByTitle(string title)
    {
        var books = await _bookRepository.GetByTitleAsync(title);

        return _mapper.Map<IEnumerable<BookDTO>>(books);
    }

    [HttpGet("author/{author}")]
    public async Task<IEnumerable<BookDTO>> GetBooksByAuthor(string author)
    {
        var books = await _bookRepository.GetByAuthorAsync(author);

        return _mapper.Map<IEnumerable<BookDTO>>(books);
    }

    [HttpGet("genre/{genre}")]
    public async Task<IEnumerable<BookDTO>> GetBookByGenre(string genre)
    {
        var books = await _bookRepository.GetByGenreAsync(genre);

        return _mapper.Map<IEnumerable<BookDTO>>(books);
    }

    [HttpGet("price-range/{min:decimal},{max:decimal}")]
    public async Task<IEnumerable<BookDTO>> GetBooksByPriceRange(decimal min, decimal max)
    {
        var books = await _bookRepository.GetByPriceRangeAsync(min, max);

        return _mapper.Map<IEnumerable<BookDTO>>(books);
    }

    [HttpGet("year-range/{afterYear:int},{beforeYear:int}")]
    public async Task<IEnumerable<BookDTO>> GetBooksByYearRange(int afterYear, int beforeYear)
    {
        var books = await _bookRepository.GetByYearRangeAsync(afterYear, beforeYear);

        return _mapper.Map<IEnumerable<BookDTO>>(books);
    }

    [HttpPost]
    public async Task<ActionResult> PostBook(BookCreationDTO bookCreationDTO)
    {
        bookCreationDTO.ISBN = bookCreationDTO.ISBN.Replace("-", "").Replace(" ", "");
        var result = _validator.Validate(bookCreationDTO);

        if (!result.IsValid)
        {
            return BadRequest(result.ToDictionary());
        }

        var bookExists = await _bookRepository.GetByISBNAsync(bookCreationDTO.ISBN);

        if (bookExists != null)
        {
            return BadRequest("This ISBN is used");
        }

        var book = _mapper.Map<Book>(bookCreationDTO);
        await _bookRepository.CreateAsync(book);
        var bookDTO = _mapper.Map<BookDTO>(book);

        return CreatedAtAction(nameof(GetBookById), new { id = book.Id }, bookDTO);
    }

    [HttpPut("{isbn}")]
    public async Task<ActionResult> PutBook(string isbn, BookCreationDTO bookCreationDTO)
    {
        bookCreationDTO.ISBN = bookCreationDTO.ISBN.Replace("-", "").Replace(" ", "");
        var result = _validator.Validate(bookCreationDTO);

        if (!result.IsValid)
        {
            return BadRequest(result.ToDictionary());
        }

        var bookExists = await _bookRepository.GetByISBNAsync(bookCreationDTO.ISBN);

        if (bookExists == null)
        {
            return NotFound();
        }

        var book = _mapper.Map<Book>(bookCreationDTO);
        book.Id = bookExists.Id;
        book.CreatedBy = bookExists.CreatedBy;

        await _bookRepository.UpdateAsync(book.Id, book);

        return NoContent();
    }

    [HttpDelete("{isbn}")]
    public async Task<ActionResult> DeleteBook(string isbn)
    {
        isbn = isbn.Replace("-", "").Replace(" ", "");
        var bookExists = await _bookRepository.GetByISBNAsync(isbn);

        if (bookExists == null)
        {
            return NotFound();
        }

        var result = await _bookRepository.DeleteAsync(bookExists.Id);

        if (result == 0)
        {
            return NotFound();
        }

        return NoContent();
    }
}
