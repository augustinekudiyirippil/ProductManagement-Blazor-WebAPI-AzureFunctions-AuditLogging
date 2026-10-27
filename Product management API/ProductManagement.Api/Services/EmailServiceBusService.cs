using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Options;
using ProductManagement.Api.Configuration;
using ProductManagement.Api.DTOs;
using System.Text.Json;

namespace ProductManagement.Api.Services;

public class EmailServiceBusService
    : IEmailServiceBusService
{
    private readonly ServiceBusSettings _settings;

    public EmailServiceBusService(
        IOptions<ServiceBusSettings> settings)
    {
        _settings = settings.Value;
    }
    public async Task SendAsync(
    ProductEmailMessage message)
    {
        await using var client =
            new ServiceBusClient(
                _settings.ConnectionString);

        ServiceBusSender emailSender =
            client.CreateSender(
                _settings.QueueName);

        ServiceBusSender teamsSender =
            client.CreateSender(
                _settings.TeamsQueueName);

        var json =
            JsonSerializer.Serialize(message);

        var emailMessage =
            new ServiceBusMessage(json);

        await emailSender.SendMessageAsync(
            emailMessage);

        await teamsSender.SendMessageAsync(
            new ServiceBusMessage(json));

        Console.WriteLine(
            $"Message sent to {_settings.QueueName}");

        Console.WriteLine(
            $"Message sent to {_settings.TeamsQueueName}");
    }



}