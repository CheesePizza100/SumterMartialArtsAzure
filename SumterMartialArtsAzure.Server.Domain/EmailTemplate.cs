namespace SumterMartialArtsAzure.Server.Domain;

public class EmailTemplate
{
    public int Id { get; private set; }
    public string TemplateKey { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;
    public string Subject { get; private set; } = string.Empty;
    public string Body { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    // EF Core constructor
    private EmailTemplate() { }

    public EmailTemplate(
        string templateKey,
        string name,
        string subject,
        string body,
        string? description = null)
    {
        TemplateKey = templateKey;
        Name = name;
        Subject = subject;
        Body = body;
        Description = description;
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
    }

    public void Update(string name, string subject, string body, string? description = null)
    {
        Name = name;
        Subject = subject;
        Body = body;
        Description = description;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Activate() => IsActive = true;
    public void Deactivate() => IsActive = false;
}