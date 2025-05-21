using System.Net.Http;
using System.Net.Http.Json;
using LibraryApplication.DTOs;

namespace LibraryUI.Services;

public class UserClient
{
    private readonly HttpClient _httpClient;
    private const string BaseUrl = "https://localhost:7284/api/user/";

    public UserClient()
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(BaseUrl)
        };

        _httpClient.DefaultRequestHeaders.Accept.Clear();
        _httpClient.DefaultRequestHeaders.Accept.Add(
            new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
    }

    public async Task<UserDTO> GetLogIn(string email, string password)
    {
        var response = await _httpClient.GetAsync($"login/{email},{password}");

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<UserDTO>();
        }

        var errorMessage = await response.Content.ReadAsStringAsync();
        throw new HttpRequestException($"{response.StatusCode} - {errorMessage}");
    }

    public async Task<IEnumerable<UserDTO>> GetUsers()
    {
        var response = await _httpClient.GetAsync("users");

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<IEnumerable<UserDTO>>();
        }

        var errorMessage = await response.Content.ReadAsStringAsync();
        throw new HttpRequestException($"{response.StatusCode} - {errorMessage}");
    }

    public async Task<IEnumerable<UserDTO>> GetAdmins(string role)
    {
        var response = await _httpClient.GetAsync($"role/{role}");

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<IEnumerable<UserDTO>>();
        }

        var errorMessage = await response.Content.ReadAsStringAsync();
        throw new HttpRequestException($"{response.StatusCode} - {errorMessage}");
    }

    public async Task<IEnumerable<UserDTO>> GetUserByEmail(string email)
    {
        var response = await _httpClient.GetAsync($"email/{email}");

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<IEnumerable<UserDTO>>();
        }

        var errorMessage = await response.Content.ReadAsStringAsync();
        throw new HttpRequestException($"{response.StatusCode} - {errorMessage}");
    }

    public async Task<UserDTO> CreateUserAsync(UserCreationDTO userCreationDTO)
    {
        var response = await _httpClient.PostAsJsonAsync("", userCreationDTO);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<UserDTO>();
        }

        var errorMessage = await response.Content.ReadAsStringAsync();
        throw new HttpRequestException($"{response.StatusCode} - {errorMessage}");
    }

    public async Task UpdateUserAsync(string email, UserUpdateDTO userUpdateDTO)
    {
        var response = await _httpClient.PutAsJsonAsync($"{email}", userUpdateDTO);

        if (!response.IsSuccessStatusCode)
        {
            var errorMessage = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"{response.StatusCode} - {errorMessage}");
        }
    }

    public async Task DeleteUserAsync(string email)
    {
        var response = await _httpClient.DeleteAsync($"{email}");

        if (!response.IsSuccessStatusCode)
        {
            var errorMessage = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"{response.StatusCode} - {errorMessage}");
        }
    }

    public async Task UpdateUserConfigAsync(string id, UserUpdateDTO userUpdateDTO)
    {
        var response = await _httpClient.PutAsJsonAsync($"configuration/{id}", userUpdateDTO);

        if (!response.IsSuccessStatusCode)
        {
            var errorMessage = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"{response.StatusCode} - {errorMessage}");
        }
    }
}
