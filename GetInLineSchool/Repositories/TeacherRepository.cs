using Dapper;
using GetInLineSchool.DTOs.Request;

namespace GetInLineSchool.Repositories
{
    public class TeacherRepository : BaseRepository
    {
        public async Task<int>CreateTeacherAsync(CreateTeacherRequest teacher)
        {
            using var connection = dbConnection;

            string sql= @"INSERT INTO Teachers (Username, Password, Email, IDSchool, Role)
                        VALUES (@Username, @Password, @Email, @IDSchool, @Role)";

            return await connection.ExecuteAsync(sql, teacher);
        }

        public async Task<int> CheckUniqueFieldsAsync(CreateTeacherRequest teacher)
        {
            using var connection = dbConnection;

            return await connection.QueryFirstOrDefaultAsync<int>
                ("SELECT COUNT(*) FROM TEACHERS WHERE Username=@Username OR Email=@Email", teacher);
        }

        public async Task<int> CheckDirectorExistsAsync(CreateTeacherRequest teacher)
        {
            using var connection = dbConnection;

            string sql = "SELECT COUNT(*) FROM TEACHERS WHERE Role=2 AND IDSchool=@IDSchool";

            return await connection.ExecuteScalarAsync<int>(sql, new { IDSchool = teacher.IDSchool });
        }
    }
}
