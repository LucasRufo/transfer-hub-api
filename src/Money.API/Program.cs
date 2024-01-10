using Money.API.Configuration;
using Money.API.Endpoints;

var builder = WebApplication.CreateBuilder(args);

ConfigureServices();

var app = builder.Build();

ConfigureApp();

app.Run();

void ConfigureServices()
{
    builder.AddMoneyDbContext(builder.Configuration);
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddApplicationServices();
    builder.Services.AddHealthCheck(builder.Configuration);
}

void ConfigureApp()
{
    app.UseHealthCheck();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.AddParticipantEndpoints();
}

public partial class Program { }
