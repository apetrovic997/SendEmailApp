using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;

class Program
{
    static async Task Main(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        var receiver = configuration["Email:To"];
        var subject = configuration["Email:Subject"];
        var emailBody = configuration["Email:EmailBody"];

        var email = Environment.GetEnvironmentVariable("EMAIL_USERNAME");
        var password = Environment.GetEnvironmentVariable("EMAIL_PASSWORD");

        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("Danilo Petrovic", email));
        message.To.Add(new MailboxAddress("KOMUNALNA MILICIJA", receiver));
        message.Subject = subject;

        var builder = new BodyBuilder
        {
            TextBody = emailBody
        };

        builder.Attachments.Add("Attachments/saobracajna.pdf");

        message.Body = builder.ToMessageBody();

        using var client = new SmtpClient();
        await client.ConnectAsync("smtp.gmail.com", 587, false);
        await client.AuthenticateAsync(email, password);
        await client.SendAsync(message);
        await client.DisconnectAsync(true);

        Console.WriteLine("Email je uspešno poslat!");
    }
}





