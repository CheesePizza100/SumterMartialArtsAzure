using Ardalis.SmartEnum;

namespace SumterMartialArtsAzure.Server.Domain.ValueObjects;

public class RequestStatus : SmartEnum<RequestStatus>
{
    public static readonly RequestStatus Pending = new(nameof(Pending), 1);
    public static readonly RequestStatus Approved = new(nameof(Approved), 2);
    public static readonly RequestStatus Rejected = new(nameof(Rejected), 3);

    // Private parameterless constructor for EF Core (required for migrations)
    private RequestStatus() : base(string.Empty, 0) { }

    private RequestStatus(string name, int value) : base(name, value) { }

    public bool CanTransitionTo(RequestStatus newStatus)
    {
        // Only Pending can transition to Approved or Rejected
        return this == Pending && (newStatus == Approved || newStatus == Rejected);
    }

    public bool IsPending => this == Pending;
    public bool IsApproved => this == Approved;
    public bool IsRejected => this == Rejected;
}