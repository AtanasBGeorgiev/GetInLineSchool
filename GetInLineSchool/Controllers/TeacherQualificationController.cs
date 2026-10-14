using GetInLineSchool.DTOs.Request;
using GetInLineSchool.Models;
using GetInLineSchool.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GetInLineSchool.Controllers
{
    [ApiController]
    [Route("api/qualification")]
    public class TeacherQualificationController : ControllerBase
    {
        QualificationService _service = new QualificationService();

        [HttpPost]
        [Authorize(Policy = "TeacherOrDirector")]
        public async Task<IActionResult> CreateQualifications([FromBody] TeacherQualificationsRequest request)
        {
            int idTeacher = int.Parse(User?.Claims.FirstOrDefault(c => c.Type == "id")?.Value);
            string role = User.FindFirst(ClaimTypes.Role)?.Value;

            var result = await _service.CreateQualifications(request, idTeacher, role);

            return result.Code == Services.StatusCodes.Success ?
                StatusCode(201, ServiceResult<TeacherQualification>.Success(null))
                : result.Code == Services.StatusCodes.Conflict ?
                Conflict(ServiceResult<Classes>.Failure(null, new List<Error>() { new Error { Key = "Conflict", Message = result.Message } }))
                : result.Code == Services.StatusCodes.BadRequest ?
                BadRequest(ServiceResult<Classes>.Failure(null, new List<Error>() { new Error { Key = "Bad request", Message = result.Message } }))
                : StatusCode(500, ServiceResult<Classes>.Failure(null, new List<Error>() { new Error { Key = "Not found", Message = result.Message } }));
        }
    }
}
