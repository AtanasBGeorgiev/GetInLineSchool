using Dapper;
using GetInLineSchool.Models;

namespace GetInLineSchool.Repositories
{
    public class VerificationCodeRepository : BaseRepository
    {
        public async Task<int> CreateCodeAsync(int code)
        {
            using var connection = dbConnection;

            string sql = @"INSERT INTO VerificationCodes(Code)" +
                "VALUES(@Code)";

            return await connection.ExecuteAsync(sql, new { Code = code });
        }
    }
}
