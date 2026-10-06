using Dapper;
using GetInLineSchool.DTOs.Request;
using GetInLineSchool.Models;
using GetInLineSchool.Services;

namespace GetInLineSchool.Repositories
{
    public class SchoolRepository : BaseRepository
    {
        public async Task<short> CreateSchoolAsync(CreateSchoolRequest school)
        {
            using var connection = dbConnection;//automatically disposes the connection after the block is exited

            var sql = @"
                INSERT INTO Schools (Name, Address, City, Email, Phone, UnifiedIdentificationCode, MateriallyResponsiblePerson, SubscriptionPlan, IsPaid, EnableStudents, SchoolCode) 
                OUTPUT INSERTED.IDSchool          
                VALUES (@Name, @Address, @City, @Email, @Phone, @UnifiedIdentificationCode, @MateriallyResponsiblePerson, @SubscriptionPlan, @IsPaid, @EnableStudents, @SchoolCode);";

            return await connection.QuerySingleAsync<short>(sql, school);
        }

        public async Task<List<int>> GetSchoolCodes()
        {
            using var connection = dbConnection;            

            var result = await connection.QueryAsync<int>("SELECT SchoolCode FROM Schools");

            return result.ToList();
        }

        public async Task<int> CheckUniqueFields(CreateSchoolRequest school)
        {
            using var connection = dbConnection;

            return await connection.QueryFirstOrDefaultAsync<int>
                ("SELECT COUNT(*) FROM SCHOOLS WHERE Email=@Email OR Phone=@Phone OR " +
                "UnifiedIdentificationCode=@UnifiedIdentificationCode", school);
        }

        public async Task<string>GetCodeBySchoolAsync(int id)
        {
            using var connection = dbConnection;

            return await connection.QueryFirstOrDefaultAsync<string>("SELECT SchoolCode FROM Schools WHERE IDSchool=@IDSchool", new { IDSchool = id });
        }

    }
}
