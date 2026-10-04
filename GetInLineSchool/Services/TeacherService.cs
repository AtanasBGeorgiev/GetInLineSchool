using GetInLineSchool.DTOs.Request;
using GetInLineSchool.Models;
using GetInLineSchool.Repositories;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace GetInLineSchool.Services
{
    public class TeacherService
    {
        private readonly TeacherRepository _repository = new TeacherRepository();

        public async Task<(StatusCodes Code, string? Message)> CreateTeacherAsync(CreateTeacherRequest teacher, bool directorCheck = false)
        {
            try
            {
                if (directorCheck)
                {
                    var directorExists = await _repository.CheckDirectorExistsAsync(teacher);
                    if (directorExists > 0)
                        return (StatusCodes.Conflict, "Director already exists for this school.");
                }

                SchoolRepository schoolRepository = new SchoolRepository();
                string schoolCode = await schoolRepository.GetCodeBySchoolAsync(teacher.IDSchool);

                if (schoolCode == null)
                    return (StatusCodes.NotFound, "School not found.");

                string username = schoolCode + teacher.Username;
                teacher.Username = username;

                var result = await _repository.CheckUniqueFieldsAsync(teacher);

                if (result > 0)
                    return (StatusCodes.Conflict, "Unique fields violation.");

                string hashedPassword = HashingService.Hash(teacher.Password);
                teacher.Password = hashedPassword;

                var rows = await _repository.CreateTeacherAsync(teacher);

                return rows > 0 ? (StatusCodes.Created,null) : (StatusCodes.ServerError, "Failed to create teacher.");
            }
            catch(Exception ex)
            {
                return (StatusCodes.ServerError, "SQL error occurred.");
            }

        }
    }
}
