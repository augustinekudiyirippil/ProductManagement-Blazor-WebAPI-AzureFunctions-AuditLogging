using System.Net.Http.Json;
using ProductManagement.UI.DTOs;

namespace ProductManagement.UI.Services;

public class AuthenticationService
{
    private readonly HttpClient _httpClient;

    public AuthenticationService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<LoginResponseDto?> LoginAsync(
        LoginRequestDto request)
    {
        var response =
            await _httpClient.PostAsJsonAsync(
                "api/Users/login",
                request);

        if (!response.IsSuccessStatusCode)
        {
            return new LoginResponseDto
            {
                IsSuccess = false,
                Message = "Login failed"
            };
        }

        return await response.Content
            .ReadFromJsonAsync<LoginResponseDto>();
    }
}