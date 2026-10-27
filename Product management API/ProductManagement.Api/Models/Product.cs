using System.ComponentModel.DataAnnotations;

namespace ProductManagement.Api.Models;

public class Product
{
    [Key]
    [Required]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public decimal Price { get; set; } = 0;

    public DateTime CreatedDate { get; set; }
    public bool IsAvailable { get; set; }

}