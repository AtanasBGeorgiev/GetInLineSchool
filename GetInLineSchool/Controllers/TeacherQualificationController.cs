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
    [Route("api/qualification")]
    public class TeacherQualificationController : BaseController
    {
        QualificationService _service = new QualificationService();

        [HttpPost]
        [Authorize(Policy = "TeacherOrDirector")]
        public async Task<IActionResult> CreateQualifications([FromBody] TeacherQualificationsRequest request)
        {
            int idTeacher = int.Parse(User?.Claims.FirstOrDefault(c => c.Type == "id")?.Value);
            string role = User.FindFirst(ClaimTypes.Role)?.Value;

            var result = await _service.CreateQualifications(request, idTeacher, role);

            return await ReturnStatusCode<TeacherQualification>(result.Code, result.Message, null);
        }
    }
}
