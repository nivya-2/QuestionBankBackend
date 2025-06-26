using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace QuestionBank.Application;

public static class ApplicationServiceRegistry
{
    /// <summary>
    /// Adds the Application services.
    /// </summary>
    /// <param name="services">The services.</param>
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });
    }
}
