using GetInLineSchool.DTOs.Request;
using GetInLineSchool.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GetInLineSchool.Controllers
{
    [ApiController]
    [Route("api/teacher-class")]
    public class TeacherClassController : BaseController
    {
        private readonly TeacherClassService _service = new TeacherClassService();
        [HttpPost]
        [Authorize(Policy = "TeacherOrDirector")]
        public async Task<IActionResult> CreateTeacherClasses([FromBody] TeacherClassesRequest request)
        {
            int idTeacher = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "id")?.Value);

            var result = await _service.CreateTeacherClassesAsync(request, idTeacher);

            return await ReturnStatusCode<TeacherClass>(result.Code, result.Message, null);
        }
    }
}
