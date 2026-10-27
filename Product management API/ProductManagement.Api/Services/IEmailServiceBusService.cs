using ProductManagement.Api.DTOs;

namespace ProductManagement.Api.Services;

public interface IEmailServiceBusService
{
    Task SendAsync(ProductEmailMessage message);
}