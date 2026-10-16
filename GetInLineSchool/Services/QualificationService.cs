using GetInLineSchool.DTOs.Request;
using GetInLineSchool.Models;
using GetInLineSchool.Repositories;
using Microsoft.Data.SqlClient;

namespace GetInLineSchool.Services
{
    public class QualificationService
    {
        private readonly QualificationRepository _repository = new QualificationRepository();
        private readonly CodeRepository _codeRepository = new CodeRepository();

        public async Task<(StatusCodes Code, string? Message)> CreateQualifications(TeacherQualificationsRequest request, int idTeacher, string role)
        {
            try
            {
                var isActiveResult = await _codeRepository.CheckIsUserActiveAsync(idTeacher.ToString(), role);
                if (!isActiveResult.IsActive)
                    return (StatusCodes.BadRequest, "Teacher is not verified.");

                List<TeacherQualification> parameters = new List<TeacherQualification>();

                for (int i = 0; i < request.IDClassCategories.Count; i++)
                {
                    parameters.Add(
                        new TeacherQualification
                        {
                            IDTeacher = idTeacher,
                            IDClassCategory = request.IDClassCategories[i]
                        });
                }
                var result = await _repository.CreateQualificationsAsync(parameters);

                if (result > 0)
                    return (StatusCodes.Created, null);

                return (StatusCodes.ServerError, "Failed to add qualifications.");
            }
            catch (SqlException ex) when (ex.Number == 2627 || ex.Number == 2601)
            {
                return (StatusCodes.Conflict, "A record with this unique value already exists.");
            }
            catch (Exception ex)
            {
                return (StatusCodes.ServerError, "SQL error occured.");
            }
        }
    }
}
