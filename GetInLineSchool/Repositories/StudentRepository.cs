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
        public async Task<Student?> GetStudentClassByUsernameAsync(string username)
        {
            using var connection = dbConnection;

            string sql = @"SELECT IDStudent,IDClass FROM Students WHERE Username = @Username AND Password  IS NULL AND IsActive = 0";

            return await connection.QueryFirstOrDefaultAsync<Student>(sql, new { Username = username });
        }
        public async Task<int>CheckSignUpCode(string code,int idClass)
        {
            using var connection = dbConnection;

            string sql = @"SELECT COUNT(*) FROM SignUpCodes WHERE Code=@Code AND CodeType = 2 AND IDClass = @IDClass AND ExpirationDate > DATEDIFF_BIG(
                            SECOND,
                            '2025-01-01',
                            CONVERT(
                                DATETIME2,
                                SYSUTCDATETIME()
                                AT TIME ZONE 'UTC'
                                AT TIME ZONE 'E. Europe Standard Time'
                            ))";

            return await connection.QueryFirstOrDefaultAsync<int>(sql, new { Code = code, IDClass = idClass });
        }
        public async Task<int> SetPassEmailStudentAsync(string password, string email, int idStudent)
        {
            using var connection = dbConnection;

            string sql = @"UPDATE Students SET Password=@Password,Email=@Email WHERE IDStudent=@IDStudent";

            return await connection.ExecuteAsync(sql, new { Password = password, Email = email, IDStudent = idStudent });
        }
    }
}
