using Dapper;
using GetInLineSchool.Models;

namespace GetInLineSchool.Repositories
{
    public class CodeRepository : BaseRepository
    {
        public async Task<int> CreateCodeAsync(string code, string email)
        {
            using var connection = dbConnection;

            string sql = @"INSERT INTO VerificationCodes(Code,Email)" +
                "VALUES(@Code, @Email)";

            return await connection.ExecuteAsync(sql, new { Code = code, Email = email });
        }

        public async Task<VerificationCode> VerifyEmailAsync(string email)
        {
            using var connection = dbConnection;

            string sql = @"SELECT TOP 1 * FROM VerificationCodes WHERE Email=@Email AND IsUsed=0 AND ExpirationDate > DATEDIFF_BIG(
                            SECOND,
                            '2025-01-01',
                            CONVERT(
                                DATETIME2,
                                SYSUTCDATETIME()
                                AT TIME ZONE 'UTC'
                                AT TIME ZONE 'E. Europe Standard Time'
                            )
                            )ORDER BY ExpirationDate DESC";

            return await connection.QueryFirstOrDefaultAsync<VerificationCode>(sql, new { Email = email });
        }
        public async Task<VerificationCode> GetLastEmailAsync(string email)
        {
            using var connection = dbConnection;

            string sql = @"SELECT TOP 1 * FROM VerificationCodes WHERE Email=@Email ORDER BY ExpirationDate DESC";

            return await connection.QueryFirstOrDefaultAsync<VerificationCode>(sql, new { Email = email });
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

            switch (role)
            {
                case "1":
                    return await connection.QueryFirstOrDefaultAsync<string>(@"SELECT Email FROM Admins WHERE IDAdmin=@IDAdmin", new { IDAdmin = id });
                case "2":
                case "3":
                case "4":
                    return await connection.QueryFirstOrDefaultAsync<string>(@"SELECT Email FROM Teachers WHERE IDTeacher=@IDTeacher", new { IDTeacher = id });
                case "5":
                    return await connection.QueryFirstOrDefaultAsync<string>(@"SELECT Email FROM Students WHERE IDStudent=@IDStudent", new { IDStudent = id });
                default:
                    return null;
            }
        }
        public async Task<(string Email, bool IsActive)> CheckIsUserActiveAsync(string id, string role)
        {
            using var connection = dbConnection;

            switch (role)
            {
                case "1":
                    return await connection.QueryFirstOrDefaultAsync<(string Email, bool IsActive)>(@"SELECT Email, IsActive FROM Admins WHERE IDAdmin=@IDAdmin", new { IDAdmin = id });
                case "2":
                case "3":
                case "4":
                    return await connection.QueryFirstOrDefaultAsync<(string Email, bool IsActive)>(@"SELECT Email, IsActive FROM Teachers WHERE IDTeacher=@IDTeacher", new { IDTeacher = id });
                case "5":
                    return await connection.QueryFirstOrDefaultAsync<(string Email, bool IsActive)>(@"SELECT Email, IsActive FROM Students WHERE IDStudent=@IDStudent", new { IDStudent = id });
                default:
                    return (null, false);
            }
        }
        public async Task<string> ActivateUserAsync(string id, string role)
        {
            using var connection = dbConnection;

            switch (role)
            {
                case "1":
                    return await connection.QueryFirstOrDefaultAsync<string>(@"UPDATE Admins SET IsActive=1 WHERE IDAdmin=@IDAdmin", new { IDAdmin = id });
                case "2":
                case "3":
                case "4":
                    return await connection.QueryFirstOrDefaultAsync<string>(@"UPDATE Teachers SET IsActive=1 WHERE IDTeacher=@IDTeacher", new { IDTeacher = id });
                case "5":
                    return await connection.QueryFirstOrDefaultAsync<string>(@"UPDATE Students SET IsActive=1 WHERE IDStudent=@IDStudent", new { IDStudent = id });
                default:
                    return null;
            }
        }
    }
}
