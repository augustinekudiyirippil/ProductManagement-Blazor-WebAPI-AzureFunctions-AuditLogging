using System.Text;
using System.Text.Json;
using System.Text.Json;
using Microsoft.Extensions.Options;
using ProductManagement.Api.Configuration;
using ProductManagement.Api.DTOs;
using ProductManagement.Api.Services;

public class AuditFunctionService : IAuditFunctionService
{
    private readonly HttpClient _httpClient;
    private readonly AzureFunctionSettings _settings;

    public AuditFunctionService(
        HttpClient httpClient,
        IOptions<AzureFunctionSettings> settings)
    {
        _httpClient = httpClient;
        _settings = settings.Value;
    }

   


    public async Task LogAsync(ProductAuditRequest request)
    {
        var json = JsonSerializer.Serialize(request);

        Console.WriteLine("URL:");
        Console.WriteLine(_settings.ProductAuditUrl);

        Console.WriteLine("Payload:");
        Console.WriteLine(json);

        using var content = new StringContent(
            json,
            Encoding.UTF8,
            "application/json");

        var response = await _httpClient.PostAsync(
            _settings.ProductAuditUrl,
            content);

        var responseContent = await response.Content.ReadAsStringAsync();

        Console.WriteLine($"Status Code: {response.StatusCode}");
        Console.WriteLine($"Response: {responseContent}");
    }

}