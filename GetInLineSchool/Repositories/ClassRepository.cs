using System.Runtime.CompilerServices;
using Dapper;
using GetInLineSchool.Models;

namespace GetInLineSchool.Repositories
{
    public class ClassRepository : BaseRepository
    {
        public async Task<int> CreateMultipleSchoolsAsync(List<Classes> parameters)
        {
            using var connection = dbConnection;

            string sql = @"INSERT INTO Classes (IDSchool, Year, Class, IDClassCategory)
                            VALUES
                            (@IDSchool, @Year, @Class, @IDClassCategory);";

            return await connection.ExecuteAsync(sql, parameters);
        }
        public async Task<byte> GetCategoryDurationAsync(byte idClassCategory)
        {
            using var connection = dbConnection;

            string sql = @"SELECT Duration FROM ClassCategories WHERE IDClassCategory=@IDClassCategory";

            return await connection.ExecuteScalarAsync<byte>(sql, new { IDClassCategory = idClassCategory });
        }
    }
}
