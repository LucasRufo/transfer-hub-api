using FluentValidation;
using Money.Domain.Providers.Datetime;
using Money.Domain.Repositories;
using Money.Domain.Services;
using Money.Domain.Validators;
using Money.Infrastructure.Repositories;

namespace Money.API.Configuration;

public static class AppServicesConfiguration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddRepositories();
        services.AddValidators();
        services.AddServices();
        services.AddProviders();

        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IParticipantRepository, ParticipantRepository>();

        return services;
    }

    private static IServiceCollection AddValidators(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<CreateParticipantRequestValidator>();

        ValidatorOptions.Global.LanguageManager.Culture = new System.Globalization.CultureInfo("en-US");

        return services;
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<ParticipantService>();

        return services;
    }

    private static IServiceCollection AddProviders(this IServiceCollection services)
    {
        services.AddScoped<IDateTimeProvider, DateTimeProvider>();

        return services;
    }
}
