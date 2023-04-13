using Microsoft.Extensions.Logging;
using NovaRecipesProject.Services.EmailSender.Models;
using MailKit.Net.Smtp;
using MimeKit;
using NovaRecipesProject.Services.MailSender;

namespace NovaRecipesProject.Services.EmailSender;

/// <inheritdoc />
public class EmailSender : IEmailSender
{
    private readonly string _smtpServer;
    private readonly int _smtpPort;
    private readonly bool _useSsl;
    private readonly string _username;
    private readonly string _password;
    /// <summary>
    /// Logger used to log data about sending emails 
    /// </summary>
    private ILogger<EmailSender> Logger { get; }

    /// <summary>
    /// Main constructor
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="settings">settings arg from DI</param>
    public EmailSender(
        ILogger<EmailSender> logger, 
        EmailSenderSettings settings
        )
    {
        Logger = logger;
        _smtpServer = settings.SmtpServer;
        _smtpPort = settings.SmtpPort;
        _useSsl = settings.UseSsl;
        _username = settings.Username;
        _password = settings.Password;
    }

    /// <inheritdoc />
    public void Send(string from, string to, string subject, string body)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("Site administration", from));
        message.To.Add(new MailboxAddress("Receiver", to));
        message.Subject = subject;

        var builder = new BodyBuilder
        {
            TextBody = body
        };
        message.Body = builder.ToMessageBody();

        using var client = new SmtpClient();

        client.Connect(_smtpServer, _smtpPort, _useSsl);
        client.Authenticate(_username, _password);
        client.Send(message);
        client.Disconnect(true);
    }

    /// <inheritdoc />
    public async Task SendAsync(string from, string to, string subject, string body)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("", from));
        message.To.Add(new MailboxAddress("", to));
        message.Subject = subject;

        var builder = new BodyBuilder
        {
            TextBody = body
        };
        message.Body = builder.ToMessageBody();

        using var client = new SmtpClient();

        await client.ConnectAsync(_smtpServer, _smtpPort, _useSsl);
        await client.AuthenticateAsync(_username, _password);
        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }

    /// <inheritdoc />
    public async Task SendAsync(EmailModel data)
    {
        await SendAsync(
            data.From, 
            data.Email, 
            data.Subject, 
            data.Message
            );
    }
}
