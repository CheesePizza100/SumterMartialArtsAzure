using SumterMartialArtsAzure.Server.Domain.ValueObjects;
using SumterMartialArtsAzure.Server.Services.Email.ContentBuilders.Constants;

namespace SumterMartialArtsAzure.Server.Services.Email.ContentBuilders;

public class StudentLoginCredentialsContentBuilder : IEmailContentBuilder
{
    private readonly string _studentName;
    private readonly string _userName;
    private readonly string _temporaryPassword;

    public string TemplateKey => EmailTemplateKeys.StudentLoginCredentials;

    public StudentLoginCredentialsContentBuilder(string studentName, string userName, string temporaryPassword)
    {
        _studentName = studentName;
        _userName = userName;
        _temporaryPassword = temporaryPassword;
    }

    public EmailContent BuildFrom(string templateSubject, string templateBody)
    {
        var variables = new Dictionary<string, string>
        {
            { "StudentName", _studentName },
            { "UserName", _userName },
            { "TemporaryPassword", _temporaryPassword }
        };

        return new EmailContent(templateSubject, templateBody).WithVariables(variables);
    }
}