using FluentEmail.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SumterMartialArtsAzure.Server.DataAccess;
using SumterMartialArtsAzure.Server.Domain.ValueObjects;

namespace SumterMartialArtsAzure.Server.Services;

public class EmailBuilder
{
    private readonly IFluentEmail _fluentEmail;
    private readonly AppDbContext _dbContext;
    private readonly ILogger<EmailBuilder> _logger;

    public EmailBuilder(
        IFluentEmail fluentEmail,
        AppDbContext dbContext,
        ILogger<EmailBuilder> logger)
    {
        _fluentEmail = fluentEmail;
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<EmailContent?> GetTemplateAsync(string templateKey)
    {
        var template = await _dbContext.EmailTemplates
            .AsNoTracking()
            .SingleAsync(t => t.TemplateKey == templateKey && t.IsActive);

        return new EmailContent(template.Subject, template.Body);
    }

    public async Task<bool> SendAsync(
        string toEmail,
        string toName,
        EmailContent emailContent)
    {
        var result = await _fluentEmail
            .To(toEmail, toName)
            .Subject(emailContent.Subject)
            .Body(emailContent.Body, isHtml: true)
            .SendAsync();

        if (result.Successful)
        {
            _logger.LogInformation("Email sent successfully to {Email}", toEmail);
            return true;
        }

        _logger.LogError("Failed to send email to {Email}: {Errors}",
            toEmail, string.Join(", ", result.ErrorMessages));
        return false;
    }
}