using GetInLineSchool.DTOs.Response;
using GetInLineSchool.Models;
using GetInLineSchool.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Superpower.Model;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

namespace GetInLineSchool.Controllers
{
    public class BaseController : ControllerBase
    {
        protected async Task<IActionResult> ReturnStatusCode<T>(Services.StatusCodes code, string? message)
        {
            return code == Services.StatusCodes.Success ?
                 Ok(ServiceResult<VerificationCode>.Success(null))
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
