using System;
using System.Collections.Generic;
using System.Text;

using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace SendProductEmailFunction.Services;

public class EmailService : IEmailService
{
    public async Task SendEmailAsync(
        string subject,
        string body)
    {
        var email = new MimeMessage();

        email.From.Add(
            MailboxAddress.Parse(
                "@gmail.com"));

        email.To.Add(
            MailboxAddress.Parse(
                ""));

        email.Subject = subject;

        email.Body = new TextPart("plain")
        {
            Text = body
        };

        using var smtpClient = new SmtpClient();

        await smtpClient.ConnectAsync(
            "smtp.gmail.com",
            587,
            SecureSocketOptions.StartTls);

        await smtpClient.AuthenticateAsync(
            "@gmail.com",
            "app password here");

        await smtpClient.SendAsync(email);

        await smtpClient.DisconnectAsync(true);
    }
}