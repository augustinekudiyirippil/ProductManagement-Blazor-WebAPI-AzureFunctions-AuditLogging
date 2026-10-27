using ProductManagement.Api.Models;

namespace ProductManagement.Api.Repositories;

public interface IUserRepository
{
    Task<User?> GetByUsernameAsync(string username);

    Task<User> CreateAsync(User user);


}