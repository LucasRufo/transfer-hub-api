using FluentMigrator;

namespace Money.Migrations.Migrations;

[Migration(1)]
public class CreateParticipantsTable : Migration
{
    const string tableName = "participants";
    public override void Up()
    {
        Create.Table(tableName)
            .WithColumn("id").AsGuid().NotNullable().PrimaryKey()
            .WithColumn("name").AsString().NotNullable()
            .WithColumn("cpf").AsString().NotNullable()
            .WithColumn("created_at").AsDateTime().NotNullable()
            .WithColumn("updated_at").AsDateTime().Nullable();
    }

    public override void Down() => Delete.Table(tableName);
}
