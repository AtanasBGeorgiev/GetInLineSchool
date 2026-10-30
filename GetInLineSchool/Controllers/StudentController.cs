using GetInLineSchool.DTOs.Request;
using GetInLineSchool.DTOs.Response;
using GetInLineSchool.Models;
using GetInLineSchool.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GetInLineSchool.Controllers
{
    [ApiController]
    [Route("api/student")]
    public class StudentController : BaseController
    {
        private readonly StudentService _service = new StudentService();
        private readonly LoginService _loginService = new LoginService();

        [HttpPost]
        [Authorize(Policy = "TeacherOrDirector")]
        public async Task<IActionResult> CreateStudentsAsync([FromBody] CreateStudentRequest request)
        {
            int idTeacher = Convert.ToInt32(User.Claims.FirstOrDefault(c => c.Type == "id")?.Value);
            var result = await _service.CreateStudentAsync(request, idTeacher);

            return await ReturnStatusCode<CreateStudentResponse>(result.Code, result.Message, null);
        }

        //Student created password and email
        [HttpPut]
        [Authorize]
        public async Task<IActionResult> SetStudentDataAsync([FromBody] AtivateStudentRequest request)
        {
            var result = await _service.SetStudentDataAsync(request);

            if (result.Code == Services.StatusCodes.Success)//data is updated
            {
                var loginResult = await _loginService.LoginAsync(request.Username, request.Password, request.Username.Trim()[0]);

                if (loginResult.Code == Services.StatusCodes.Success)
                    return Ok(ServiceResult<LoginResponse>.Success(new LoginResponse { Token = loginResult.Token },
                        new List<Error>() {result.Message != null ? new Error { Key = "Email", Message = result.Message } : null }));

                return StatusCode(500, ServiceResult<LoginResponse>.Failure(null, new List<Error>() { new Error { Key = "Login", Message = "Updated successfully but login failed." } })); 
            }

            return await ReturnStatusCode<CreateStudentResponse>(result.Code, result.Message, null);
        }
    }
}
