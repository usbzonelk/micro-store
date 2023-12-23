using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using System.Text;

namespace UserService.Utils;

public class HashText
{

    public static string HashPass(string inputText)
    {
        byte[] salt = RandomNumberGenerator.GetBytes(128 / 8);

        string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: inputText,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100000,
            numBytesRequested: 256 / 8));
        return $"{hashed}|||{Convert.ToBase64String(salt)}";
    }
    public static bool VerifyPass(string passwordToCheck, string storedFullHash)
    {
        // string storedFullHash = HashPass(passwordToCheck);
        string[] parts = storedFullHash.Split(new string[] { "|||" }, StringSplitOptions.None);
        if (parts.Length != 2)
        {
            return false;
        }

        string storedHashedPassword = parts[0];
        string storedSalt = parts[1];

        byte[] salt = Convert.FromBase64String(storedSalt);

        string hashedEnteredPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: passwordToCheck,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100000,
            numBytesRequested: 256 / 8));

        return hashedEnteredPassword == storedHashedPassword;
    }
}