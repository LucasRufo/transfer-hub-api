using Money.Migrations;

await Migrator.ExecuteAsync(args, typeof(Migrator).Assembly);