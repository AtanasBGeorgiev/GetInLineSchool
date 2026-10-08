using GetInLineSchool.DTOs.Request;
using GetInLineSchool.Models;
using GetInLineSchool.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GetInLineSchool.Controllers
{
    [ApiController]
    [Route("api/code")]
    [Authorize]
    public class CodeController : ControllerBase
    {
        private readonly CodeService _service = new CodeService();

        [HttpPost("verify-email")]
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

        [HttpPost("create-ver-code")]
        public async Task<IActionResult> CreateVerificationCode()
        {
            string userId = User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;
            string role = User.FindFirst(ClaimTypes.Role)?.Value;

            var result = await _service.CreateVerificationCode(userId, role);

            return result.Code == Services.StatusCodes.Success ?
                StatusCode(201, ServiceResult<VerificationCode>.Success(null))
                : StatusCode(500, ServiceResult<VerificationCode>.Failure(null, new List<Error>() { new Error { Key = "Ver code", Message = result.Message } }));
        }

        //[HttpPost("sign-up-code")]
        //[Authorize(Policy = "TeacherOrDirector")]
        //public async Task<IActionResult> CreateSignUpCode([FromBody] SignUpCodeRequest request)
        //{
        //    string userId = User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;
        //    string role = User.FindFirst(ClaimTypes.Role)?.Value;


        //}
    }
}
