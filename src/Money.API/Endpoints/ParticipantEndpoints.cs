namespace Money.API.Endpoints;

public static class ParticipantEndpoints
{
    public static void AddParticipantEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api").WithOpenApi();

        group.MapPost("/participants", () => 
        {
            return Results.Ok();
        });
    }
}
