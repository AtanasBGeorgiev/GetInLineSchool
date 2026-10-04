using GetInLineSchool.Models;
using GetInLineSchool.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GetInLineSchool.DTOs.Request;

namespace GetInLineSchool.Controllers
{
    [ApiController]
    [Consumes("application/json")]
    [Route("api/schools")]
    public class SchoolController : ControllerBase
    {
        private readonly SchoolService _service = new SchoolService();

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> Create([FromBody] CreateSchoolRequest school)
        {
            var result = await _service.CreateSchoolAsync(school);

            return result == Services.StatusCodes.Conflict ?
                Conflict(ServiceResult<School>.Failure(null, new List<Error>() { new Error { Key = "Global", Message = "Unique fields violation." } }))
            : result == Services.StatusCodes.ServerError ?
            StatusCode(500, ServiceResult<School>.Failure(null, new List<Error>() { new Error { Key = "Global", Message = "Failed to create school." } }))
            : StatusCode(201, (ServiceResult<School>.Success(null)));
        }
    }
}