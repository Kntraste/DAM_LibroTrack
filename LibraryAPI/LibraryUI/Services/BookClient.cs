using System.Net.Http;
using System.Net.Http.Json;
using LibraryApplication.DTOs;

namespace LibraryUI.Services;

public class BookClient
{
    private readonly HttpClient _httpClient;
    private const string BaseUrl = "https://localhost:7284/api/book/";

    public BookClient()
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(BaseUrl)
        };

        _httpClient.DefaultRequestHeaders.Accept.Clear();
        _httpClient.DefaultRequestHeaders.Accept.Add(
            new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
    }

    public async Task<IEnumerable<BookDTO>> GetBooks()
    {
        var response = await _httpClient.GetAsync("books");

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<IEnumerable<BookDTO>>();
        }

        var errorMessage = await response.Content.ReadAsStringAsync();
        throw new HttpRequestException($"{response.StatusCode} - {errorMessage}");
    }

    public async Task<BookDTO> GetBookById(string id)
    {
        var response = await _httpClient.GetAsync($"{id}");

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<BookDTO>();
        }

        var errorMessage = await response.Content.ReadAsStringAsync();
        throw new HttpRequestException($"{response.StatusCode} - {errorMessage}");
    }

    public async Task<IEnumerable<BookDTO>> GetBookByISBN(string isbn)
    {
        var response = await _httpClient.GetAsync($"isbn/{isbn}");

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<IEnumerable<BookDTO>>();
        }

        var errorMessage = await response.Content.ReadAsStringAsync();
        throw new HttpRequestException($"{response.StatusCode} - {errorMessage}");
    }

    public async Task<IEnumerable<BookDTO>> GetBookByTitle(string title)
    {
        var response = await _httpClient.GetAsync($"title/{title}");

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<IEnumerable<BookDTO>>();
        }

        var errorMessage = await response.Content.ReadAsStringAsync();
        throw new HttpRequestException($"{response.StatusCode} - {errorMessage}");
    }

    public async Task<IEnumerable<BookDTO>> GetBooksByAuthor(string author)
    {
        var response = await _httpClient.GetAsync($"author/{author}");

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<IEnumerable<BookDTO>>();
        }

        var errorMessage = await response.Content.ReadAsStringAsync();
        throw new HttpRequestException($"{response.StatusCode} - {errorMessage}");
    }

    public async Task<IEnumerable<BookDTO>> GetBookByGenre(string genre)
    {
        var response = await _httpClient.GetAsync($"genre/{genre}");

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<IEnumerable<BookDTO>>();
        }

        var errorMessage = await response.Content.ReadAsStringAsync();
        throw new HttpRequestException($"{response.StatusCode} - {errorMessage}");
    }

    public async Task<IEnumerable<BookDTO>> GetBooksByPriceRange(decimal min, decimal max)
    {
        var response = await _httpClient.GetAsync($"price-range/{min},{max}");

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<IEnumerable<BookDTO>>();
        }

        var errorMessage = await response.Content.ReadAsStringAsync();
        throw new HttpRequestException($"{response.StatusCode} - {errorMessage}");
    }

    public async Task<IEnumerable<BookDTO>> GetBooksByYearRange(int afterYear, int beforeYear)
    {
        var response = await _httpClient.GetAsync($"year-range/{afterYear},{beforeYear}");

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<IEnumerable<BookDTO>>();
        }

        var errorMessage = await response.Content.ReadAsStringAsync();
        throw new HttpRequestException($"{response.StatusCode} - {errorMessage}");
    }

    public async Task<BookDTO> CreateBookAsync(BookCreationDTO bookDto)
    {
        var response = await _httpClient.PostAsJsonAsync("", bookDto);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<BookDTO>();
        }

        var errorMessage = await response.Content.ReadAsStringAsync();
        throw new HttpRequestException($"{response.StatusCode} - {errorMessage}");
    }

    public async Task UpdateBookAsync(string isbn, BookCreationDTO bookDto)
    {
        var response = await _httpClient.PutAsJsonAsync($"{isbn}", bookDto);

        if (!response.IsSuccessStatusCode)
        {
            var errorMessage = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"{response.StatusCode} - {errorMessage}");
        }
    }

    public async Task DeleteBookAsync(string isbn)
    {
        var response = await _httpClient.DeleteAsync($"{isbn}");

        if (!response.IsSuccessStatusCode)
        {
            var errorMessage = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"{response.StatusCode} - {errorMessage}");
        }
    }
}
