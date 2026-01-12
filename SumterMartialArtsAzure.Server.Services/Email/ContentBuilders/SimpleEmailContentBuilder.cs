using SumterMartialArtsAzure.Server.Domain.ValueObjects;

namespace SumterMartialArtsAzure.Server.Services.Email.ContentBuilders;

public class SimpleEmailContentBuilder : IEmailContentBuilder
{
    private readonly string _templateKey;
    private readonly Dictionary<string, string> _variables = new();

    public string TemplateKey => _templateKey;

    public SimpleEmailContentBuilder(string templateKey)
    {
        _templateKey = templateKey;
    }

    public SimpleEmailContentBuilder WithVariable(string key, string value)
    {
        _variables[key] = value;
        return this;
    }

    public EmailContent BuildFrom(string templateSubject, string templateBody)
    {
        return new EmailContent(templateSubject, templateBody).WithVariables(_variables);
    }
}