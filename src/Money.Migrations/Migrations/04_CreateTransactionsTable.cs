using FluentMigrator;

namespace Money.Migrations.Migrations;

[Migration(4)]
public class CreateTransactionsTable : Migration
{
    const string tableName = "transactions";
    public override void Up()
    {
        Create.Table(tableName)
            .WithColumn("id").AsGuid().NotNullable().PrimaryKey()
            .WithColumn("transaction_type_id").AsByte().NotNullable().ForeignKey("transaction_type", "id")
            .WithColumn("participant_id").AsGuid().NotNullable().ForeignKey("participants", "id")
            .WithColumn("amount").AsDecimal(8, 2).NotNullable()
            .WithColumn("created_at").AsDateTime().NotNullable();
    }

    public override void Down() => Delete.Table(tableName);
}