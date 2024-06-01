using System.Collections;
using System.Security.Cryptography;
using System.Text;

namespace ROH.Domain.Accounts;

public record User(long Id = 0, long IdAccount = 0, Guid Guid = default, string? Email = null, string? UserName = null)
{
    public virtual Account? Account { get; set; } = new Account();
    public byte[]? Salt { get; set; }
    public byte[]? PasswordHash { get; set; }

    // Method to set password hash and salt
    public void SetPassword(string password)
    {
        if (string.IsNullOrEmpty(password)) 
            throw new ArgumentException("Password cannot be null or empty.");
        Salt = new byte[16]; // Generate a 16-byte salt
        RandomNumberGenerator.Fill(Salt);

        // Combine password and salt, then hash
        byte[] combinedBytes = Encoding.UTF8.GetBytes(password + Convert.ToBase64String(Salt));
        PasswordHash = SHA256.HashData(combinedBytes);
    }

    // Method to verify password
    public bool VerifyPassword(string password)
    {
        if (string.IsNullOrEmpty(password) || PasswordHash == null || Salt == null)
            return false;
        // Combine entered password and stored salt, then hash
        byte[] combinedBytes = Encoding.UTF8.GetBytes(password + Convert.ToBase64String(Salt));
        byte[] enteredPasswordHash = SHA256.HashData(combinedBytes);

        // Compare computed hash with stored hash
        return StructuralComparisons.StructuralEqualityComparer.Equals(PasswordHash, enteredPasswordHash);
    }
}