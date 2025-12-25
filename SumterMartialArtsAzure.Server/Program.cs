using Microsoft.EntityFrameworkCore;
using SumterMartialArtsAzure.Server.Api;
using SumterMartialArtsAzure.Server.Api.Features.Instructors.GetInstructorAvailability;
using SumterMartialArtsAzure.Server.Api.Features.Instructors.GetInstructorById;
using SumterMartialArtsAzure.Server.Api.Features.Instructors.GetInstructors;
using SumterMartialArtsAzure.Server.Api.Features.Programs.GetProgramById;
using SumterMartialArtsAzure.Server.Api.Features.Programs.GetPrograms;
using SumterMartialArtsAzure.Server.Api.Middleware;
using SumterMartialArtsAzure.Server.DataAccess;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// in a traditional vertical slice / CQRS-style API, each handler (like GetProgramsHandler) is a small service that performs a single operation.
// DbContext itself is a DI service. So, for ASP.NET Core to automatically inject it into your handler, the handler itself must also be managed by the DI container.
builder.Services.AddScoped<GetProgramsHandler>();
builder.Services.AddScoped<GetProgramByIdHandler>();
builder.Services.AddScoped<GetInstructorsHandler>();
builder.Services.AddScoped<GetInstructorByIdHandler>();
builder.Services.AddScoped<GetInstructorAvailabilityHandler>();

builder.Services.AddHealthChecks();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.WithOrigins(
                    "http://localhost:4200", // Local development
                    "https://*.azurestaticapps.net" // Azure Static Web Apps
                )
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

//app.UseMiddleware<ExceptionHandlingMiddleware>();

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    DbSeeder.Seed(db);
}
app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowFrontend");

app.UseAuthorization();

app.MapProgramEndpoints();
app.MapFallbackToFile("/index.html");

app.MapHealthChecks("/health");

app.Run();