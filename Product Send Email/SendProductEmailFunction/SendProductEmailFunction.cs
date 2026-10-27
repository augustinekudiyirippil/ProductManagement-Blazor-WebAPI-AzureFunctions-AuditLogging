using System;
using System.Text.Json;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using SendProductEmailFunction.DTO;
using SendProductEmailFunction.Services;

namespace SendProductEmailFunction;

public class SendProductEmailFunction
{
    private readonly ILogger<SendProductEmailFunction> _logger;
    private readonly IEmailService _emailService;

    //public SendProductEmailFunction(ILogger<SendProductEmailFunction> logger)
    //{
    //    _logger = logger;
    //}

    public SendProductEmailFunction(
            ILogger<SendProductEmailFunction> logger,
            IEmailService emailService)
            {
            _logger = logger;
            _emailService = emailService;
            }


    //[Function(nameof(SendProductEmailFunction))]
    //public async Task Run(
    // [ServiceBusTrigger(
    //    "product-email-queue",
    //    Connection = "ServiceBusConnection")]
    //ServiceBusReceivedMessage message,
    // ServiceBusMessageActions messageActions)
    //{
    //    _logger.LogInformation("===== FUNCTION TRIGGERED =====");
    //    _logger.LogInformation(message.Body.ToString());

    //    await messageActions.CompleteMessageAsync(message);
    //}


    [Function(nameof(SendProductEmailFunction))]
    public async Task Run(
    [ServiceBusTrigger(
        "product-email-queue",
        Connection = "ServiceBusConnection")]
    ServiceBusReceivedMessage message,
    ServiceBusMessageActions messageActions)
    {
        _logger.LogInformation(
            "===== FUNCTION TRIGGERED =====");

        var emailMessage =
            JsonSerializer.Deserialize<ProductEmailMessage>(
                message.Body.ToString());

        if (emailMessage == null)
        {
            _logger.LogError(
                "Failed to deserialize message.");

            return;
        }

        var subject =
            $"Product {emailMessage.ActionType} Notification";

        var body =
        $"""
    Product Id: {emailMessage.ProductId}

    Product Name: {emailMessage.ProductName}

    Action Type: {emailMessage.ActionType}

    User Id: {emailMessage.UserId}

    Event Date: {emailMessage.EventDate:dd-MMM-yyyy HH:mm:ss}
    """;

        await _emailService.SendEmailAsync(
            subject,
            body);

        await messageActions.CompleteMessageAsync(
            message);

        _logger.LogInformation(
            "Email sent successfully.");
    }


}