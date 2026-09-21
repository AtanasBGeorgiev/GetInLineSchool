using GetInLineSchool.Repositories;
using GetInLineSchool.Models;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;

namespace GetInLineSchool.Services
{
    public class TeacherService
    {
        private readonly TeacherRepository _repository = new TeacherRepository();

        public async Task<int> CreateTeacherAsync(Teacher teacher)
        {
            //if role=director check token role for admin

            SchoolRepository schoolRepository = new SchoolRepository();
            string schoolCode = await schoolRepository.GetCodeBySchoolAsync(teacher.IDSchool);

            if (schoolCode == null)
                return 0;

            string username = schoolCode + teacher.Username;
            teacher.Username = username;

            var result = await _repository.CheckUniqueFieldsAsync(teacher);

            if (result > 0)
                return -1;

            string hashedPassword = HelperService.HashPassword(teacher.Password);
            teacher.Password = hashedPassword;

            return await _repository.CreateTeacherAsync(teacher);
        }
    }
}
