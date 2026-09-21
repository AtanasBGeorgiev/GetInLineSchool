using Dapper;
using GetInLineSchool.Models;

namespace GetInLineSchool.Repositories
{
    public class TeacherRepository : BaseRepository
    {
        public async Task<int>CreateTeacherAsync(Teacher teacher)
        {
            using var connection = dbConnection;

            string sql= @"INSERT INTO Teachers (Username, Password, Email, IDSchool, Role)
                        VALUES (@Username, @Password, @Email, @IDSchool, @Role)";

            return await connection.ExecuteAsync(sql, teacher);
        }

        public async Task<int> CheckUniqueFieldsAsync(Teacher teacher)
        {
            using var connection = dbConnection;

            return await connection.QueryFirstOrDefaultAsync<int>
                ("SELECT COUNT(*) FROM TEACHERS WHERE Username=@Username OR Email=@Email", teacher);
        }
    }
}
