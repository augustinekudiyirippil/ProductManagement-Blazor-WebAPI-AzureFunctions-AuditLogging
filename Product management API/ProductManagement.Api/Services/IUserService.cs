using ProductManagement.Api.DTOs;
using ProductManagement.Api.Models;

namespace ProductManagement.Api.Services;

public interface IUserService
{
    Task<User> RegisterAsync(
        UserRegistrationDto dto);

    Task<User?> ValidateUserAsync(
       string username,
       string password);
}