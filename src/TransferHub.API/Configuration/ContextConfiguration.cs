using Microsoft.EntityFrameworkCore;
using TransferHub.Infrastructure;

namespace TransferHub.API.Configuration;

public static class ContextConfiguration
{
    public static void AddTransferHubDbContext(this WebApplicationBuilder builder, IConfiguration configuration)
    {
        builder.Services.AddDbContext<TransferHubContext>((dbBuilder) =>
        {
            dbBuilder.UseNpgsql(configuration.GetConnectionString("DefaultConnection"),
                    options => options.CommandTimeout(30).EnableRetryOnFailure());
        });
    }
}
