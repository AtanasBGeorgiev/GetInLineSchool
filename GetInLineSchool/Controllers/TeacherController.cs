using GetInLineSchool.Services;
using Microsoft.AspNetCore.Mvc;
using GetInLineSchool.Models;
using System.Linq.Expressions;
using Microsoft.Data.SqlClient;

namespace GetInLineSchool.Controllers
{
    [ApiController]
    [Route("api/teachers")]
    public class TeacherController : ControllerBase
    {
        private readonly TeacherService _service = new TeacherService();

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Teacher teacher)
        {
            try
            {
                var result = await _service.CreateTeacherAsync(teacher);

                if (result == 1)
                    return Ok(ServiceResult<Teacher>.Success(null));
                if (result == 0) 
                    return BadRequest(ServiceResult<Teacher>.Failure(null, new List<Error>() { new Error { Key = "School", Message = "School not found." } }));
                if (result == -1)
                    return Conflict(ServiceResult<Teacher>.Failure(null, new List<Error>() { new Error { Key = "Global", Message = "Unique fields violation." } }));

                return StatusCode(500, ServiceResult<Teacher>.Failure(null, new List<Error>() { new Error { Key = "Global", Message = "Failed to create teacher." } }));
            }
            catch (SqlException ex)
            {
                return Conflict(ServiceResult<Teacher>.Failure(null, new List<Error>() { new Error { Key = "SQL", Message = "SQL add teacher exception" } }));
            }
            
        }
    }
}
