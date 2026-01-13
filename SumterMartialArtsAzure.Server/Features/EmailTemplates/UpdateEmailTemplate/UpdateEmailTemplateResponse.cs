using SumterMartialArtsAzure.Server.Api.Auditing;

namespace SumterMartialArtsAzure.Server.Api.Features.EmailTemplates.UpdateEmailTemplate;

public record UpdateEmailTemplateResponse(int TemplateId, bool Success, string Message) : IAuditableResponse
{
    public string EntityId => TemplateId.ToString();

    public object GetAuditDetails() => new
    {
        Success,
        Message
    };
}