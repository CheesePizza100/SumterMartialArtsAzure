using MediatR;
using Microsoft.EntityFrameworkCore;
using SumterMartialArtsAzure.Server.DataAccess;
using SumterMartialArtsAzure.Server.Services;

namespace SumterMartialArtsAzure.Server.Api.Features.Students.UpdateMyContactInfo;

public class UpdateMyContactInfoHandler
    : IRequestHandler<UpdateMyContactInfoCommand, UpdateMyContactInfoCommandResponse>
{
    private readonly AppDbContext _dbContext;
    private readonly ICurrentUserService _currentUserService;

    public UpdateMyContactInfoHandler(
        AppDbContext dbContext,
        ICurrentUserService currentUserService)
    {
        _dbContext = dbContext;
        _currentUserService = currentUserService;
    }

    public async Task<UpdateMyContactInfoCommandResponse> Handle(UpdateMyContactInfoCommand request, CancellationToken cancellationToken)
    {
        var studentId = _currentUserService.GetStudentId();

        if (studentId == null)
            throw new UnauthorizedAccessException("User is not associated with a student");

        var student = await _dbContext.Students
            .FirstOrDefaultAsync(s => s.Id == studentId.Value, cancellationToken);

        if (student == null)
            throw new InvalidOperationException("Student not found");

        student.UpdateContactInfo(
            name: request.Name,
            email: request.Email,
            phone: request.Phone
        );

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new UpdateMyContactInfoCommandResponse(
            true,
            "Contact information updated successfully",
            student.Id
        );
    }
}