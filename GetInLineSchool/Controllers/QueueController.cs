using GetInLineSchool.DTOs.Request;
using GetInLineSchool.DTOs.Response;
using GetInLineSchool.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GetInLineSchool.Controllers
{
    [ApiController]
    [Route("api/queue")]
    public class QueueController : BaseController
    {
        private readonly QueueService _service = new QueueService();

        [HttpPost]
        [Authorize(Policy = "Queue")]
        public async Task<IActionResult> CreateQueue([FromBody] CreateQueueRequest request)
        {
            int creatorId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "id")?.Value);
            string role = User.FindFirst(ClaimTypes.Role)?.Value;

            var result = await _service.CreateQueueAsync(request, creatorId, role);

            return await ReturnStatusCode<CreateQueueResponse>(result.Code, result.Message, result.Response);
        }
    }
}
