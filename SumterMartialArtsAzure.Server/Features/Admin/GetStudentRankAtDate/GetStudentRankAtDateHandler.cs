using System.Text.Json;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SumterMartialArtsAzure.Server.DataAccess;
using SumterMartialArtsAzure.Server.Domain.Events;

namespace SumterMartialArtsAzure.Server.Api.Features.Admin.GetStudentRankAtDate;

public class GetStudentRankAtDateHandler
    : IRequestHandler<GetStudentRankAtDateQuery, GetStudentRankAtDateResponse>
{
    private readonly AppDbContext _dbContext;

    public GetStudentRankAtDateHandler(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<GetStudentRankAtDateResponse> Handle(
        GetStudentRankAtDateQuery request,
        CancellationToken cancellationToken)
    {
        // Get all events up to the specified date
        var events = await _dbContext.StudentProgressionEvents
            .AsNoTracking()
            .Where(e => e.StudentId == request.StudentId
                        && e.ProgramId == request.ProgramId
                        && e.OccurredAt <= request.AsOfDate)
            .OrderBy(e => e.Version)
            .ToListAsync(cancellationToken);

        if (!events.Any())
            return null;

        // Replay events to reconstruct state
        string currentRank = "Not Enrolled";
        DateTime? enrolledDate = null;
        DateTime? lastTestDate = null;
        string? lastTestNotes = null;

        foreach (var evt in events)
        {
            switch (evt.EventType)
            {
                case "EnrollmentEventData":
                    var enrollData = JsonSerializer.Deserialize<EnrollmentEventData>(evt.EventData);
                    currentRank = enrollData?.InitialRank ?? "Unknown";
                    enrolledDate = evt.OccurredAt;
                    break;

                case "PromotionEventData":
                    var promotionData = JsonSerializer.Deserialize<PromotionEventData>(evt.EventData);
                    if (promotionData != null)
                    {
                        currentRank = promotionData.ToRank;
                        lastTestDate = evt.OccurredAt;
                        lastTestNotes = promotionData.Reason;
                    }
                    break;

                case "TestAttemptEventData":
                    var testData = JsonSerializer.Deserialize<TestAttemptEventData>(evt.EventData);
                    if (testData != null)
                    {
                        lastTestDate = evt.OccurredAt;
                        lastTestNotes = testData.InstructorNotes;
                    }
                    break;
            }
        }

        return new GetStudentRankAtDateResponse(
            currentRank,
            enrolledDate,
            lastTestDate,
            lastTestNotes,
            events.Count
        );
    }
}