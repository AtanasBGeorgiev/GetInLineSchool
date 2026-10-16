using Microsoft.AspNetCore.Mvc;
using GetInLineSchool.DTOs.Request;
using GetInLineSchool.Services;
using GetInLineSchool.DTOs.Response;

namespace GetInLineSchool.Controllers
{
    [ApiController]
    [Route("api/login")]
    public class LoginController : BaseController
    {
        private readonly LoginService _service = new LoginService();

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            request.Username = request.Username.Trim();
            request.Password = request.Password.Trim();

            char firstChar = request.Username[0];

            var result = await _service.LoginAsync(request.Username, request.Password, firstChar);

            return await ReturnStatusCode<LoginResponse>(result.Code, result.Message, new LoginResponse { Token = result.Token });
        }
    }
}
