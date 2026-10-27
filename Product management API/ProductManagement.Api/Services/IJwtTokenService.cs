using ProductManagement.Api.Models;

namespace ProductManagement.Api.Services;

public interface IJwtTokenService
{
    string GenerateToken(User user);
}