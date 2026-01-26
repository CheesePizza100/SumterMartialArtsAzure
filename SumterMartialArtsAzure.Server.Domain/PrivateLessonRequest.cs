using SumterMartialArtsAzure.Server.Domain.Common;
using SumterMartialArtsAzure.Server.Domain.Events;
using SumterMartialArtsAzure.Server.Domain.ValueObjects;

namespace SumterMartialArtsAzure.Server.Domain;

public class PrivateLessonRequest : Entity
{
    public int Id { get; private set; }
    public int InstructorId { get; private set; }
    public string StudentName { get; private set; } = string.Empty;
    public string StudentEmail { get; private set; } = string.Empty;
    public string? StudentPhone { get; private set; }
    public string? Notes { get; private set; }
    public LessonTime RequestedLessonTime { get; private set; } = null!;
    public RequestStatus Status { get; private set; } = RequestStatus.Pending;
    public string RejectionReason { get; private set; }
    public DateTime CreatedAt { get; private set; }

    // Navigation property
    public Instructor Instructor { get; set; } = null!;

    // Private constructor for EF Core
    private PrivateLessonRequest() { }

    /// <summary>
    /// Factory method to create a new private lesson request
    /// </summary>
    public static PrivateLessonRequest Create(
        int instructorId,
        string studentName,
        string studentEmail,
        string? studentPhone,
        LessonTime requestedLessonTime,
        string? notes = null)
    {
        if (string.IsNullOrWhiteSpace(studentName))
            throw new ArgumentException("Student name is required", nameof(studentName));

        if (string.IsNullOrWhiteSpace(studentEmail))
            throw new ArgumentException("Student email is required", nameof(studentEmail));

        if (requestedLessonTime.Start < DateTime.UtcNow)
            throw new InvalidOperationException("Cannot request a lesson in the past");

        var request = new PrivateLessonRequest
        {
            InstructorId = instructorId,
            StudentName = studentName,
            StudentEmail = studentEmail,
            StudentPhone = studentPhone,
            RequestedLessonTime = requestedLessonTime,
            Notes = notes,
            Status = RequestStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };

        // Raise domain event
        request.AddDomainEvent(new PrivateLessonRequestCreated(request.Id, request.InstructorId, request.StudentName, request.StudentEmail, request.RequestedLessonTime.Start, request.RequestedLessonTime.End)); 
        return request;
    }

    /// <summary>
    /// Approve the lesson request
    /// </summary>
    public void Approve()
    {
        if (!Status.CanTransitionTo(RequestStatus.Approved))
            throw new InvalidOperationException(
                $"Cannot approve request in {Status.Name} status. Only pending requests can be approved.");

        Status = RequestStatus.Approved;

        // Raise domain event
        AddDomainEvent(new PrivateLessonRequestApproved(Id, InstructorId, StudentName, StudentEmail, RequestedLessonTime.Start, RequestedLessonTime.End));
    }

    /// <summary>
    /// Reject the lesson request
    /// </summary>
    public void Reject(string reason)
    {
        if (!Status.CanTransitionTo(RequestStatus.Rejected))
            throw new InvalidOperationException(
                $"Cannot reject request in {Status.Name} status. Only pending requests can be rejected.");

        Status = RequestStatus.Rejected;
        RejectionReason = reason;

        // Raise domain event
        AddDomainEvent(new PrivateLessonRequestRejected(Id, InstructorId, StudentName, StudentEmail, reason));
    }

    /// <summary>
    /// Update the requested lesson time (only if still pending)
    /// </summary>
    public void UpdateStatus(RequestStatus newStatus)
    {
        if (newStatus == null)
            throw new ArgumentNullException(nameof(newStatus));

        // Optional: Add validation - e.g., can't go from Rejected back to Pending
        if (Status.IsRejected && newStatus.IsPending)
            throw new InvalidOperationException("Cannot revert a rejected request to pending");

        if (Status.IsApproved && newStatus.IsPending)
            throw new InvalidOperationException("Cannot revert an approved request to pending");

        Status = newStatus;
    }
}