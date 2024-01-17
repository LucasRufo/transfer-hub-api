using FluentValidation;
using TransferHub.API.Endpoints.Shared;
using TransferHub.API.Extensions;
using TransferHub.Domain.Requests;
using TransferHub.Domain.Responses;
using TransferHub.Domain.Services;
using System.Net;

namespace TransferHub.API.Endpoints;

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

            return Results.Created($"/api/v1/participants/{createParticipantResult.Value.Id}", new CreateParticipantResponse(createParticipantResult.Value));
        });

        participantsV1.MapGet("/participants/{participantId:guid}/statement", async (Guid participantId, int? page, int? pageSize, ParticipantService participantService, HttpContext context) =>
        {
            page ??= 1;

            if (pageSize is null || pageSize > 20)
                pageSize = 20;

            var statementResult = await participantService.Statement(participantId, page.Value, pageSize.Value);

            if (statementResult.IsError)
                return Results.NotFound(CustomProblemDetails.CreateDomainProblemDetails(HttpStatusCode.NotFound, context.Request.Path, statementResult.FirstError));

            return Results.Ok(statementResult.Value);
        });
    }
}
