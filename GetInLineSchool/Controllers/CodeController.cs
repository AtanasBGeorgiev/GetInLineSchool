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
    public class CodeController : BaseController
    {
        private readonly CodeService _service = new CodeService();

        [HttpPost("verify-email")]
        public async Task<IActionResult> VerifyEmail([FromBody] VerifyEmailRequest request)
        {
            string userId = User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;
            string role = User.FindFirst(ClaimTypes.Role)?.Value;

            var result = await _service.VerifyEmailAsync(request, userId, role);

            return await ReturnStatusCode<VerificationCode>(result.Code, result.Message, null);
        }

        [HttpPost("create-ver-code")]
        [Authorize]
        public async Task<IActionResult> CreateVerificationCode()
        {
            string userId = User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;
            string role = User.FindFirst(ClaimTypes.Role)?.Value;

            var result = await _service.CreateVerificationCode(userId, role);

            return await ReturnStatusCode<VerificationCode>(result.Code, result.Message, null);
        }

        [HttpPost("sign-up-code")]
        [Authorize(Policy = "TeacherOrDirector")]
        public async Task<IActionResult> CreateSignUpCode([FromBody] SignUpCodeRequest request)
        {
            string userId = User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;
            string role = User.FindFirst(ClaimTypes.Role)?.Value;

            var result = await _service.CreateSignUpCode(userId, role, request.CodeType);

            return await ReturnStatusCode<CreateSignUpCodeResponse>(result.Code, result.Message, new CreateSignUpCodeResponse { Code = result.signUpCode });
        }
    }
}
