using Azure.Identity;
using Azure.Monitor.OpenTelemetry.Exporter;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Azure.Functions.Worker.OpenTelemetry;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenTelemetry;
using SendProductEmailFunction.Services;

var builder = FunctionsApplication.CreateBuilder(args);

try
{
    var keyVaultUri =
        new Uri("https://productmanagement-kv.vault.azure.net/");

    builder.Configuration.AddAzureKeyVault(
        keyVaultUri,
        new DefaultAzureCredential());

    Console.WriteLine(
        "ServiceBusConnection exists: " +
        !string.IsNullOrEmpty(
            builder.Configuration["ServiceBusConnection"]));

    Console.WriteLine(
    "Queue Name: " +
    builder.Configuration["ServiceBusSettings:QueueName"]);
}
catch (Exception ex)
{
    Console.WriteLine(ex.ToString());
}

builder.ConfigureFunctionsWebApplication();

if (!string.IsNullOrEmpty(
    Environment.GetEnvironmentVariable(
        "APPLICATIONINSIGHTS_CONNECTION_STRING")))
{
    builder.Services.AddOpenTelemetry()
        .UseFunctionsWorkerDefaults()
        .UseAzureMonitorExporter();
}

builder.Services.AddSingleton<
    IEmailService,
    EmailService>();

builder.Build().Run();