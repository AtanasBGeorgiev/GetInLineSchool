using GetInLineSchool.DTOs.Request;
using GetInLineSchool.DTOs.Response;
using GetInLineSchool.Models;
using GetInLineSchool.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

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

            return result.Code == Services.StatusCodes.Conflict ?
                Conflict(ServiceResult<CreateSchoolResponse>.Failure(null, new List<Error>() { new Error { Key = "Global", Message = "Unique fields violation." } }))
            : result.Code == Services.StatusCodes.ServerError ?
            StatusCode(500, ServiceResult<CreateSchoolResponse>.Failure(null, new List<Error>() { new Error { Key = "Global", Message = "Failed to create school." } }))
            : StatusCode(201, (ServiceResult<CreateSchoolResponse>.Success(new CreateSchoolResponse { IDSchool = result.IdSchool })));
        }
    }
}