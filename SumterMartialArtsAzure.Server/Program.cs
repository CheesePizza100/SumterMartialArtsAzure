using MediatR;
using Microsoft.EntityFrameworkCore;
using SumterMartialArtsAzure.Server.Api;
using SumterMartialArtsAzure.Server.Api.Behaviors;
using SumterMartialArtsAzure.Server.Api.Middleware;
using SumterMartialArtsAzure.Server.DataAccess;
using SumterMartialArtsAzure.Server.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);

    // Order matters! These run in the order registered:
    // 1. Logging (first - logs everything)
    // 2. Validation (validates before executing)
    // 3. Exception Handling (catches exceptions from handlers)
    cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
    cfg.AddOpenBehavior(typeof(ExceptionHandlingBehavior<,>));

    cfg.NotificationPublisherType = typeof(LoggingNotificationPublisher);
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

var emailSettings = builder.Configuration.GetSection("EmailSettings").Get<EmailSettings>()
                    ?? throw new InvalidOperationException("EmailSettings not configured");

builder.Services
    .AddFluentEmail(emailSettings.FromEmail, emailSettings.FromName)
    .AddSmtpSender(
        emailSettings.SmtpServer,
        emailSettings.SmtpPort
    );

builder.Services.AddScoped<IEmailService, EmailService>();

// in a traditional vertical slice / CQRS-style API, each handler (like GetProgramsHandler) is a small service that performs a single operation.
// DbContext itself is a DI service. So, for ASP.NET Core to automatically inject it into your handler, the handler itself must also be managed by the DI container.
//builder.Services.AddScoped<GetProgramsHandler>();
//builder.Services.AddScoped<GetProgramByIdHandler>();
//builder.Services.AddScoped<GetInstructorsHandler>();
//builder.Services.AddScoped<GetInstructorByIdHandler>();
//builder.Services.AddScoped<GetInstructorAvailabilityHandler>();

builder.Services.AddHealthChecks();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.WithOrigins(
                    "https://localhost:4200",
                    "http://localhost:4200",
                    "https://jolly-smoke-0f6352e10.4.azurestaticapps.net",
                    "https://*.azurestaticapps.net"
                )
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});
var notificationHandlers = builder.Services
    .Where(d => d.ServiceType.IsGenericType &&
                d.ServiceType.GetGenericTypeDefinition() == typeof(INotificationHandler<>))
    .ToList();

Console.WriteLine($"=== Registered INotificationHandler implementations ({notificationHandlers.Count}): ===");
foreach (var handler in notificationHandlers)
{
    Console.WriteLine($"Service: {handler.ServiceType}, Implementation: {handler.ImplementationType?.Name ?? "Factory"}");
}
Console.WriteLine("=== End ===");
var app = builder.Build();
app.UseGlobalExceptionHandling();
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    DbSeeder.Seed(db);
}
app.UseCors("AllowFrontend");

// since we're separating frontend and backend, we need to remove the frontend hosting from your API.
// the visual studio project type web api and angular hosts both together
//app.UseDefaultFiles();
//app.UseStaticFiles();
//app.MapFallbackToFile("/index.html");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapProgramEndpoints();
app.MapHealthChecks("/health");

app.Run();

public class EmailSettings
{
    public string SmtpServer { get; set; } = string.Empty;
    public int SmtpPort { get; set; }
    public string SmtpUsername { get; set; } = string.Empty;
    public string SmtpPassword { get; set; } = string.Empty;
    public string FromEmail { get; set; } = string.Empty;
    public string FromName { get; set; } = string.Empty;
    public bool EnableSsl { get; set; }
}