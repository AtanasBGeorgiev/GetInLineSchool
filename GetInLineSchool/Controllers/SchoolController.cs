using GetInLineSchool.DTOs.Request;
using GetInLineSchool.DTOs.Response;
using GetInLineSchool.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GetInLineSchool.Controllers
{
    [ApiController]
    [Consumes("application/json")]
    [Route("api/schools")]
    public class SchoolController : BaseController
    {
        private readonly SchoolService _service = new SchoolService();

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> Create([FromBody] CreateSchoolRequest school)
        {
            var result = await _service.CreateSchoolAsync(school);

            return await ReturnStatusCode<CreateSchoolResponse>(result.Code, result.Message, new CreateSchoolResponse { IDSchool = result.IdSchool });
        }
    }
}