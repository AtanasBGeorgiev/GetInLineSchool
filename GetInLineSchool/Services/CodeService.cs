using GetInLineSchool.DTOs.Request;
using GetInLineSchool.Repositories;

namespace GetInLineSchool.Services
{
    public class CodeService
    {
        private readonly CodeRepository _repository = new CodeRepository();

        public async Task<(StatusCodes Code, string? Message)> VerifyEmailAsync(VerifyEmailRequest request, string id, string role)
        {
            try
            {
                //check if target email is the authenticated user's email
                var email = await _repository.GetEmailAsync(id, role);

                if (email == null)
                    return (StatusCodes.NotFound, "Email not found.");

                //get the verification code row from the database and check if it exists, is not expired, and matches the provided code
                var result = await _repository.VerifyEmailAsync(email);
                if (result == null)
                    return (StatusCodes.NotFound, "Code not found.");

                /*if (result.ExpirationDate < HelperService.GetSecondsSinceEpoch())
                {
                    await _loginRepository.MarkCodeAsUsedAsync(result.IDCode);
                    return (StatusCodes.BadRequest, "Code has expired.");
                }*/

                if (!HashingService.Verify(request.Code, result.Code))
                {
                    await _repository.AddAttemptAsync(result.IDCode);
                    if (result.Attempts + 1 >= 3)
                    {
                        await _repository.MarkCodeAsUsedAsync(result.IDCode);
                        return (StatusCodes.BadRequest, "Invalid code.Reached attempts limit.");
                    }

                    return (StatusCodes.BadRequest, "Invalid code.");
                }

                await _repository.MarkCodeAsUsedAsync(result.IDCode);
                await _repository.ActivateUserAsync(id, role);

                return (StatusCodes.Success, null);
            }
            catch (Exception ex)
            {
                return (StatusCodes.ServerError, "SQL Exception occurred.");
            }
        }

        public async Task<(StatusCodes Code, string? Message)> CreateVerificationCode(string id, string role)
        {
            try
            {
                var result = await _repository.CheckIsUserActiveAsync(id, role);
                if (result.Email == null)
                    return (StatusCodes.NotFound, "Email not found.");

                if (result.IsActive)
                    return (StatusCodes.BadRequest, "User's email is already verified.");

                var emailResult = await HelperService.SendEmail(result.Email, "Потвърждение на регистрацията и верификационен код", "");

                if (emailResult.Code == Services.StatusCodes.Success)
                    return (StatusCodes.Success, null);

                return (emailResult.Code, emailResult.Message);
            }
            catch (Exception)
            {
                return (StatusCodes.ServerError, "SQL error occured.");
            }
        }
    }
}
