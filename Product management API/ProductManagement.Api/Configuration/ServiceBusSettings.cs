namespace ProductManagement.Api.Configuration;

public class ServiceBusSettings
{
    public string ConnectionString { get; set; } = string.Empty;

    public string QueueName { get; set; } = string.Empty;

    public string TeamsQueueName { get; set; } = string.Empty;

}