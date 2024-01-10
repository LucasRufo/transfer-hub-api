using FluentValidation;
using Money.API.Endpoints.Shared;
using Money.API.Extensions;
using Money.API.Requests;
using Money.Domain.Services;
using System.Net;

namespace Money.API.Endpoints;

public static class ParticipantEndpoints
{
    public static void AddParticipantEndpoints(this WebApplication app)
    {
        var participants = app.NewVersionedApi("Participants");

        var participantsV1 = participants
            .MapGroup("/api/v{version:apiVersion}")
            .HasApiVersion(1);

        participantsV1.MapPost("/participants", async (CreateParticipantRequest request, IValidator<CreateParticipantRequest> validator, ParticipantService participantService, HttpContext context) =>
        {
            var validationResult = await validator.ValidateAsync(request);

            if (!validationResult.IsValid)
                return Results.BadRequest(CustomProblemDetails.CreateValidationProblemDetails(context.Request.Path, validationResult.ToCustomProblemDetailsError()));

            var createParticipantResult = await participantService.Create(request);

            if (createParticipantResult.IsError)
                return Results.UnprocessableEntity(CustomProblemDetails.CreateDomainProblemDetails(HttpStatusCode.UnprocessableEntity, context.Request.Path, createParticipantResult.FirstError));

            return Results.Created($"/api/v1/participants/{createParticipantResult.Value.Id}", createParticipantResult.Value);
        });
    }
}
