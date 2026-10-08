using GetInLineSchool.DTOs.Request;
using GetInLineSchool.Repositories;

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
        
    }
}
