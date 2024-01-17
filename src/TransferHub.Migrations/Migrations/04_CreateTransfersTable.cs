using FluentMigrator;

namespace TransferHub.Migrations.Migrations;

[Migration(4)]
public class CreateTransfersTable : Migration
{
    const string tableName = "transfers";
    public override void Up()
    {
        Create.Table(tableName)
            .WithColumn("id").AsGuid().NotNullable().PrimaryKey()
            .WithColumn("from_participant_id").AsGuid().NotNullable().ForeignKey("participants", "id")
            .WithColumn("to_participant_id").AsGuid().NotNullable().ForeignKey("participants", "id")
            .WithColumn("created_at").AsDateTime().NotNullable();
    }

    public override void Down() => Delete.Table(tableName);
}
