using GetInLineSchool.DTOs.Request;
using GetInLineSchool.DTOs.Response;
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

        [Authorize(Policy = "TeacherOrDirector")]
        public async Task<IActionResult> CreateStudentAsync([FromBody] CreateStudentRequest request)
        {
            int idTeacher = Convert.ToInt32(User.Claims.FirstOrDefault(c => c.Type == "id")?.Value);
            var result = await _service.CreateStudentAsync(request, idTeacher);

            return await ReturnStatusCode<CreateStudentResponse>(result.Code, result.Message, null);
        }
    }
}
