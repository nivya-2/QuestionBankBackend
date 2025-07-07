using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QuestionBank.Application.Contracts.Persistence;
using Microsoft.EntityFrameworkCore;

namespace QuestionBank.Infrastructure;

public static class InfrastructureServiceRegistry
{
    /// <summary>
    /// Adds infrastructure services to the dependency injection container, including the database context.
    /// </summary>
    /// <param name="services">The service collection to which services are added.</param>
    /// <param name="configuration">The application configuration object, used to access connection strings.</param>
    public static void AddInfrastructureServices(this IServiceCollection services,
    IConfiguration configuration)
    {
        services.AddEntityFrameworkNpgsql().AddDbContext<QuestionBankDbContext>(options =>
        options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IQuestionBankDbContext>(provider => provider.GetService<QuestionBankDbContext>());

    }
}
