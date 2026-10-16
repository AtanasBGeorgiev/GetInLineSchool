using GetInLineSchool.DTOs.Request;
using GetInLineSchool.Repositories;
using Microsoft.Data.SqlClient;

namespace GetInLineSchool.Services
{
    public class TeacherClassService
    {
        private readonly TeacherClassRepository _service = new TeacherClassRepository();

        public async Task<(StatusCodes Code, string? Message)> CreateTeacherClassesAsync(TeacherClassesRequest request, int idTeacher)
        {
            try
            {
                List<short> qualifications = await _service.GetTeacherQualifications(idTeacher);
                if (qualifications.Count < 1)
                    return (StatusCodes.BadRequest, "First you have to set your qualifications.");

                var idSchool = await _service.GetSchoolIdByTeacherIdAsync(idTeacher);
                if (idTeacher < 1)
                    return (StatusCodes.BadRequest, "School not found.");

                List<Models.TeacherClass> parameters = new List<Models.TeacherClass>();

                for (int i = 0; i < request.TeacherClasses.Count; i++)
                {
                    var check = await _service.GetSchoolIdByClassIdAsync(request.TeacherClasses[i].IDClass);
                    if (idSchool != check.idSchool)
                        return (StatusCodes.BadRequest, "You can choose classes only from your school.");

                    if (!qualifications.Contains(check.idCategory))
                        return (StatusCodes.BadRequest, "You can choose classes only from your qualification category.");

                    parameters.Add(
                        new Models.TeacherClass
                        {
                            IDTeacher = idTeacher,
                            IDClass = request.TeacherClasses[i].IDClass,
                            Subject = request.TeacherClasses[i].Subject
                        });
                }

                var result = await _service.CreateTeacherClassesAsync(parameters);

                if (result > 0)
                    return (StatusCodes.Created, null);

                return (StatusCodes.ServerError, "Failed to create teacher classes.");
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
