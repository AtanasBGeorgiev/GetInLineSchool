using Dapper;
using GetInLineSchool.Models;
using GetInLineSchool.Services;

namespace GetInLineSchool.Repositories
{
    public class SchoolRepository : BaseRepository
    {
        public async Task<int> CreateSchoolAsync(School school)
        {
            using var connection = dbConnection;//automatically disposes the connection after the block is exited

            var sql = @"
                INSERT INTO Schools (Name, Address, City, Email, Phone, UnifiedIdentificationCode, MateriallyResponsiblePerson, SubscriptionPlan, IsPaid, EnableStudents, SchoolCode) 
                VALUES (@Name, @Address, @City, @Email, @Phone, @UnifiedIdentificationCode, @MateriallyResponsiblePerson, @SubscriptionPlan, @IsPaid, @EnableStudents, @SchoolCode);";

            return await connection.ExecuteAsync(sql, school);
        }

        public async Task<List<int>> GetSchoolCodes()
        {
            using var connection = dbConnection;

            var result = await connection.QueryAsync<int>("SELECT SchoolCode FROM Schools");

            return result.ToList();
        }

        public async Task<int> CheckUniqueFields(School school)
        {
            using var connection = dbConnection;

            return await connection.QueryFirstOrDefaultAsync<int>
                ("SELECT COUNT(*) FROM SCHOOLS WHERE Name=@Name OR Email=@Email OR Phone=@Phone OR " +
                "UnifiedIdentificationCode=@UnifiedIdentificationCode", school);
        }
        
    }
}
