using GetInLineSchool.DTOs.Request;
using GetInLineSchool.Models;
using GetInLineSchool.Repositories;
using Microsoft.Data.SqlClient;

namespace GetInLineSchool.Services
{
    public class ClassService
    {
        private readonly ClassRepository _repository = new ClassRepository();
        private readonly CodeRepository _codeRepository = new CodeRepository();

        public async Task<(StatusCodes Code, string? Message)> CreateMultipleClassesAsync(MultiClassRequest request, string idDirector, string role)
        {
            try
            {
                var isActiveResult = await _codeRepository.CheckIsUserActiveAsync(idDirector, role);
                if (!isActiveResult.IsActive)
                    return (StatusCodes.BadRequest, "Director is not verified.");

                var duration = await _repository.GetCategoryDurationAsync(request.IDClassCategory);
                if (request.CountYears > duration)
                    return (StatusCodes.BadRequest, "Count of years can not exceed category's duration.");

                short idSchool = await _codeRepository.GetSchoolIdAsync(idDirector);

                if (idSchool == null)
                    return (StatusCodes.NotFound, "School not found.");

                List<Classes> parameters = new List<Classes>();

                int currentYear = DateTime.Now.Year;

                for (int i = 0; i < request.CountYears; i++)
                {
                    char currentClass = '\u0410';//Bulgarian A

                    for (int j = 0; j < request.CountClasses; j++)
                    {
                        parameters.Add(
                            new Classes
                            {
                                IDSchool = idSchool,
                                Year = currentYear,
                                Class = currentClass,
                                IDClassCategory = request.IDClassCategory
                            });
                        currentClass++;
                    }
                    currentYear = currentYear - 1;
                }

                var result = await _repository.CreateMultipleSchoolsAsync(parameters);

                if (result > 0)
                    return (StatusCodes.Success, null);

                return (StatusCodes.ServerError, "Failed to add classes.");
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
