using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Npgsql;
using Dapper;

namespace Money.Migrations;

internal class MigrationService
{
    private readonly string _connectionString;

    public MigrationService(string connectionString)
    {
        if (string.IsNullOrEmpty(connectionString))
            throw new ArgumentNullException(nameof(connectionString));

        _connectionString = connectionString;
    }

    private void CreateDatabaseIfItDoesNotExist()
    {
        var connectionStringBuilder = new NpgsqlConnectionStringBuilder(_connectionString);

        var database = connectionStringBuilder.Database;

        connectionStringBuilder.Database = "template1";

        using var connection = new NpgsqlConnection(connectionStringBuilder.ToString());

        var databaseAlreadyExists = connection.QuerySingle<bool>($@"SELECT EXISTS (SELECT datname FROM pg_catalog.pg_database WHERE datname = '{database}')");

        if (!databaseAlreadyExists)
            connection.Execute($@"CREATE DATABASE {database};");
    }

    public void Up(Assembly assembly) => Execute((executor) => executor.MigrateUp(), assembly);

    private void Execute(Action<IMigrationRunner> action, Assembly assembly)
    {
        CreateDatabaseIfItDoesNotExist();

        var serviceProvider = new ServiceCollection()
            .AddFluentMigratorCore()
            .ConfigureRunner(
                runnerBuilder =>
                {
                    runnerBuilder
                        .AddPostgres()
                        .WithGlobalConnectionString(_connectionString)
                        .ScanIn(assembly).For.Migrations();
                })
            .AddLogging(lb => lb.AddFluentMigratorConsole());

        var executor = serviceProvider.BuildServiceProvider().GetRequiredService<IMigrationRunner>();

        action(executor);
    }
}
