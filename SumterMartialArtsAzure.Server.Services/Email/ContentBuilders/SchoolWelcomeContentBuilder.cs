using SumterMartialArtsAzure.Server.Domain.ValueObjects;
using SumterMartialArtsAzure.Server.Services.Email.ContentBuilders.Constants;

namespace SumterMartialArtsAzure.Server.Services.Email.ContentBuilders;

public class SchoolWelcomeContentBuilder : IEmailContentBuilder
{
    private readonly string _studentName;

    public string TemplateKey => EmailTemplateKeys.SchoolWelcome;


    public SchoolWelcomeContentBuilder(string studentName)
    {
        _studentName = studentName;
    }

    public EmailContent BuildFrom(string templateSubject, string templateBody)
    {
        var variables = new Dictionary<string, string>
        {
            { "StudentName", _studentName }
        };

        return new EmailContent(templateSubject, templateBody).WithVariables(variables);
    }
}