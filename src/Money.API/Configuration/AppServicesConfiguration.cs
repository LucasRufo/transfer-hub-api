using Money.Domain.Repositories;
using Money.Infrastructure.Repositories;

namespace Money.API.Configuration;

public static class AppServicesConfiguration
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IParticipantRepository, ParticipantRepository>();

        return services;
    }
}
