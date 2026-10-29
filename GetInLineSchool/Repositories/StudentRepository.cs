using Dapper;
using GetInLineSchool.DTOs.Request;
using GetInLineSchool.Models;
using Org.BouncyCastle.Crypto.Digests;

namespace GetInLineSchool.Repositories
{
    public class StudentRepository : BaseRepository
    {
        public async Task<int>CreateStudentAsync(List<Student> parameters)
        {
            using var connection = dbConnection;

            string sql = @"INSERT INTO Students (Username, IDClass)"+
                "VALUES" +
                "(@Username, @IDClass)";

            return await connection.ExecuteAsync(sql, parameters);
        }

        public async Task<int> CheckTeacherClass(int idTeacher, int idClass)
        {
            using var connection = dbConnection;

            string sql = @"SELECT COUNT(*) FROM TeacherClasses WHERE IDTeacher = @IDTeacher AND IDClass = @IDClass AND IsActive=1";

            return await connection.ExecuteScalarAsync<int>(sql, new { IDTeacher = idTeacher, IDClass = idClass });
        }

        public async Task<Classes?> GetSchoolIdByClassIdAsync(int id)
        {
            using var connection = dbConnection;

            return await connection.QueryFirstOrDefaultAsync<Classes>("SELECT IDSchool,ClassCode FROM Classes WHERE IDClass=@IDClass", new { IDClass = id });
        }

        public async Task<int> CheckStudentsExistAsync(CreateStudentRequest request)
        {
            using var connection = dbConnection;

            return await connection.QueryFirstOrDefaultAsync<int>
                ("SELECT COUNT(*) FROM Students WHERE IDClass=@IDClass", request);
        }

        public async Task<int> CheckUniqueFieldsAsync(CreateStudentRequest request)
        {
            using var connection = dbConnection;

            return await connection.QueryFirstOrDefaultAsync<int>
                ("SELECT COUNT(*) FROM Students WHERE Username=@Username OR Email=@Email", request);
        }
        public async Task<int> GetCountStudentsInClass(int idClass)
        {
            using var connection = dbConnection;

            return await connection.QueryFirstOrDefaultAsync<int>
                ("SELECT COUNT(*) FROM Students WHERE IDClass=@IDClass", new { IDClass = idClass });
        }
    }
}
