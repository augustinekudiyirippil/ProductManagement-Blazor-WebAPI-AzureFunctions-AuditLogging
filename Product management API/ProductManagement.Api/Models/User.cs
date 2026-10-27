using System.ComponentModel.DataAnnotations;

namespace ProductManagement.Api.Models;

public class User
{
    [Key]
    [Required]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Username { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public DateTime CreatedDate { get; set; }
}