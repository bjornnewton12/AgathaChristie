using System.Security.Cryptography;
using AgathaChristie.Domain.Models;
using AgathaChristie.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AgathaChristie.Infrastructure.Services;

public class AuthService
{
    private readonly AppDbContext _db;

    public AuthService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<User?> RegisterAsync(string username, string password)
    {
        var existing = await _db.Users.FirstOrDefaultAsync(u => u.Username == username);
        if (existing is not null) return null;

        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = username,
            PasswordHash = HashPassword(password),
            CreatedAt = DateTime.UtcNow
        };

        _db.Users.Add(user);
        await _db.SaveChangesAsync();
        return user;
    }

    public async Task<User?> LoginAsync(string username, string password)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Username == username);
        if (user is null || !VerifyPassword(password, user.PasswordHash)) return null;
        return user;
    }

    private static string HashPassword(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(16);
        var hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, 100_000, HashAlgorithmName.SHA256, 32);
        return $"{Convert.ToBase64String(salt)}.{Convert.ToBase64String(hash)}";
    }

    private static bool VerifyPassword(string password, string storedHash)
    {
        var parts = storedHash.Split('.');
        var salt = Convert.FromBase64String(parts[0]);
        var expectedHash = Convert.FromBase64String(parts[1]);
        var actualHash = Rfc2898DeriveBytes.Pbkdf2(password, salt, 100_000, HashAlgorithmName.SHA256, 32);
        return CryptographicOperations.FixedTimeEquals(actualHash, expectedHash);
    }
}
