using Dapper;
using GetInLineSchool.DTOs.Request;
using GetInLineSchool.Models;

namespace GetInLineSchool.Repositories
{
    public class LoginRepository : BaseRepository
    {
        public async Task<User> LoginTeacherAsync(string username)
        {
            using var connection = dbConnection;

            string query = "SELECT * FROM Teachers WHERE Username=@Username";

            return await connection.QueryFirstOrDefaultAsync<Teacher>(query, new { Username = username });
        }
        public async Task<User> LoginStudentAsync(string username)
        {
            using var connection = dbConnection;

            string query = "SELECT * FROM Students WHERE Username=@Username";

            return await connection.QueryFirstOrDefaultAsync<Student>(query, new { Username = username });
        }
        public async Task<User> LoginAdminAsync(string username)
        {
            using var connection = dbConnection;

            string query = "SELECT * FROM Admins WHERE Username=@Username";

            return await connection.QueryFirstOrDefaultAsync<Admin>(query, new { Username = username });
        }

        public async Task<VerificationCode> VerifyEmailAsync(VerifyEmailRequest request)
        {
            using var connection = dbConnection;

            string sql = @"SELECT * FROM VerificationCodes WHERE Email=@Email AND IsUsed=0";

            return await connection.QueryFirstOrDefaultAsync<VerificationCode>(sql, new { Email = request.Email });
        }

        public async Task<int> MarkCodeAsUsedAsync(int idCode)
        {
            using var connection = dbConnection;

            string sql = @"UPDATE VerificationCodes SET IsUsed=1 WHERE IDCode=@IDCode";

            return await connection.ExecuteAsync(sql, new { IDCode = idCode });
        }
        public async Task<int> AddAttemptAsync(int idCode)
        {
            using var connection = dbConnection;

            string sql = @"UPDATE VerificationCodes SET Attempts=Attempts+1 WHERE IDCode=@IDCode";

            return await connection.ExecuteAsync(sql, new { IDCode = idCode });
        }
        public async Task<string> GetEmailAsync(string id, string role)
        {
            using var connection = dbConnection;

            string sql = role switch
            {
                "1" => @"SELECT Email FROM Admins WHERE IDAdmin=@IDAdmin",
                "2" or "3" or "4" => @"SELECT Email FROM Teachers WHERE IDTeacher=@IDTeacher",
                "5" => @"SELECT Email FROM Students WHERE IDStudent=@IDStudent"
            };

            switch (role)
            {
                case "1":
                    return await connection.QueryFirstOrDefaultAsync<string>(sql, new { IDAdmin = id });
                case "2":
                case "3":
                case "4":
                    return await connection.QueryFirstOrDefaultAsync<string>(sql, new { IDTeacher = id });
                case "5":
                    return await connection.QueryFirstOrDefaultAsync<string>(sql, new { IDStudent = id });
                default:
                    return null;
            }
        }
    }
}
