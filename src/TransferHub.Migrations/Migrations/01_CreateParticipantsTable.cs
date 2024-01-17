using FluentMigrator;

namespace TransferHub.Migrations.Migrations;

[Migration(1)]
public class CreateParticipantsTable : Migration
{
    const string tableName = "participants";
    public override void Up()
    {
        Create.Table(tableName)
            .WithColumn("id").AsGuid().NotNullable().PrimaryKey()
            .WithColumn("name").AsString(200).NotNullable()
            .WithColumn("cpf").AsString(20).Unique().NotNullable()
            .WithColumn("balance").AsDecimal(10, 2).NotNullable()
            .WithColumn("created_at").AsDateTime().NotNullable()
            .WithColumn("updated_at").AsDateTime().Nullable();
    }

    public override void Down() => Delete.Table(tableName);
}
