namespace SumterMartialArtsAzure.Server.Domain.ValueObjects;

public class EmailContent
{
    private readonly string _subject;
    private readonly string _body;

    public string Subject => _subject;
    public string Body => _body;

    public EmailContent(string subject, string body)
    {
        _subject = subject;
        _body = body;
    }

    public EmailContent WithVariables(Dictionary<string, string> variables)
    {
        var subject = _subject;
        var body = _body;

        foreach (var variable in variables)
        {
            subject = subject.Replace($"{{{{{variable.Key}}}}}", variable.Value);
            body = body.Replace($"{{{{{variable.Key}}}}}", variable.Value);
        }

        return new EmailContent(subject, body);
    }
}