using Microsoft.EntityFrameworkCore;
using Services.Context;
using Services.Models;
using System.Security.Cryptography;
using System.Text;

namespace Services.Repositories;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);
    Task AddAsync(User user);

    // helper
    string GenerateRandomPassword(int length = 12);
    string HashPassword(string password);
    bool VerifyPassword(string password, string hash);
}

public class UserRepository : IUserRepository
{
    private readonly DataContext _db;
    public UserRepository(DataContext db) => _db = db;

    public async Task<User?> GetByEmailAsync(string email)
        => await _db.Users.FirstOrDefaultAsync(u => u.Email == email);

    public async Task AddAsync(User user)
    {
        _db.Users.Add(user);
        await _db.SaveChangesAsync();
    }

    // Generate random password (useful for seeding)
    public string GenerateRandomPassword(int length)
{
    const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%^&*";
        var bytes = new byte[length];
        RandomNumberGenerator.Fill(bytes);

        var result = new StringBuilder(length);
        for (int i = 0; i < length; i++)
        {
            result.Append(chars[bytes[i] % chars.Length]);
        }

        return result.ToString();
    }

    // Hash & verify with BCrypt
    public string HashPassword(string password)
        => BCrypt.Net.BCrypt.HashPassword(password);

    public bool VerifyPassword(string password, string hash)
        => BCrypt.Net.BCrypt.Verify(password, hash);
}
