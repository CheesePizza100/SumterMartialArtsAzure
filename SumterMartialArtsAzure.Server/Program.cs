using System.Text;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SumterMartialArtsAzure.Server.Api;
using SumterMartialArtsAzure.Server.Api.Behaviors;
using SumterMartialArtsAzure.Server.Api.EndpointConfigurations;
using SumterMartialArtsAzure.Server.Api.Features.Auth.Login;
using SumterMartialArtsAzure.Server.Api.Features.PrivateLessons.GetPrivateLessons.Filters;
using SumterMartialArtsAzure.Server.Api.Middleware;
using SumterMartialArtsAzure.Server.DataAccess;
using SumterMartialArtsAzure.Server.Domain.Events;
using SumterMartialArtsAzure.Server.Domain.Services;
using SumterMartialArtsAzure.Server.Services;
using SumterMartialArtsAzure.Server.Services.Email;

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
    cfg.AddBehavior<IPipelineBehavior<LoginCommand, LoginCommandResponse>, LoginAuditingBehavior>();
    cfg.AddOpenBehavior(typeof(AuditingBehavior<,>));
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

builder.Services.AddScoped<ITokenGeneratorService, TokenGeneratorService>();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddScoped<IAuditService, AuditService>();
builder.Services.AddScoped<IPasswordHasher, BcryptPasswordHasher>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<EmailOrchestrator>();
builder.Services.AddScoped<EmailSender>();
builder.Services.AddScoped<EmailBodyParser>();
builder.Services.AddTransient<IEventProjector, EnrollmentEventProjector>();
builder.Services.AddTransient<IEventProjector, PromotionEventProjector>();
builder.Services.AddTransient<IEventProjector, TestAttemptEventProjector>();
builder.Services.AddTransient<IPrivateLessonFilter, PendingLessonsFilter>();
builder.Services.AddTransient<IPrivateLessonFilter, RecentLessonsFilter>();
builder.Services.AddTransient<IPrivateLessonFilter, AllLessonsFilter>();

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

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireRole("Admin"));

    options.AddPolicy("InstructorOrAdmin", policy =>
        policy.RequireRole("Instructor", "Admin"));

    options.AddPolicy("StudentOnly", policy =>
        policy.RequireRole("Student"));
});

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter your JWT token"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();
app.UseGlobalExceptionHandling();
app.UseAuthentication();
app.UseAuthorization();

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
app.MapEndpoints();
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