namespace ProductManagement.UI.DTOs;

public class LoginResponseDto
{
    public bool IsSuccess { get; set; }

    public string Message { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;
    public int UserId { get; set; }

    public string Username { get; set; } = string.Empty;

    public string Token { get; set; } = string.Empty;


}