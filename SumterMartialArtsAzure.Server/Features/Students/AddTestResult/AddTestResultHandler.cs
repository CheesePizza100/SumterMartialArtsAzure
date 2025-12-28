using MediatR;
using Microsoft.EntityFrameworkCore;
using SumterMartialArtsAzure.Server.DataAccess;

namespace SumterMartialArtsAzure.Server.Api.Features.Students.AddTestResult;

public class AddTestResultHandler
    : IRequestHandler<AddTestResultCommand, AddTestResultResponse>
{
    private readonly AppDbContext _dbContext;

    public AddTestResultHandler(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<AddTestResultResponse> Handle(
        AddTestResultCommand request,
        CancellationToken cancellationToken)
    {
        var student = await _dbContext.Students
            .Include(s => s.ProgramEnrollments)
            .Include(s => s.TestHistory)
            .FirstOrDefaultAsync(s => s.Id == request.StudentId, cancellationToken);

        if (student == null)
            return new AddTestResultResponse(false, "Student not found");

        try
        {
            // Use aggregate business method
            student.RecordTestResult(
                programId: request.ProgramId,
                programName: request.ProgramName,
                rankAchieved: request.Rank,
                passed: request.Result.Equals("Pass", StringComparison.OrdinalIgnoreCase),
                notes: request.Notes,
                testDate: request.TestDate
            );

            await _dbContext.SaveChangesAsync(cancellationToken);

            return new AddTestResultResponse(true, "Test result recorded successfully");
        }
        catch (InvalidOperationException ex)
        {
            return new AddTestResultResponse(false, ex.Message);
        }
    }
}