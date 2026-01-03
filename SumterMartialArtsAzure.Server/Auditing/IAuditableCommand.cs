namespace SumterMartialArtsAzure.Server.Api.Auditing;

public interface IAuditableCommand
{
    string Action { get; }
    string EntityType { get; }
}

public interface IAuditableResponse
{
    string EntityId { get; }
    object GetAuditDetails();
}