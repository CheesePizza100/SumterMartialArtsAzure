using MediatR;

namespace SumterMartialArtsAzure.Server.Api.Features.EmailTemplates.GetEmailTemplates;

public record GetEmailTemplatesQuery : IRequest<List<EmailTemplateResponse>>;