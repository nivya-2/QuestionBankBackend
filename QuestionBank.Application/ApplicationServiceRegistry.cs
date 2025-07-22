using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using QuestionBank.Application.Behaviors;
using FluentValidation;
using QuestionBank.Application.Features.QuestionManagement;

namespace QuestionBank.Application;

public static class ApplicationServiceRegistry
{
    /// <summary>
    /// Registers application-level services, including MediatR handlers from the current assembly.
    /// </summary>
    /// <param name="services">The service collection to which services are added.</param>
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ExceptionHandlingBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddValidatorsFromAssemblyContaining<SetQuestionsCommandValidator>();
    }
}
