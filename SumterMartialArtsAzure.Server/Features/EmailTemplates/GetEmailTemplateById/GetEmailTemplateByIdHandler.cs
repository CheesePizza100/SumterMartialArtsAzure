using MediatR;
using Microsoft.EntityFrameworkCore;
using SumterMartialArtsAzure.Server.DataAccess;

namespace SumterMartialArtsAzure.Server.Api.Features.EmailTemplates.GetEmailTemplateById;

public class GetEmailTemplateByIdHandler
    : IRequestHandler<GetEmailTemplateByIdQuery, EmailTemplateDetailResponse?>
{
    private readonly AppDbContext _dbContext;

    public GetEmailTemplateByIdHandler(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<EmailTemplateDetailResponse?> Handle(
        GetEmailTemplateByIdQuery request,
        CancellationToken cancellationToken)
    {
        return _dbContext.EmailTemplates
            .AsNoTracking()
            .Where(t => t.Id == request.Id)
            .Select(t => new EmailTemplateDetailResponse(
                t.Id,
                t.TemplateKey,
                t.Name,
                t.Subject,
                t.Body,
                t.Description,
                t.IsActive
            )).FirstOrDefaultAsync(cancellationToken);
    }
}