namespace SumterMartialArtsAzure.Server.Api.Features.EmailTemplates.GetEmailTemplates;

public record EmailTemplateResponse(
    int Id,
    string TemplateKey,
    string Name,
    string Subject,
    string? Description,
    bool IsActive,
    DateTime LastModified
);