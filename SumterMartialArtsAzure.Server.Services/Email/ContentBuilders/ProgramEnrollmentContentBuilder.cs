using SumterMartialArtsAzure.Server.Domain.ValueObjects;
using SumterMartialArtsAzure.Server.Services.Email.ContentBuilders.Constants;

namespace SumterMartialArtsAzure.Server.Services.Email.ContentBuilders;

public class ProgramEnrollmentContentBuilder : IEmailContentBuilder
{
    private readonly string _studentName;
    private readonly string _programName;
    private readonly string _initialRank;

    public string TemplateKey => EmailTemplateKeys.ProgramEnrollment;

    public ProgramEnrollmentContentBuilder(string studentName, string programName, string initialRank)
    {
        _studentName = studentName;
        _programName = programName;
        _initialRank = initialRank;
    }

    public EmailContent BuildFrom(string templateSubject, string templateBody)
    {
        var variables = new Dictionary<string, string>
        {
            { "StudentName", _studentName },
            { "ProgramName", _programName },
            { "InitialRank", _initialRank }
        };

        return new EmailContent(templateSubject, templateBody).WithVariables(variables);
    }
}