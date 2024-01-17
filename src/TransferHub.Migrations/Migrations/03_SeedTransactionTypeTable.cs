using FluentMigrator;

namespace TransferHub.Migrations.Migrations;

[Migration(3)]
public class SeedTransactionTypeTable : Migration
{
    const string tableName = "transaction_type";
    public override void Up()
    {
        Insert.IntoTable(tableName)
            .Row(new
            {
                id = 0,
                name = "Credit"
            })
            .Row(new
            {
                id = 1,
                name = "Debit"
            });
    }

    public override void Down() => Delete.FromTable(tableName).AllRows();
}

