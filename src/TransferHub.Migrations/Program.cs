using TransferHub.Migrations;

await Migrator.ExecuteAsync(args, typeof(Migrator).Assembly);