using GetInLineSchool.DTOs.Request;
using GetInLineSchool.DTOs.Response;
using GetInLineSchool.Models;
using GetInLineSchool.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GetInLineSchool.Controllers
{
    [ApiController]
    [Route("api/code")]
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
        [Authorize]
        public async Task<IActionResult> CreateVerificationCode()
        {
            string userId = User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;
            string role = User.FindFirst(ClaimTypes.Role)?.Value;

            var result = await _service.CreateVerificationCode(userId, role);

            return result.Code == Services.StatusCodes.Success ?
                StatusCode(201, ServiceResult<VerificationCode>.Success(null))
                : StatusCode(500, ServiceResult<VerificationCode>.Failure(null, new List<Error>() { new Error { Key = "Ver code", Message = result.Message } }));
        }

        [HttpPost("sign-up-code")]
        [Authorize(Policy = "TeacherOrDirector")]
        public async Task<IActionResult> CreateSignUpCode([FromBody] SignUpCodeRequest request)
        {
            string userId = User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;
            string role = User.FindFirst(ClaimTypes.Role)?.Value;

            var result = await _service.CreateSignUpCode(userId, role, request.CodeType);

            return result.Code == Services.StatusCodes.Success ?
                StatusCode(201, ServiceResult<CreateSignUpCodeResponse>.Success(new CreateSignUpCodeResponse { Code = result.signUpCode }))
                : result.Code == Services.StatusCodes.NotFound ?
                NotFound(ServiceResult<SignUpCode>.Failure(null, new List<Error>() { new Error { Key = "Sign up code", Message = result.Message } }))
                : result.Code == Services.StatusCodes.Forbidden ?
                StatusCode(403,ServiceResult<SignUpCode>.Failure(null, new List<Error>() { new Error { Key = "Sign up code", Message = result.Message } }))
                 : result.Code == Services.StatusCodes.BadRequest ?
                BadRequest(ServiceResult<SignUpCode>.Failure(null, new List<Error>() { new Error { Key = "Verify", Message = result.Message } }))
                : result.Code == Services.StatusCodes.Conflict ?
                Conflict(ServiceResult<SignUpCode>.Failure(null, new List<Error>() { new Error { Key = "Conflict", Message = result.Message } }))
                : StatusCode(500, ServiceResult<SignUpCode>.Failure(null, new List<Error>() { new Error { Key = "Verify", Message = result.Message } }));
        }
    }
}
