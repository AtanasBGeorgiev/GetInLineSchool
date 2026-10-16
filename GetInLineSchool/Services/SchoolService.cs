using GetInLineSchool.Repositories;
using GetInLineSchool.DTOs.Request;

namespace GetInLineSchool.Services
{
    public class SchoolService
    {
        private readonly SchoolRepository _repository = new SchoolRepository();

        public async Task<(StatusCodes Code, string? Message, short? IdSchool)> CreateSchoolAsync(CreateSchoolRequest school)
        {
            if (await _repository.CheckUniqueFields(school) > 0)
            {
                return (StatusCodes.Conflict,"Unique fields violation.", null);
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
                return id > 0 ? (StatusCodes.Created, null, id) : (StatusCodes.ServerError, "Failed to create school.", null);
            }
            catch(Exception ex)
            {
                return (StatusCodes.ServerError, "SQL error occured.", null);
            }
            
        }
    }
}
