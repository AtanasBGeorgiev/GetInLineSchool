using GetInLineSchool.DTOs.Request;
using GetInLineSchool.DTOs.Response;
using GetInLineSchool.Models;
using GetInLineSchool.Services;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Ocsp;
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

                if (teacher.Role != 2)//because director is created from an admin so token can not be accessed by a director
                {
                    LoginService _loginService = new LoginService();
                    var loginResult = await _loginService.LoginAsync(teacher.Username, teacher.Password, teacher.Username.Trim()[0]);

                    if (loginResult.Code == Services.StatusCodes.Success)
                        return StatusCode(201, ServiceResult<LoginResponse>.Success(new LoginResponse { Token = loginResult.Token }));

                    return StatusCode(201, ServiceResult<LoginResponse>.Success(new LoginResponse { Token = loginResult.Token }, new List<Error>() { new Error { Key = "Login", Message = "Created successfully but login failed." } }));
                }

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
