using FluentEmail.Core;
using SumterMartialArtsAzure.Server.Domain.ValueObjects;

namespace SumterMartialArtsAzure.Server.Services.Email;

public class EmailSender
{
    private readonly IFluentEmail _fluentEmail;

    public EmailSender(IFluentEmail fluentEmail)
    {
        _fluentEmail = fluentEmail;
    }

    public Task SendAsync(string toEmail, string toName, EmailContent content)
    {
        return _fluentEmail
            .To(toEmail, toName)
            .Subject(content.Subject)
            .Body(content.Body, isHtml: true)
            .SendAsync();
    }
}