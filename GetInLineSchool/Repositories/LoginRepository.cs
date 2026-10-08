using Dapper;
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
    }
}
