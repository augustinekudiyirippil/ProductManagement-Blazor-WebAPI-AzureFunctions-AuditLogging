using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ProductManagement.Api.Data;
using ProductManagement.Api.Repositories;
using ProductManagement.Api.Services;
using ProductManagement.Api.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Azure.Identity;
using Microsoft.ApplicationInsights.Extensibility;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.



var keyVaultUri =
new Uri("https://productmanagement-kv.vault.azure.net/");

builder.Configuration.AddAzureKeyVault(
    keyVaultUri,
    new DefaultAzureCredential());

builder.Services.AddApplicationInsightsTelemetry(options =>
{
    
options.ConnectionString =
 
builder.Configuration[
 
"ApplicationInsights:ConnectionString"];
     
});
Console.WriteLine("ApplicationInsights:ConnectionString:");
Console.WriteLine(
    builder.Configuration["ApplicationInsights:ConnectionString"]);



var connectionString =
    builder.Configuration.GetConnectionString(
        "DefaultConnection");

Console.WriteLine(connectionString);

Console.WriteLine(
    builder.Configuration["AzureFunctions:ProductAuditUrl"]);

Console.WriteLine(
    builder.Configuration["ServiceBusSettings:QueueName"]);

Console.WriteLine(
    builder.Configuration["ServiceBusSettings:ConnectionString"]);


builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();


builder.Services.AddScoped<IUserRepository,UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IEmailServiceBusService,EmailServiceBusService>();
builder.Services.AddScoped<IBlobStorageService,BlobStorageService>();
builder.Services.AddScoped<IProductImageRepository, ProductImageRepository>();
 

builder.Services.Configure<ServiceBusSettings>(
    builder.Configuration.GetSection(
        "ServiceBusSettings"));

builder.Services.Configure<BlobStorageSettings>(
    builder.Configuration.GetSection(
        "BlobStorage"));

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));




var jwtSection =
    builder.Configuration.GetSection(
        "Jwt");

var key =
    Encoding.UTF8.GetBytes(
        jwtSection["Key"]!);

builder.Services
    .AddAuthentication(
        JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                ValidIssuer =
                    jwtSection["Issuer"],

                ValidAudience =
                    jwtSection["Audience"],

                IssuerSigningKey =
                    new SymmetricSecurityKey(
                        key),

                ClockSkew =
                    TimeSpan.Zero
            };
    });

builder.Services.AddAuthorization();

builder.Services.AddScoped<
    IJwtTokenService,
    JwtTokenService>();



builder.Services.Configure<AzureFunctionSettings>(
    builder.Configuration.GetSection(
        "AzureFunctions"));

 

builder.Services.AddHttpClient<IAuditFunctionService,AuditFunctionService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

if (!app.Environment.IsDevelopment())
{
    app.UseAuthentication();
    app.UseAuthorization();
}

//app.UseAuthentication();

//app.UseAuthorization();

//app.UseAntiforgery();


app.MapControllers();

app.Run();