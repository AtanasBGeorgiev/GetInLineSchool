using Dapper;
using GetInLineSchool.Models;

namespace GetInLineSchool.Repositories
{
    public class QualificationRepository : BaseRepository
    {
        public async Task<int> CreateQualificationsAsync(List<TeacherQualification> parameters)
        {
            using var connection = dbConnection;

            string sql = @"INSERT INTO TeacherQualifications (IDTeacher, IDClassCategory)" +
                "VALUES" +
                "(@IDTeacher,@IDClassCategory)";

            return await connection.ExecuteAsync(sql, parameters);
        }
    }
}
