namespace Money.API.Configuration;

public static class HealthCheckConfiguration
{
    public static void AddHealthCheck(this IServiceCollection services, IConfiguration configuration)
        => services.AddHealthChecks().AddNpgSql(configuration.GetConnectionString("DefaultConnection")!);

    public static void UseHealthCheck(this WebApplication app)
        => app.MapHealthChecks("/health");
}
