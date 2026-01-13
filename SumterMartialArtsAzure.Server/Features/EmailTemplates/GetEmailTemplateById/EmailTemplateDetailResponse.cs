namespace SumterMartialArtsAzure.Server.Api.Features.EmailTemplates.GetEmailTemplateById;

public record EmailTemplateDetailResponse(
    int Id,
    string TemplateKey,
    string Name,
    string Subject,
    string Body,
    string? Description,
    bool IsActive
);