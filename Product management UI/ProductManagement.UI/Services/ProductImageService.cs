using Microsoft.AspNetCore.Components.Forms;
using ProductManagement.UI.DTOs;
using ProductManagement.UI.Services;
using System.Net.Http.Headers;

public class ProductImageService
    : IProductImageService
{
    private readonly HttpClient _httpClient;

    public ProductImageService(
        HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    //public async Task UploadImagesAsync(
    //    int productId,
    //    List<IBrowserFile> files)
    //{
    //    using var content =
    //        new MultipartFormDataContent();

    //    foreach (var file in files)
    //    {
    //        var streamContent =
    //            new StreamContent(
    //                file.OpenReadStream(
    //                    maxAllowedSize:
    //                        20 * 1024 * 1024));

    //        streamContent.Headers.ContentType =
    //            new MediaTypeHeaderValue(
    //                file.ContentType);

    //        content.Add(
    //            streamContent,
    //            "files",
    //            file.Name);
    //    }

    //    var response =
    //        await _httpClient.PostAsync(
    //            $"api/products/{productId}/images",
    //            content);

    //    response.EnsureSuccessStatusCode();
    //}



    public async Task UploadImageAsync(
    int productId,
    IBrowserFile file,
    bool isPrimary)
    {
        using var content =
            new MultipartFormDataContent();

        var streamContent =
            new StreamContent(
                file.OpenReadStream(
                    20 * 1024 * 1024));

        content.Add(
            streamContent,
            "File",
            file.Name);

        content.Add(
            new StringContent(
                isPrimary.ToString()),
            "IsPrimary");

        var response =
            await _httpClient.PostAsync(
                $"api/products/{productId}/images",
                content);

        response.EnsureSuccessStatusCode();
    }

    public async Task<List<ProductImageDto>>
        GetImagesAsync(int productId)
    {
        return await _httpClient
            .GetFromJsonAsync<List<ProductImageDto>>(
                $"api/products/{productId}/images")
            ?? new List<ProductImageDto>();
    }


    public async Task DeleteImageAsync(
    int imageId)
    {
        var response =
            await _httpClient.DeleteAsync(
                $"api/products/images/{imageId}");

        response.EnsureSuccessStatusCode();

    }

}