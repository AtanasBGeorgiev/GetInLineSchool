using GetInLineSchool.DTOs.Request;
using GetInLineSchool.Models;
using GetInLineSchool.Repositories;
using System.Text;

namespace GetInLineSchool.Services
{
    public class StudentService
    {
        private readonly StudentRepository _repository = new StudentRepository();

        public async Task<(StatusCodes Code, string? Message)> CreateStudentAsync(CreateStudentRequest request, int idTeacher)
        {
            try
            {
                var existsPair = await _repository.CheckTeacherClass(idTeacher, request.IDClass);
                if (existsPair < 1)
                    return (StatusCodes.NotFound, "This class is not yours.");

                var studentsExist = await _repository.CheckStudentsExistAsync(request);
                if (studentsExist > 0)
                    return (StatusCodes.BadRequest, "Students are already created for this class.Now you can add only single students.");

                var classResult = await _repository.GetSchoolIdByClassIdAsync(request.IDClass);
                if (classResult == null)
                    return (StatusCodes.NotFound, "School not found.");

                SchoolRepository schoolRepository = new SchoolRepository();
                var schoolCode = await schoolRepository.GetCodeBySchoolAsync(classResult.IDSchool);
                if (schoolCode == null)
                    return (StatusCodes.NotFound, "School code not found.");

                List<Student> parameters = new List<Student>();
                int number = 1;
                for (int i = 0; i < request.Names.Count; i++)
                {
                    parameters.Add(new Student
                    {
                        Username = new StringBuilder().Append(schoolCode).Append(classResult.ClassCode)
                        .Append(request.Names[i]).Append(number).ToString(),
                        IDClass = request.IDClass
                    });

                    number++;
                }

                var result = await _repository.CreateStudentAsync(parameters);

                if (result > 0)
                    return (StatusCodes.Created, null);

                return (StatusCodes.ServerError, "Failed to create students.");
            }
            catch (Exception)
            {
                return (StatusCodes.ServerError, "SQL error occured");
            }
        }
    }
}
