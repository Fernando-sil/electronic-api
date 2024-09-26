using System.Security.Cryptography;
using api.Interfaces;

namespace api.Helpers;

public class Password : IPasswordInterface
{
    public void HashPassword(string password, out byte[] hashedPassword, out byte[] salt)
    {
        using var hmac = new HMACSHA512();
        salt = hmac.Key;
        hashedPassword = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
    }

    public bool IsPasswordValid(string password, byte[] hashedPassword, byte[] salt)
    {
        using var hmac = new HMACSHA512(salt);
        var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        return computedHash.SequenceEqual(hashedPassword);
    }
}