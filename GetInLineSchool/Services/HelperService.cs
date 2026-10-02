using MailKit.Net.Smtp;
using MimeKit;
using System.Security.Cryptography;
using GetInLineSchool.Repositories;

namespace GetInLineSchool.Services
{
    public enum StatusCodes
    {
        Success,//200
        Created,//201
        BadRequest,//400
        Unauthorized,//401
        Forbidden,//403
        NotFound,//404
        Conflict,//409
        ServerError//500
    }

    public class HelperService
    {
        public static int CreateSchoolCode()
        {
            return RandomNumberGenerator.GetInt32(100, 1_000);
        }

        public static async Task<(StatusCodes Code, string? Message)> SendEmail(string to, string subject, string? username)
        {
            int code = RandomNumberGenerator.GetInt32(100_000, 1_000_000);

            //First save code in database
            VerificationCodeRepository repository = new VerificationCodeRepository();

            string hashedCode = HashingService.Hash(code.ToString());

            var result = await repository.CreateCodeAsync(hashedCode,to);

            if (result == 0)
                return (StatusCodes.ServerError, "Failed to create verification code.");

            try
            {
                var email = new MimeMessage();

                email.From.Add(new MailboxAddress("Admin нареди се на опашка", "atanas23system@gmail.com"));
                email.To.Add(new MailboxAddress("Recipient", to));
                email.Subject = subject;

                email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                {
                    Text = username != null ? "<b>Твоето потребителско име е: </b>" + username +
                    "<br> <b> Код за потвърждение:</b> " + code :
                    "<b>Кoд за потвърждение:</b> " + code
                };

                using (var smtp = new SmtpClient())
                {
                    await smtp.ConnectAsync("smtp.gmail.com", 465, true);
                    await smtp.AuthenticateAsync("atanas23system@gmail.com", Environment.GetEnvironmentVariable("EMAIL_PASSWORD"));
                    await smtp.SendAsync(email);
                    await smtp.DisconnectAsync(true);
                }

                return (StatusCodes.Success, null);
            }
            catch (MailKit.Security.AuthenticationException)
            {
                return (StatusCodes.ServerError, "SMTP authentication failed.");
            }
            catch (MailKit.Net.Smtp.SmtpCommandException)
            {
                return (StatusCodes.ServerError, "SMTP server rejected the request.");
            }
            catch (MailKit.Net.Smtp.SmtpProtocolException)
            {
                return (StatusCodes.ServerError, "SMTP protocol error.");
            }
            catch (Exception)
            {
                return (StatusCodes.ServerError, "Unexpected error occurred.");
            }
        }
    }
}
