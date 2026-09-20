using GetInLineSchool.Repositories;
using GetInLineSchool.Models;

namespace GetInLineSchool.Services
{
    public class SchoolService
    {
        private readonly SchoolRepository _repository = new SchoolRepository();

        public async Task<int> CreateSchoolAsync(School school)
        {
            if (await _repository.CheckUniqueFields(school) > 0)
            {
                return -1;
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

            return await _repository.CreateSchoolAsync(school);
        }
    }
}
