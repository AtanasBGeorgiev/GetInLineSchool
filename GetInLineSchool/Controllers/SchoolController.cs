using GetInLineSchool.Models;
using GetInLineSchool.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace GetInLineSchool.Controllers
{
    [ApiController]
    [Consumes("application/json")]
    [Route("api/schools")]
    public class SchoolController : ControllerBase
    {
        private readonly SchoolService _service = new SchoolService();
        
        [HttpPost]
        public async Task<IActionResult>Create([FromBody] School school)
        {
            try
            {
                var result = await _service.CreateSchoolAsync(school);

                if (result == -1)
                {
                    return Conflict(ServiceResult<School>.Failure(null, new List<Error>() { new Error { Key = "Global", Message = "Unique fields violation." } }));
                }
                if (result == 0)
                {
                    return StatusCode(500, ServiceResult<School>.Failure(null, new List<Error>() { new Error { Key = "Global", Message = "Failed to create school." } }));
                }

                return Ok(ServiceResult<School>.Success(school));
            }
            catch (SqlException ex)
            {
                return Conflict(ServiceResult<School>.Failure(null, new List<Error>() { new Error { Key = "SQL", Message = "SQL exception." } }));
            }
            
        }
    }
}