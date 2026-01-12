using MediatR;
using Microsoft.EntityFrameworkCore;
using SumterMartialArtsAzure.Server.DataAccess;

namespace SumterMartialArtsAzure.Server.Api.Features.EmailTemplates.GetEmailTemplates;

public class GetEmailTemplatesHandler
    : IRequestHandler<GetEmailTemplatesQuery, List<EmailTemplateResponse>>
{
    private readonly AppDbContext _dbContext;

    public GetEmailTemplatesHandler(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<List<EmailTemplateResponse>> Handle(
        GetEmailTemplatesQuery request,
        CancellationToken cancellationToken)
    {
        return _dbContext.EmailTemplates
            .AsNoTracking()
            .OrderBy(t => t.Name)
            .Select(t => new EmailTemplateResponse(
                t.Id,
                t.TemplateKey,
                t.Name,
                t.Subject,
                t.Description,
                t.IsActive,
                t.UpdatedAt ?? t.CreatedAt
            )).ToListAsync(cancellationToken);
    }
}