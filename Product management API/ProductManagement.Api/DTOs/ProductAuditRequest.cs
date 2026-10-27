namespace ProductManagement.Api.DTOs;

public class ProductAuditRequest
{
    public int ProductId { get; set; }

    public int UserId { get; set; }

    public string ActionType { get; set; } = string.Empty;

    public string? Description { get; set; }
}