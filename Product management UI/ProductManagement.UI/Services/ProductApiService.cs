using System.Net.Http.Json;
using ProductManagement.UI.Models;
using ProductManagement.UI.DTOs;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace ProductManagement.UI.Services;

public class ProductApiService
{
    private readonly HttpClient _httpClient;
    private readonly ProtectedSessionStorage _sessionStorage;

    public ProductApiService(
      HttpClient httpClient,
      ProtectedSessionStorage sessionStorage)
    {
        _httpClient = httpClient;
        _sessionStorage = sessionStorage;
    }


  

    public async Task<List<Product>?>GetProductsAsync()
    {
        await AddJwtTokenAsync();

        return await _httpClient
            .GetFromJsonAsync<List<Product>>(
                "api/Products");
    }


    public async Task<Product?> GetProductByIdAsync(int id)
    {
        await AddJwtTokenAsync();

        return await _httpClient.GetFromJsonAsync<Product>
        (
            $"api/Products/{id}"
        );
    }

    //public async Task CreateProductAsync(Product product)
    //{
    //    await AddJwtTokenAsync();
    //    await _httpClient.PostAsJsonAsync(
    //        "api/Products",
    //        product);
    //}

    public async Task CreateProductAsync(Product product)
    {
        await AddJwtTokenAsync();

        var userIdResult =
            await _sessionStorage.GetAsync<int>("UserId");

        if (!userIdResult.Success || userIdResult.Value <= 0)
        {
            throw new InvalidOperationException(
                "User ID was not found.");
        }

        var request = new ProductRequestDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            CreatedDate = product.CreatedDate,
            UserId = userIdResult.Value
        };

        await _httpClient.PostAsJsonAsync(
            "api/Products",
            request);
    }

    //public async Task UpdateProductAsync(Product product)
    //{
    //    await AddJwtTokenAsync();
    //    await _httpClient.PutAsJsonAsync(
    //        $"api/Products/{product.Id}",
    //        product);
    //}


    public async Task UpdateProductAsync(Product product)
    {
        await AddJwtTokenAsync();

        var userIdResult =
            await _sessionStorage.GetAsync<int>("UserId");

        if (!userIdResult.Success || userIdResult.Value <= 0)
        {
            throw new InvalidOperationException(
                "User ID was not found.");
        }

        var request = new ProductRequestDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            CreatedDate = product.CreatedDate,
            UserId = userIdResult.Value
        };

        await _httpClient.PutAsJsonAsync(
            $"api/Products/{product.Id}",
            request);
    }


    //public async Task DeleteProductAsync(int id)
    //{
    //    await AddJwtTokenAsync();
    //    await _httpClient.DeleteAsync(
    //        $"api/Products/{id}");
    //}


    public async Task DeleteProductAsync(Product product)
    {
        await AddJwtTokenAsync();

       



        var userIdResult =
         await _sessionStorage.GetAsync<int>("UserId");

        if (!userIdResult.Success || userIdResult.Value <= 0)
        {
            throw new InvalidOperationException(
                "User ID was not found.");
        }

        var request = new ProductRequestDto
        {
            Id = product.Id,
            Name = product.Name,
            UserId = userIdResult.Value
        };

        var httpRequest = new HttpRequestMessage(
            HttpMethod.Delete,
            "api/Products");

        httpRequest.Content = JsonContent.Create(request);

        await _httpClient.SendAsync(httpRequest);
    }


    private async Task AddJwtTokenAsync()
    {
        var tokenResult =
            await _sessionStorage
                .GetAsync<string>(
                    "JwtToken");

        



        if (tokenResult.Success &&
            !string.IsNullOrWhiteSpace(
                tokenResult.Value))
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(
                    "Bearer",
                    tokenResult.Value);
        }
    }



    public async Task DisableProductAsync(Product product)
    {
        await AddJwtTokenAsync();

        var userIdResult =
            await _sessionStorage.GetAsync<int>("UserId");

        if (!userIdResult.Success || userIdResult.Value <= 0)
        {
            throw new InvalidOperationException(
                "User ID was not found.");
        }

        var request = new ProductRequestDto
        {
            Id = product.Id,
            Name = product.Name,
            UserId = userIdResult.Value
        };

        await _httpClient.PatchAsJsonAsync(
            "api/Products/disable",
            request);
    }



    public async Task EnableProductAsync(Product product)
    {
        await AddJwtTokenAsync();

        var userIdResult =
            await _sessionStorage.GetAsync<int>("UserId");

        if (!userIdResult.Success || userIdResult.Value <= 0)
        {
            throw new InvalidOperationException(
                "User ID was not found.");
        }

        var request = new ProductRequestDto
        {
            Id = product.Id,
            Name = product.Name,
            UserId = userIdResult.Value
        };

        await _httpClient.PatchAsJsonAsync(
            "api/Products/enable",
            request);
    }

}