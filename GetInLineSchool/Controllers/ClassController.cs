using GetInLineSchool.DTOs.Request;
using GetInLineSchool.Models;
using GetInLineSchool.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GetInLineSchool.Controllers
{
    [ApiController]
    [Route("api/class")]
    [Authorize(Policy = "Director")]
    public class ClassController : BaseController
    {
        private readonly ClassService _service = new ClassService();

        [HttpPost("multi-class")]
        public async Task<IActionResult> CreateMultipleClasses([FromBody] MultiClassRequest request)
        {
            string directorId = User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;
            string role = User.FindFirst(ClaimTypes.Role)?.Value;

            var result = await _service.CreateMultipleClassesAsync(request, directorId, role);

            return await ReturnStatusCode<Classes>(result.Code, result.Message);
        }

        [HttpPost("single-class")]
        public async Task<IActionResult> CreateClass([FromBody] SingleClassRequest request)
        {
            string directorId = User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;
            string role = User.FindFirst(ClaimTypes.Role)?.Value;

            var result = await _service.CreateClassAsync(request, directorId, role);

            return await ReturnStatusCode<Classes>(result.Code, result.Message, null);
        }
    }
}
