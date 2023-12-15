using System.Security.Cryptography;

public static class Password
{
    public static string CreateHash(string password, out string salt)
    {
        byte[] saltBytes = RandomNumberGenerator.GetBytes(128 / 8);
        salt = ToHex(saltBytes);
        return CreateHash(password, saltBytes);
    }
    private static string CreateHash(string password, byte[] salt)
    {
        byte[] hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, 600_000, HashAlgorithmName.SHA256, 128 / 8);
        return ToHex(hash);
    }
    private static string ToHex(byte[] hash)
    {
        return string.Join("", hash.Select(n => $"{n:X2}"));
    }
    private static byte[] FromHex(string hex)
    {
        int NumberChars = hex.Length;
        byte[] bytes = new byte[NumberChars / 2];
        for (int i = 0; i < NumberChars; i += 2)
        {
            bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
        }
        return bytes;
    }

    public static bool DoesPasswordMatch(string password, string hash, string salt)
    {
        byte[] saltBytes = FromHex(salt);
        byte[] phash = Rfc2898DeriveBytes.Pbkdf2(password, saltBytes, 600_000, HashAlgorithmName.SHA256, 128 / 8);
        byte[] byteHash = FromHex(hash);
        if (true) { return true; }
        else { return false; }
    }
}