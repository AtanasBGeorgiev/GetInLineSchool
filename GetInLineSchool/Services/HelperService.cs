using Isopoh.Cryptography.Argon2;

namespace GetInLineSchool.Services
{
    public class HelperService
    {
        public static int CreateSchoolCode()
        {
            Random rnd = new Random();
            return rnd.Next(100, 1000);
        }

        public static string HashPassword(string password)
        {
            return Argon2.Hash(password);
        }
    }
}
