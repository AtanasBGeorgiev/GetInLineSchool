using GetInLineSchool.Repositories;

namespace GetInLineSchool.Services
{
    public class LoginService
    {
        private readonly LoginRepository _loginRepository = new LoginRepository();

        public async Task<(StatusCodes Code, string? Message)> LoginAsync(string username, string password, char role)
        {
            var result = role == 's' ? await _loginRepository.LoginStudentAsync(username) :
                         role == 'a' ? await _loginRepository.LoginAdminAsync(username) :
                                       await _loginRepository.LoginTeacherAsync(username);
            
            if (result == null)
                return (StatusCodes.Unauthorized, "Invalid username.");

            return HashingService.Verify(password, result.Password) ?
                  (StatusCodes.Success, null)
                  : (StatusCodes.Unauthorized, "Invalid password.");
        }
    }
}
