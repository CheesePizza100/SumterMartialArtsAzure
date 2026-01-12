using SumterMartialArtsAzure.Server.Domain.ValueObjects;
using SumterMartialArtsAzure.Server.Services.Email.ContentBuilders.Constants;

namespace SumterMartialArtsAzure.Server.Services.Email.ContentBuilders;

public class InstructorLoginCredentialsContentBuilder : IEmailContentBuilder
{
    private readonly string _instructorName;
    private readonly string _userName;
    private readonly string _temporaryPassword;

    public string TemplateKey => EmailTemplateKeys.InstructorLoginCredentials;

    public InstructorLoginCredentialsContentBuilder(string instructorName, string userName, string temporaryPassword)
    {
        _instructorName = instructorName;
        _userName = userName;
        _temporaryPassword = temporaryPassword;
    }

    public EmailContent BuildFrom(string templateSubject, string templateBody)
    {
        var variables = new Dictionary<string, string>
        {
            { "StudentName", _instructorName },
            { "UserName", _userName },
            { "TemporaryPassword", _temporaryPassword }
        };

        return new EmailContent(templateSubject, templateBody).WithVariables(variables);
    }
}