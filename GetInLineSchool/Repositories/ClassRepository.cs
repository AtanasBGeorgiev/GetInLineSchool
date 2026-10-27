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

            string sql = @"INSERT INTO Classes (IDSchool, Year, Class, IDClassCategory, ClassCode)
                            VALUES
                            (@IDSchool, @Year, @Class, @IDClassCategory, @ClassCode);";

            return await connection.ExecuteAsync(sql, parameters);
        }
        public async Task<byte> GetCategoryDurationAsync(byte idClassCategory)
        {
            using var connection = dbConnection;

            string sql = @"SELECT Duration FROM ClassCategories WHERE IDClassCategory=@IDClassCategory";

            return await connection.ExecuteScalarAsync<byte>(sql, new { IDClassCategory = idClassCategory });
        }
        public async Task<List<int>> GetClassCodes(short idSchool)
        {
            using var connection = dbConnection;

            var result = await connection.QueryAsync<int>("SELECT ClassCode FROM Classes WHERE IDSchool=@IDSchool AND IsActive = 1", new { IDSchool = idSchool });

            return result.ToList();
        }
    }
}
