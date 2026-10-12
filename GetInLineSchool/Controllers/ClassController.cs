using GetInLineSchool.DTOs.Request;
using GetInLineSchool.Models;
using GetInLineSchool.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GetInLineSchool.Controllers
{
    [ApiController]
    [Route("api/class")]
    [Authorize(Policy = "Director")]
    public class ClassController : ControllerBase
    {
        private readonly ClassService _service = new ClassService();

        [HttpPost("multi-class")]
        public async Task<IActionResult> CreateMultipleClasses([FromBody] MultiClassRequest request)
        {
            string directorId = User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;
            string role = User.FindFirst(ClaimTypes.Role)?.Value;

            var result = await _service.CreateMultipleClassesAsync(request, directorId, role);

            return result.Code == Services.StatusCodes.Success ?
                StatusCode(201, ServiceResult<Classes>.Success(null))
                : result.Code == Services.StatusCodes.NotFound ?
                NotFound(ServiceResult<Classes>.Failure(null, new List<Error>() { new Error { Key = "Not found", Message = result.Message } }))
                : result.Code == Services.StatusCodes.Conflict ?
                Conflict(ServiceResult<Classes>.Failure(null, new List<Error>() { new Error { Key = "Conflict", Message = result.Message } }))
                : StatusCode(500, ServiceResult<Classes>.Failure(null, new List<Error>() { new Error { Key = "Not found", Message = result.Message } }));
        }
    }
}
