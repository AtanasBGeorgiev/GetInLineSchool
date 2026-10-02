using Microsoft.AspNetCore.Mvc;
using GetInLineSchool.DTOs.Request;
using GetInLineSchool.Services;
using GetInLineSchool.Models;

namespace GetInLineSchool.Controllers
{
    [ApiController]
    [Route("api/login")]
    public class LoginController : ControllerBase
    {
        private readonly LoginService _service = new LoginService();

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            request.Username = request.Username.Trim();
            request.Password = request.Password.Trim();

            char firstChar = request.Username[0];

            var result = await _service.LoginAsync(request.Username, request.Password, firstChar);

            return result.Code == Services.StatusCodes.Success ?
                Ok(ServiceResult<Teacher>.Success(null))
            : Unauthorized(ServiceResult<Teacher>.Failure(null, new List<Error>() { new Error { Key = "Authorization", Message = result.Message } }));
        }
    }
}
