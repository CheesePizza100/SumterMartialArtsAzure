using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace SumterMartialArtsAzure.Server.DataAccess;

public class AppDbContextFactory
    : IDesignTimeDbContextFactory<AppDbContext>
{
    private readonly IMediator _mediator;

    public AppDbContextFactory(IMediator mediator)
    {
        _mediator = mediator;
    }

    public AppDbContextFactory()
    {
    }

    public AppDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

        // Use connection string from appsettings.json
        optionsBuilder.UseSqlServer(
            configuration.GetConnectionString("DefaultConnection"));
        return new AppDbContext(optionsBuilder.Options, _mediator);
    }
}