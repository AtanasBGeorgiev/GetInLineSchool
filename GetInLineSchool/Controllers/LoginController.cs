using Microsoft.AspNetCore.Mvc;
using GetInLineSchool.DTOs.Request;
using GetInLineSchool.Services;
using GetInLineSchool.Models;
using GetInLineSchool.DTOs.Response;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

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
                Ok(ServiceResult<LoginResponse>.Success(new LoginResponse { Token = result.Token }))
            : Unauthorized(ServiceResult<LoginResponse>.Failure(null, new List<Error>() { new Error { Key = "Authorization", Message = result.Message } }));
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> VerifyEmail([FromBody] VerifyEmailRequest request)
        {
            string userId = User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;
            string role = User.FindFirst(ClaimTypes.Role)?.Value;

            var result = await _service.VerifyEmailAsync(request, userId, role);

            return result.Code == Services.StatusCodes.Success ?
                 Ok(ServiceResult<VerificationCode>.Success(null))
                : result.Code == Services.StatusCodes.NotFound ?
                NotFound(ServiceResult<VerificationCode>.Failure(null, new List<Error>() { new Error { Key = "Verify", Message = result.Message } }))
                : result.Code == Services.StatusCodes.BadRequest ?
                BadRequest(ServiceResult<VerificationCode>.Failure(null, new List<Error>() { new Error { Key = "Verify", Message = result.Message } }))
                : StatusCode(500, ServiceResult<VerificationCode>.Failure(null, new List<Error>() { new Error { Key = "Verify", Message = result.Message } }));     
        }
    }
}
