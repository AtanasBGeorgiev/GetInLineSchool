using GetInLineSchool.Repositories;
using GetInLineSchool.Models;
using GetInLineSchool.DTOs.Request;

namespace GetInLineSchool.Services
{
    public class SchoolService
    {
        private readonly SchoolRepository _repository = new SchoolRepository();

        public async Task<(StatusCodes Code,short? IdSchool)> CreateSchoolAsync(CreateSchoolRequest school)
        {
            if (await _repository.CheckUniqueFields(school) > 0)
            {
                return (StatusCodes.Conflict, null);
            }

            List<int> codes = await _repository.GetSchoolCodes();

            int code = 0;

            while (true)
            {
                code = HelperService.CreateSchoolCode();

                if (!codes.Contains(code))
                    break;
            }

            school.SchoolCode = code;

            try
            {
                var id = await _repository.CreateSchoolAsync(school);
                return id > 0 ? (StatusCodes.Created, id) : (StatusCodes.ServerError, null);
            }
            catch(Exception ex)
            {
                return (StatusCodes.ServerError, null);
            }
            
        }
    }
}
