using System.CommandLine;
using System.Reflection;

namespace Money.Migrations;

internal class Migrator
{
    internal static readonly string[] connectionStringAliases = ["--connectionString", "-s"];

    internal static async Task<int> ExecuteAsync(string[] args, Assembly assembly)
    {
        var connectionStringOption = new Option<string>(connectionStringAliases, "Database connection string.")
        {
            IsRequired = true,
            Arity = ArgumentArity.ExactlyOne,
        };

        var rootCommand = new RootCommand("Migrator service for the Money REST Api.");

        var upCommnad = new Command("up", "Execute migrations up.") { connectionStringOption };

        rootCommand.AddCommand(upCommnad);

        upCommnad.SetHandler((connectionString) =>
        {
            var migrationService = new MigrationService(connectionString);

            migrationService.Up(assembly);
        }, 
        connectionStringOption);

        return await rootCommand.InvokeAsync(args);
    }
}
