using FluentMigrator;

namespace TransferHub.Migrations.Migrations;

[Migration(2)]
public class CreateTransactionTypeTable : Migration
{
    const string tableName = "transaction_type";
    public override void Up()
    {
        Create.Table("transaction_type")
            .WithColumn("id").AsByte().PrimaryKey()
            .WithColumn("name").AsString(20).NotNullable();
    }

    public override void Down() => Delete.Table(tableName);
}
