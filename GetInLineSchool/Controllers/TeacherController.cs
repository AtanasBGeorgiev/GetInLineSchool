using GetInLineSchool.DTOs.Request;
using GetInLineSchool.Models;
using GetInLineSchool.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GetInLineSchool.Controllers
{
    [ApiController]
    [Route("api/teachers")]
    public class TeacherController : ControllerBase
    {
        private readonly TeacherService _service = new TeacherService();

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTeacherRequest teacher)
        {
            //Only admin can create director
            string role = User.FindFirst(ClaimTypes.Role)?.Value;

            if (teacher.Role == 2 && role != "1")
                return Unauthorized(ServiceResult<Teacher>.Failure(null, new List<Error>() { new Error { Key = "Authorization", Message = "You have not permission to create a director." } }));
           
            var result = (teacher.Role == 2 && role == "1") ?
                await _service.CreateTeacherAsync(teacher, true) :
                await _service.CreateTeacherAsync(teacher);

            if (result.Code == Services.StatusCodes.Created)
            {
                var emailResult = await HelperService.SendEmail(teacher.Email, "Потвърждение на регистрацията и верификационен код", teacher.Username);

                if (emailResult.Code == Services.StatusCodes.Success)
                    return StatusCode(201, ServiceResult<Teacher>.Success(null));

                return StatusCode(500, ServiceResult<Teacher>.Failure(null, new List<Error>() { new Error { Key = "Global", Message = "User is created successfully but " + emailResult.Message } }));     
            }

            return result.Code == Services.StatusCodes.NotFound ?
                NotFound(ServiceResult<Teacher>.Failure(null, new List<Error>() { new Error { Key = "School", Message = result.Message } }))
            : result.Code == Services.StatusCodes.Conflict ?
                Conflict(ServiceResult<Teacher>.Failure(null, new List<Error>() { new Error { Key = "Global", Message = result.Message } }))
            : StatusCode(500, ServiceResult<Teacher>.Failure(null, new List<Error>() { new Error { Key = "Global", Message = result.Message } }));
        }
    }
}
