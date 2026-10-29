using GetInLineSchool.DTOs.Request;
using GetInLineSchool.Models;
using GetInLineSchool.Repositories;
using Microsoft.Data.SqlClient;
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

                if (request.Names.Count > 1)//first are added multiple students but after that can be added one by one
                {
                    var studentsExist = await _repository.CheckStudentsExistAsync(request);
                    if (studentsExist > 0)
                        return (StatusCodes.BadRequest, "Students are already created for this class.Now you can add only single students.");
                }

                var classResult = await _repository.GetSchoolIdByClassIdAsync(request.IDClass);
                if (classResult == null)
                    return (StatusCodes.NotFound, "School not found.");

                SchoolRepository schoolRepository = new SchoolRepository();
                var schoolCode = await schoolRepository.GetCodeBySchoolAsync(classResult.IDSchool);
                if (schoolCode == null)
                    return (StatusCodes.NotFound, "School code not found.");

                var countStudents = await _repository.GetCountStudentsInClass(request.IDClass);
                List<Student> parameters = new List<Student>();
                int number = countStudents + 1;

                for (int i = 0; i < request.Names.Count; i++)
                {
                    parameters.Add(new Student
                    {
                        Username = new StringBuilder().Append('s').Append(schoolCode).Append(classResult.ClassCode)
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

        public async Task<(StatusCodes Code, string? Message)> SetStudentDataAsync(AtivateStudentRequest request)
        {
            try
            {
                var student = await _repository.GetStudentClassByUsernameAsync(request.Username);
                if (student == null || student.IDClass == 0)
                    return (StatusCodes.NotFound, "Student not found.");

                int isValid = await _repository.CheckSignUpCode(request.SignUpCode, student.IDClass);
                if (isValid == 0)
                    return (StatusCodes.BadRequest, "Wrong signUpCode or it does not exist for your class.");

                string hashedPassword = HashingService.Hash(request.Password);

                var result = await _repository.SetPassEmailStudentAsync(hashedPassword, request.Email, student.IDStudent);

                if (result < 1)
                    return (StatusCodes.ServerError, "Failed to update student data.");

                var sendEmailResult = await HelperService.SendEmail(request.Email, "Verification code", null);

                return (StatusCodes.Success, sendEmailResult.Message);
            }
            catch (SqlException ex) when (ex.Number == 2627 || ex.Number == 2601)
            {
                return (StatusCodes.Conflict, "Email already in use.");
            }
            catch (Exception ex)
            {
                return (StatusCodes.ServerError, "SQL error occured.");
            }

        }
    }
}
