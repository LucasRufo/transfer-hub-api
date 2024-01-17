using TransferHub.API.Configuration;
using TransferHub.API.Configuration.Swagger;
using TransferHub.API.Endpoints;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

ConfigureServices();

var app = builder.Build();

ConfigureApp();

app.Run();

void ConfigureServices()
{
    builder.AddTransferHubDbContext(builder.Configuration);
    builder.Services.AddProblemDetails();
    builder.Services.AddSwagger();
    builder.Services.AddApplicationServices();
    builder.Services.AddHealthCheck(builder.Configuration);
    builder.Services.AddVersioning();

    builder.Services.ConfigureHttpJsonOptions(options => {
        options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    });
}

void ConfigureApp()
{
    app.UseExceptionHandler();
    var pathbase = builder.Configuration["PathBase"];
    app.UsePathBase(pathbase);
    app.UseHealthCheck();
    app.AddParticipantEndpoints();
    app.AddTransactionEndpoints();
    app.UseSwaggerCustom(pathbase ?? "");
}

public partial class Program { }
