namespace SumterMartialArtsAzure.Server.Services.Email;

public class EmailBodyParser
{
    public string ProcessConditional(string body, string conditionalKey, bool condition)
    {
        if (condition)
        {
            // Remove the "if" tag
            body = body.Replace($"{{{{#if {conditionalKey}}}}}", "");

            // Remove everything from {{else}} to {{/if}}
            var elseIndex = body.IndexOf("{{else}}");
            var endIfIndex = body.IndexOf("{{/if}}");
            if (elseIndex >= 0 && endIfIndex > elseIndex)
            {
                body = body.Remove(elseIndex, endIfIndex - elseIndex + "{{/if}}".Length);
            }
        }
        else
        {
            // Remove everything from {{#if...}} to {{else}}
            var ifIndex = body.IndexOf($"{{{{#if {conditionalKey}}}}}");
            var elseIndex = body.IndexOf("{{else}}");
            if (ifIndex >= 0 && elseIndex > ifIndex)
            {
                body = body.Remove(ifIndex, elseIndex - ifIndex + "{{else}}".Length);
            }
            body = body.Replace("{{/if}}", "");
        }

        return body;
    }

    public string ProcessLoop(string body, string loopKey, List<string> items)
    {
        var itemsList = items.Any() ? string.Join("</li><li>", items) : "";

        body = body.Replace($"{{{{#each {loopKey}}}}}", "");
        body = body.Replace("{{this}}", itemsList);
        body = body.Replace("{{/each}}", "");

        return body;
    }
}