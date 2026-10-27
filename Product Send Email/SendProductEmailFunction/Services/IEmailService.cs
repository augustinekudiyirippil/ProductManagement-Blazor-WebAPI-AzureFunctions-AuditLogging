using System;
using System.Collections.Generic;
using System.Text;

namespace SendProductEmailFunction.Services;

public interface IEmailService
{
    Task SendEmailAsync(
        string subject,
        string body);
}