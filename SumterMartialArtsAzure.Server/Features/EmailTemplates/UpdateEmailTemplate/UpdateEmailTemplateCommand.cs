using MediatR;
using SumterMartialArtsAzure.Server.Api.Auditing;
using SumterMartialArtsAzure.Server.Domain;

namespace SumterMartialArtsAzure.Server.Api.Features.EmailTemplates.UpdateEmailTemplate;

public record UpdateEmailTemplateRequest(
    string Name,
    string Subject,
    string Body,
    string? Description
);

public record UpdateEmailTemplateCommand(
    int Id,
    string Name,
    string Subject,
    string Body,
    string? Description
) : IRequest<UpdateEmailTemplateResponse>, IAuditableCommand
{
    public string Action => AuditActions.EmailTemplateUpdated;
    public string EntityType => "EmailTemplate";
}