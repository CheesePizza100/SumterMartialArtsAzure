using SumterMartialArtsAzure.Server.Domain.ValueObjects;
using SumterMartialArtsAzure.Server.Services.Email.ContentBuilders.Constants;

namespace SumterMartialArtsAzure.Server.Services.Email.ContentBuilders;

public class ProgramWithdrawalContentBuilder : IEmailContentBuilder
{
    private readonly string _studentName;
    private readonly string _withdrawnProgram;
    private readonly List<string> _remainingPrograms;
    private readonly EmailBodyParser _emailBodyParser;

    public string TemplateKey => EmailTemplateKeys.ProgramWithdrawal;

    public ProgramWithdrawalContentBuilder(
        string studentName,
        string withdrawnProgram,
        List<string> remainingPrograms)
    {
        _studentName = studentName;
        _withdrawnProgram = withdrawnProgram;
        _remainingPrograms = remainingPrograms;
        _emailBodyParser = new();
    }

    public EmailContent BuildFrom(string templateSubject, string templateBody)
    {
        var variables = new Dictionary<string, string>
        {
            { "StudentName", _studentName },
            { "WithdrawnProgram", _withdrawnProgram }
        };

        var emailContent = new EmailContent(templateSubject, templateBody).WithVariables(variables);

        var body = _emailBodyParser.ProcessConditional(
            emailContent.Body,
            "HasRemainingPrograms",
            _remainingPrograms.Any());

        body = _emailBodyParser.ProcessLoop(body, "RemainingPrograms", _remainingPrograms);

        return new EmailContent(emailContent.Subject, body);
    }
}