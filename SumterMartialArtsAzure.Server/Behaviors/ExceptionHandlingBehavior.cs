using MediatR;

namespace SumterMartialArtsAzure.Server.Api.Behaviors;

public class ExceptionHandlingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<ExceptionHandlingBehavior<TRequest, TResponse>> _logger;

    public ExceptionHandlingBehavior(ILogger<ExceptionHandlingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        try
        {
            return await next();
        }
        catch (InvalidOperationException ex)
        {
            // Business rule violations - these are expected, don't log as errors
            _logger.LogWarning(
                ex,
                "Business rule violation in {RequestName}: {Message}",
                typeof(TRequest).Name,
                ex.Message
            );

            // If the response type supports failure results, return that
            // Otherwise, re-throw to be caught by global middleware
            throw;
        }
        catch (ArgumentException ex)
        {
            // Validation errors - expected exceptions
            _logger.LogWarning(
                ex,
                "Validation error in {RequestName}: {Message}",
                typeof(TRequest).Name,
                ex.Message
            );

            throw;
        }
        catch (Exception ex)
        {
            // Unexpected exceptions - log as error
            _logger.LogError(
                ex,
                "Unhandled exception in {RequestName}",
                typeof(TRequest).Name
            );

            throw; // Let global middleware handle it
        }
    }
}