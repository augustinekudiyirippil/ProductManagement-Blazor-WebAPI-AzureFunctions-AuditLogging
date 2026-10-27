using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProductLogs;
using ProductLogs.Services;
using Azure.Identity;


var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

var keyVaultUri =
    new Uri("https://productmanagement-kv.vault.azure.net/");

builder.Configuration.AddAzureKeyVault(
    keyVaultUri,
    new DefaultAzureCredential());


builder.Services
    .AddApplicationInsightsTelemetryWorkerService()
    .ConfigureFunctionsApplicationInsights();

builder.Services.AddDbContext<LoggingDbContext>(
    options =>
        options.UseSqlServer(
            builder.Configuration.GetConnectionString(
                "DefaultConnection")));

builder.Services.AddHttpClient<AuditFunctionService>();





builder.Build().Run();
