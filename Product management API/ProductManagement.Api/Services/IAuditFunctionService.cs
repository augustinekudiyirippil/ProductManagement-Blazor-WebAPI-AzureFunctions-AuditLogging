using ProductManagement.Api.DTOs;

namespace ProductManagement.Api.Services
{
    
    public interface IAuditFunctionService
    {
        Task LogAsync(
            ProductAuditRequest request);
    }
}
