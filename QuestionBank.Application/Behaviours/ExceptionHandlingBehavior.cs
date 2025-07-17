using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace QuestionBank.Application.Behaviors;

/// <summary>
/// Represents structured details about an exception, to be returned in the API response.
/// </summary>
public class ExceptionDetails
{
    /// <summary>Gets or sets the HTTP status code for the error response.</summary>
    /// <example>404</example>
    public int StatusCode { get; set; }

    /// <summary>Gets or sets a short title describing the type of error.</summary>
    /// <example>Not Found</example>
    public string Title { get; set; } = "Error";

    /// <summary>Gets or sets a detailed message explaining the error.</summary>
    /// <example>The interview with ID 42 was not found in the database.</example>
    public string Message { get; set; } = "Something went wrong.";

    /// <summary>Gets or sets the type of the exception (e.g., ArgumentException).</summary>
    /// <example>KeyNotFoundException</example>
    public string ErrorType { get; set; } = "Exception";
}

/// <summary>
/// A MediatR pipeline behavior that globally handles exceptions thrown during request processing
/// and writes a structured JSON error response to the HTTP context.
/// </summary>
/// <typeparam name="TRequest">The type of the request.</typeparam>
/// <typeparam name="TResponse">The type of the response.</typeparam>
public class ExceptionHandlingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExceptionHandlingBehavior{TRequest, TResponse}"/> class.
    /// </summary>
    /// <param name="httpContextAccessor">The accessor for the current HTTP context.</param>
    public ExceptionHandlingBehavior(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    /// <summary>
    /// Handles execution of the MediatR pipeline. Catches unhandled exceptions and formats them as JSON responses.
    /// </summary>
    /// <param name="request">The incoming request.</param>
    /// <param name="next">The next delegate in the pipeline.</param>
    /// <param name="cancellationToken">A token that can be used to cancel the operation.</param>
    /// <returns>The result of the handler or a default response in case of an error.</returns>
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        try
        {
            return await next();
        }
        catch (Exception ex)
        {
            var context = _httpContextAccessor.HttpContext;

            var error = new ExceptionDetails
            {
                Message = ex switch
                {
                    DbUpdateException => "Invalid foreign key or entity reference. Check that all referenced IDs exist.",
                    ValidationException validationEx => string.Join(" | ", validationEx.Errors.Select(e => e.ErrorMessage)),
                    _ => ex.Message
                },
                ErrorType = ex.GetType().Name,
                StatusCode = ex switch
                {
                    KeyNotFoundException => StatusCodes.Status404NotFound,
                    ArgumentException => StatusCodes.Status400BadRequest,
                    DbUpdateException => StatusCodes.Status400BadRequest, 
                    UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
                    ValidationException => StatusCodes.Status400BadRequest,
                    _ => StatusCodes.Status500InternalServerError
                },
                Title = ex switch
                {
                    KeyNotFoundException => "Not Found",
                    ArgumentException => "Bad Request",
                    ValidationException => "Validation Failed",
                    UnauthorizedAccessException => "Unauthorized",
                    DbUpdateException => "Bad Request",
                    _ => "Internal Server Error"
                }
            };

            if (context != null && !context.Response.HasStarted)
            {
                context.Response.StatusCode = error.StatusCode;
                context.Response.ContentType = "application/json";
                var json = JsonSerializer.Serialize(error);
                await context.Response.WriteAsync(json, Encoding.UTF8);
            }
            return default!;
        }
    }
}
