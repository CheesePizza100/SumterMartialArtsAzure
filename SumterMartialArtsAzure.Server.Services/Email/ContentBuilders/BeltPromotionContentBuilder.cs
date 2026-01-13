using SumterMartialArtsAzure.Server.Domain.ValueObjects;
using SumterMartialArtsAzure.Server.Services.Email.ContentBuilders.Constants;

namespace SumterMartialArtsAzure.Server.Services.Email.ContentBuilders;

public class BeltPromotionContentBuilder : IEmailContentBuilder
{
    private readonly string _studentName;
    private readonly string _programName;
    private readonly string _fromRank;
    private readonly string _toRank;
    private readonly DateTime _promotionDate;
    private readonly string _instructorNotes;

    public string TemplateKey => EmailTemplateKeys.BeltPromotion;

    public BeltPromotionContentBuilder(
        string studentName,
        string programName,
        string fromRank,
        string toRank,
        DateTime promotionDate,
        string instructorNotes)
    {
        _studentName = studentName;
        _programName = programName;
        _fromRank = fromRank;
        _toRank = toRank;
        _promotionDate = promotionDate;
        _instructorNotes = instructorNotes;
    }

    public EmailContent BuildFrom(string templateSubject, string templateBody)
    {
        var variables = new Dictionary<string, string>
        {
            { "StudentName", _studentName },
            { "ProgramName", _programName },
            { "FromRank", _fromRank },
            { "ToRank", _toRank },
            { "PromotionDate", _promotionDate.ToString("MMMM dd, yyyy") },
            { "InstructorNotes", _instructorNotes }
        };

        return new EmailContent(templateSubject, templateBody).WithVariables(variables);
    }
}