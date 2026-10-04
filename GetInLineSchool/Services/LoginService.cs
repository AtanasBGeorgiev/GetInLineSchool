using GetInLineSchool.Repositories;
using GetInLineSchool.Services;
using GetInLineSchool.DTOs.Request;

namespace GetInLineSchool.Services
{
    public class LoginService
    {
        private readonly LoginRepository _loginRepository = new LoginRepository();

        public async Task<(StatusCodes Code,string?Token, string? Message)> LoginAsync(string username, string password, char role)
        {
            try
            {
                var result = role == 's' ? await _loginRepository.LoginStudentAsync(username) :
                             role == 'a' ? await _loginRepository.LoginAdminAsync(username) :
                                           await _loginRepository.LoginTeacherAsync(username);

                if (result == null)
                    return (StatusCodes.Unauthorized, null, "Invalid username.");

                if (!HashingService.Verify(password, result.Password))
                    return (StatusCodes.Unauthorized, null, "Invalid password.");

                var token = JWTService.CreateToken(result);
                return (StatusCodes.Success, token, null);
            }
            catch(Exception ex)
            {
                return (StatusCodes.ServerError, null, "An error occurred while creating the token or executing SQL.");
            }
        }

        public async Task<(StatusCodes Code, string? Message)> VerifyEmailAsync(VerifyEmailRequest request, string id, string role)
        {
            try
            {
                //check if target email is the authenticated user's email
                var email = await _loginRepository.GetEmailAsync(id, role);

                if (email == null)
                    return (StatusCodes.NotFound, "Email not found.");
                if (email != request.Email)
                    return (StatusCodes.BadRequest, "Can verify only your own email.");

                //get the verification code row from the database and check if it exists, is not expired, and matches the provided code
                var result = await _loginRepository.VerifyEmailAsync(request);
                if (result == null)
                    return (StatusCodes.NotFound, "Code not found.");

                if (result.ExpirationDate < HelperService.GetSecondsSinceEpoch())
                {
                    await _loginRepository.MarkCodeAsUsedAsync(result.IDCode);
                    return (StatusCodes.BadRequest, "Code has expired.");
                }

                if (!HashingService.Verify(request.Code, result.Code))
                {
                    await _loginRepository.AddAttemptAsync(result.IDCode);
                    if (result.Attempts + 1 >= 3)
                    {
                        await _loginRepository.MarkCodeAsUsedAsync(result.IDCode);
                        return (StatusCodes.BadRequest, "Invalid code.Reached attempts limit.");
                    }

                    return (StatusCodes.BadRequest, "Invalid code.");
                }

                await _loginRepository.MarkCodeAsUsedAsync(result.IDCode);
                return (StatusCodes.Success, null);
            }
            catch(Exception ex)
            {
                return (StatusCodes.ServerError, "SQL Exception occurred.");
            }
        }
    }
}
