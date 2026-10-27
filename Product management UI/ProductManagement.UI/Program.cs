using Azure.Identity;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using ProductManagement.UI.Components;
using ProductManagement.UI.Services;




var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


var keyVaultUri =
    new Uri("https://productmanagement-kv.vault.azure.net/");

builder.Configuration.AddAzureKeyVault(
    keyVaultUri,
    new DefaultAzureCredential());



builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddHttpClient<ProductApiService>((serviceProvider, client) =>
{
    var configuration =
        serviceProvider.GetRequiredService<IConfiguration>();

    client.BaseAddress = new Uri(
        configuration["ApiSettings:BaseUrl"]!);
});


builder.Services.AddHttpClient<AuthenticationService>(
    (serviceProvider, client) =>
    {
        var configuration =
            serviceProvider.GetRequiredService<IConfiguration>();

        client.BaseAddress =
            new Uri(
                configuration["ApiSettings:BaseUrl"]!);
    });

//builder.Services.AddScoped<AuthenticationService>();

builder.Services.AddScoped<ProtectedSessionStorage>();

//builder.Services.AddScoped<ProductApiService>();

//builder.Services.AddScoped<IProductImageService, ProductImageService>();

builder.Services.AddHttpClient<
    IProductImageService,
    ProductImageService>((serviceProvider, client) =>
    {
        var configuration =
            serviceProvider.GetRequiredService<IConfiguration>();

        client.BaseAddress =
            new Uri(
                configuration["ApiSettings:BaseUrl"]!);
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();