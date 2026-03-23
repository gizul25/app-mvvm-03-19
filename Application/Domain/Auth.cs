using System;
using System.Text;
using System.Security.Cryptography;
using MyAvaloniaApp.Models;

namespace MyAvaloniaApp.Domain;

public class Auth
{
    public static User? Login(Database db, string username, string password)
    {
        foreach (User user in db.Users)
        {
            if (user.Username != username)
            {
                continue;
            }
            if (!CompareHashedToPlain(user.PasswordHash, password))
            {
                return null;
            }
            return user;
        }
        
        return null;
    }

    public static string GenerateHashAndSalt(string password)
    {
        byte[] salt = RandomNumberGenerator.GetBytes(128 / 8); 

        string hashedPassword = GenerateHashWithSalt(password, salt);
        return hashedPassword; 
    }

    public static bool CompareHashedToPlain(string passwordHash, string password)
    {
        byte[] hashBytes = Convert.FromBase64String(passwordHash);
        byte[] salt = new byte[16];
        Array.Copy(hashBytes, 0, salt, 0, 16);

        string currentHash = GenerateHashWithSalt(password, salt);
        return passwordHash == currentHash;
    }

    public static string GenerateHashWithSalt(string password, byte[] salt)
    {
        var utf8 = new UTF8Encoding();
        byte[] passwordBytes = utf8.GetBytes(password);
        byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
            passwordBytes,
            salt,
            100000,
            HashAlgorithmName.SHA1,
            128 / 8
        );

        byte[] hashBytes = new byte[32];
        Array.Copy(salt, 0, hashBytes, 0, 16);
        Array.Copy(hash, 0, hashBytes, 16, 16);

        return Convert.ToBase64String(hashBytes);
    }
}