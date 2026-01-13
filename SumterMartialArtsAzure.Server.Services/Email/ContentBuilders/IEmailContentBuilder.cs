using SumterMartialArtsAzure.Server.Domain.ValueObjects;

namespace SumterMartialArtsAzure.Server.Services.Email.ContentBuilders;

public interface IEmailContentBuilder
{
    string TemplateKey { get; }
    EmailContent BuildFrom(string templateSubject, string templateBody);
}