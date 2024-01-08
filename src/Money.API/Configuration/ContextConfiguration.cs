using Microsoft.EntityFrameworkCore;
using Money.Infrastructure;

namespace Money.API.Configuration;

public static class ContextConfiguration
{
    public static void AddMoneyDbContext(this WebApplicationBuilder builder, IConfiguration configuration)
    {
        builder.Services.AddDbContext<MoneyContext>((dbBuilder) =>
        {
            dbBuilder.UseNpgsql(configuration.GetConnectionString("DefaultConnection"),
                    options => options.CommandTimeout(30).EnableRetryOnFailure());
        });
    }
}
