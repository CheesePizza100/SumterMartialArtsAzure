using MediatR;

namespace SumterMartialArtsAzure.Server.Api.Features.EmailTemplates.GetEmailTemplateById;

public record GetEmailTemplateByIdQuery(int Id) : IRequest<EmailTemplateDetailResponse?>;