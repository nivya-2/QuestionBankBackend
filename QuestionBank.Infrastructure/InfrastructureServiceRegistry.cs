using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QuestionBank.Application.Contracts.Persistence;
using Microsoft.EntityFrameworkCore;

namespace QuestionBank.Infrastructure;

public static class InfrastructureServiceRegistry
{
    public static void AddInfrastructureServices(this IServiceCollection services,
    IConfiguration configuration)
    {
        services.AddEntityFrameworkNpgsql().AddDbContext<QuestionBankDbContext>(options =>
        options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IQuestionBankDbContext>(provider => provider.GetService<QuestionBankDbContext>());
    }
}
