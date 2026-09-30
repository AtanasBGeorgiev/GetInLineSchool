using System.Security.Cryptography;
using System.Text;
using Konscious.Security.Cryptography;

namespace GetInLineSchool.Services
{
    public class HashingService
    {
        private const int _saltSize = 16; // 128 bit
        private const int _hashSize = 32; // 256 bit
        private const int _degreeOfParallelism = 4; // Number of threads to use for hashing
        private const int _iterations = 3; // Number of iterations
        private const int _memorySize = 65536; // 64 MB

        public static string Hash(string password)
        {
            // Generate a random salt
            byte[] salt = new byte[_saltSize];

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);//fill the salt with random bytes
            }

            //Create hash
            byte[] hash = HashPassword(password, salt);

            // Combine salt and hash
            var combinedBytes = new byte[salt.Length + hash.Length];
            Array.Copy(salt,0,combinedBytes,0, salt.Length);
            Array.Copy(hash,0,combinedBytes,salt.Length, hash.Length);

            return Convert.ToBase64String(combinedBytes);
        }

        private static byte[] HashPassword(string password, byte[] salt)
        {
            var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password))//from text to byte array
            {
                Salt = salt,
                DegreeOfParallelism = _degreeOfParallelism,
                Iterations = _iterations,
                MemorySize = _memorySize
            };

            return argon2.GetBytes(_hashSize);
        }

        public static bool Verify(string password, string hashedPassword)
        {
            //Decode the stored hash
            byte[] combinedBytes = Convert.FromBase64String(hashedPassword);

            //Extract salt and hash
            byte[] salt = new byte[_saltSize];
            byte[] hash = new byte[_hashSize];

            Array.Copy(combinedBytes,0,salt,0, _saltSize);
            Array.Copy(combinedBytes, _saltSize, hash, 0, _hashSize);

            //Compute hash for the input password
            byte[] newHash = HashPassword(password, salt);

            //Compare the hashes
            return CryptographicOperations.FixedTimeEquals(hash, newHash);
        }
    }
}
