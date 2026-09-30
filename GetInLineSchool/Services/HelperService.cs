using MailKit.Net.Smtp;
using MimeKit;
using System.Security.Cryptography;
using DotNetEnv;
using GetInLineSchool.Repositories;

namespace GetInLineSchool.Services
{
    public class HelperService
    {
        public static int CreateSchoolCode()
        {
            return RandomNumberGenerator.GetInt32(100, 1_000);
        }

        public static async Task<int> SendEmail(string to,string subject,string? username)
        {
            int code = RandomNumberGenerator.GetInt32(100_000, 1_000_000);

            //First save code in database
            VerificationCodeRepository repository = new VerificationCodeRepository();

            var result = await repository.CreateCodeAsync(code);

            if (result == 0)
                return 0;

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

            return 1;
        }
    }
}
