using Microsoft.EntityFrameworkCore;
using ProductManagement.Api.Data;
using ProductManagement.Api.Models;

namespace ProductManagement.Api.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await _context.Users
            .FirstOrDefaultAsync(
                u => u.Username == username);
    }

    public async Task<User> CreateAsync(User user)
    {
        _context.Users.Add(user);

        await _context.SaveChangesAsync();

        return user;
    }
}
