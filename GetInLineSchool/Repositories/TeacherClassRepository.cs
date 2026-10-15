using Dapper;
using GetInLineSchool.Models;

namespace GetInLineSchool.Repositories
{
    public class TeacherClassRepository : BaseRepository
    {
        public async Task<int> CreateTeacherClassesAsync(List<TeacherClass> parameters)
        {
            using var connection = dbConnection;

            string sql = @"INSERT INTO TeacherClasses (IDTeacher,IDClass,Subject)" +
                "VALUES" +
                "(@IDTeacher,@IDClass,@Subject)";

            return await connection.ExecuteAsync(sql, parameters);
        }

        public async Task<List<short>> GetTeacherQualifications(int idTeacher)
        {
            using var connection = dbConnection;

            string sql = @"SELECT IDClassCategory FROM TeacherQualifications WHERE IDTeacher=@IDTeacher";

            return (await connection.QueryAsync<short>(sql, new { idTeacher })).ToList();
        }

        public async Task<int>GetSchoolIdByTeacherIdAsync(int idTeacher)
        {
            using var connection = dbConnection;

            string sql = @"SELECT IDSchool FROM Teachers WHERE IDTeacher=@IDTeacher";

            return await connection.QueryFirstOrDefaultAsync<int>(sql, new { idTeacher });
        }
        public async Task<(int idSchool, short idCategory)> GetSchoolIdByClassIdAsync(int idClass)
        {
            using var connection = dbConnection;

            string sql = @"SELECT IDSchool,IDClassCategory FROM Classes WHERE IDClass=@IDClass ";

            return await connection.QueryFirstOrDefaultAsync<(int, short)>(sql, new { idClass });
        }
    }
}
