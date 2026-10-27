using Microsoft.AspNetCore.Identity;
using ProductManagement.Api.DTOs;
using ProductManagement.Api.Models;
using ProductManagement.Api.Repositories;

namespace ProductManagement.Api.Services;

public class UserService : IUserService
{

    private readonly IUserRepository _userRepository;
    private readonly PasswordHasher<User> _passwordHasher;


    public UserService(
     IUserRepository userRepository)
    {
        _userRepository = userRepository;

        _passwordHasher =
            new PasswordHasher<User>();
    }


    public async Task<User> RegisterAsync(
    UserRegistrationDto dto)
{
    var existingUser =
        await _userRepository.GetByUsernameAsync(
            dto.Username);

    if (existingUser != null)
    {
        throw new Exception(
            "Username already exists.");
    }

    var user = new User
    {
        Name = dto.Name,
        Email = dto.Email,
        Username = dto.Username,
        CreatedDate = DateTime.UtcNow
    };

    user.Password =
        _passwordHasher.HashPassword(
            user,
            dto.Password);

    return await _userRepository.CreateAsync(
        user);
}




    
    public async Task<User?> ValidateUserAsync(
    string username,
    string password)
    {
        var user =
            await _userRepository.GetByUsernameAsync(
                username);

        if (user == null)
        {
            return null;
        }

        var result =
            _passwordHasher.VerifyHashedPassword(
                user,
                user.Password,
                password);

        if (result ==
            PasswordVerificationResult.Success)
        {
            return user;
        }

        return null;
    }

}