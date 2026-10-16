using GetInLineSchool.Services;
using Microsoft.AspNetCore.Mvc;

namespace GetInLineSchool.Controllers
{
    public class BaseController : ControllerBase
    {
        protected async Task<IActionResult> ReturnStatusCode<T>(Services.StatusCodes code, string? message, T? successData)
        {
            return code == Services.StatusCodes.Success ?
                 Ok(ServiceResult<T>.Success(successData))
                : code == Services.StatusCodes.Created ?
                StatusCode(201, ServiceResult<T>.Success(default))
                 : code == Services.StatusCodes.BadRequest ?
                 BadRequest(ServiceResult<T>.Failure(default, new List<Error>() { new Error { Key = "Bad request", Message = message } }))
                : code == Services.StatusCodes.Forbidden ?
                StatusCode(403, ServiceResult<T>.Failure(default, new List<Error>() { new Error { Key = "Forbidden", Message = message } }))
                : code == Services.StatusCodes.Unauthorized ?
                 Unauthorized(ServiceResult<T>.Failure(default, new List<Error>() { new Error { Key = "Authorization", Message = message } }))
                : code == Services.StatusCodes.NotFound ?
                NotFound(ServiceResult<T>.Failure(default, new List<Error>() { new Error { Key = "Not found", Message = message } }))
                : code == Services.StatusCodes.Conflict ?
                Conflict(ServiceResult<T>.Failure(default, new List<Error>() { new Error { Key = "Conflict", Message = message } }))
                : StatusCode(500, ServiceResult<T>.Failure(default, new List<Error>() { new Error { Key = "Server error", Message = message } }));
        }
    }
}
