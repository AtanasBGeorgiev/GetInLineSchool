using Dapper;
using GetInLineSchool.Models;

namespace GetInLineSchool.Repositories
{
    public class QueueRepository : BaseRepository
    {
        public async Task<int> CreateQueueAsync(Queue queue)
        {
            using var connection = dbConnection;

            string sql = @"INSERT INTO Queues(IDSchool,IDTeacher,IDStudent,IDClass,Subject,Duration,Code)"
                        + "VALUES(@IDSchool,@IDTeacher,@IDStudent,@IDClass,@Subject,@Duration,@Code)";

            return await connection.ExecuteAsync(sql, queue);
        }
        public async Task<int> GetStudentPermissionClass(int idStudent)
        {
            using var connection = dbConnection;

            string sql = @"SELECT IDClass FROM Students WHERE IDStudent = @IDStudent";

            return await connection.QueryFirstOrDefaultAsync<int>(sql, new { IDStudent = idStudent });
        }

        public async Task<(int IDClass, short IDSchool, bool Permission)> CheckStudentClassAndPermissionAsync(int idStudent)
        {
            using var connection = dbConnection;

            string sql = @"SELECT C.IDClass,C.IDSchool,S.EnableStudents
                            FROM Students ST
                            INNER JOIN Classes C
	                            ON ST.IDClass=C.IDClass
                            INNER JOIN Schools S
	                            ON C.IDSchool = S.IDSchool
                            WHERE ST.IDStudent = @IDStudent";

            return await connection.QueryFirstOrDefaultAsync<(int IDClass, short IDSchool, bool Permission)>(sql, new { IDStudent = idStudent });
        }
        public async Task<(short TeacherSchool, short ClassSchool)> CheckTeacherSchoolAsync(int idTeacher, int? idClass)
        {
            using var connection = dbConnection;

            string sql = @"SELECT T.IDSchool AS TeacherSchoolID,C.IDSchool AS ClassSchoolID
                            FROM Teachers T
                            INNER JOIN Classes C
                                ON C.IDClass = @IDClass
                            WHERE T.IDTeacher = @IDTeacher";

            return await connection.QueryFirstOrDefaultAsync<(short TeacherSchool, short ClassSchool)>(sql, new { IDTeacher = idTeacher, IDClass = idClass });
        }
        public async Task<short> GetTeacherSchoolAsync(int idTeacher)
        {
            using var connection = dbConnection;

            string sql = @"SELECT IDSchool FROM Teachers WHERE IDTeacher = @IDTeacher";

            return await connection.QueryFirstOrDefaultAsync<short>(sql, new { IDTeacher = idTeacher});
        }
        public async Task<bool> IsActiveCodeAsync(string code)
        {
            using var connection = dbConnection;

            string sql = @"SELECT CASE
                                WHEN EXISTS
                                (
                                    SELECT 1
                                    FROM Queues
                                    WHERE Code = @Code
                                      AND DATEADD(SECOND, Duration * 60,
                                          DATEADD(SECOND, Date, CAST('2025-01-01' AS DATETIME2))) > GETDATE()
                                )
                                THEN 1
                                ELSE 0
                            END";

            return await connection.QuerySingleAsync<bool>(sql, new { Code = code });
        }
    }
}
