using SumterMartialArtsAzure.Server.Api.Features.EmailTemplates.GetEmailTemplateById;
using SumterMartialArtsAzure.Server.Api.Features.EmailTemplates.GetEmailTemplates;
using SumterMartialArtsAzure.Server.Api.Features.EmailTemplates.UpdateEmailTemplate;

namespace SumterMartialArtsAzure.Server.Api.EndpointConfigurations;

public static class EmailTemplatesEndpoints
{
    public static void Map(RouteGroupBuilder api)
    {
        var adminEmailTemplates = api.MapGroup("/admin/email-templates")
            .RequireAuthorization("AdminOnly");
        GetEmailTemplatesEndpoint.MapEndpoint(adminEmailTemplates);
        GetEmailTemplateByIdEndpoint.MapEndpoint(adminEmailTemplates);
        UpdateEmailTemplateEndpoint.MapEndpoint(adminEmailTemplates);
    }
}