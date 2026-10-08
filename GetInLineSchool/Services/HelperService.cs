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
        private static readonly DateTime Epoch = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static long GetSecondsSinceEpoch()
        {
            var bulgarianTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "FLE Standard Time");

            return (long)(bulgarianTime - Epoch).TotalSeconds;
        }

        public static int CreateSchoolCode()
        {
            return RandomNumberGenerator.GetInt32(100, 1_000);
        }

        public static async Task<(StatusCodes Code, string? Message)> SendEmail(string to, string subject, string? username)
        {
            int code = RandomNumberGenerator.GetInt32(100_000, 1_000_000);

            //First save code in database
            CodeRepository repository = new CodeRepository();

            var isThereCode = await repository.VerifyEmailAsync(to);//prevent creating codes uncontrolled
            if (isThereCode != null)
                return (StatusCodes.Conflict, "There is active code for you.");

            var recentCode = await repository.GetLastEmailAsync(to);
            if (recentCode.ExpirationDate > GetSecondsSinceEpoch() + 239)//there is recent created code
                return (StatusCodes.Conflict, "Wait 1 minute before requesting a new code.");

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
                    Text = username != null
                        ? "<b>Твоето потребителско име е: </b>" + username +
                          "<br><b>Това е тестова система и ако е получен имейл на реален имейл адрес, не му обръщайте внимание.</b><br>" +
                          "<b>Код за потвърждение:</b> " + code
                        : "<b>Това е тестова система и ако е получен имейл на реален имейл адрес, не му обръщайте внимание.</b><br>" +
                          "<b>Код за потвърждение:</b> " + code
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
