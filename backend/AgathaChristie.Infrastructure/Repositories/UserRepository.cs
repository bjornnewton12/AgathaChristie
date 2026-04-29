using AgathaChristie.Application.Interfaces;
using AgathaChristie.Domain.Models;
using AgathaChristie.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AgathaChristie.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _db;

    public UserRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await _db.Users.FirstOrDefaultAsync(u => u.Username == username);
    }

    public async Task AddAsync(User user)
    {
        _db.Users.Add(user);
        await _db.SaveChangesAsync();
    }
}