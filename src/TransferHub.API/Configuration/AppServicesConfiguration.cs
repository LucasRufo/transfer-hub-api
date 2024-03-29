﻿using FluentValidation;
using TransferHub.Domain.Providers.Datetime;
using TransferHub.Domain.Repositories;
using TransferHub.Domain.Services;
using TransferHub.Domain.Validators;
using TransferHub.Infrastructure.Repositories;

namespace TransferHub.API.Configuration;

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
        services.AddScoped<ITransactionRepository, TransactionRepository>();

        return services;
    }

    private static IServiceCollection AddValidators(this IServiceCollection services)
    {
        ValidatorOptions.Global.LanguageManager.Enabled = false;

        services.AddValidatorsFromAssemblyContaining<CreateParticipantRequestValidator>();

        return services;
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<ParticipantService>();
        services.AddScoped<TransactionService>();

        return services;
    }

    private static IServiceCollection AddProviders(this IServiceCollection services)
    {
        services.AddScoped<IDateTimeProvider, DateTimeProvider>();

        return services;
    }
}
