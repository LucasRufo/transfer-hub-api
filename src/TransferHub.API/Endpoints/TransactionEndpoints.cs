using FluentValidation;
using TransferHub.API.Endpoints.Shared;
using TransferHub.API.Extensions;
using TransferHub.Domain.Requests;
using TransferHub.Domain.Responses;
using TransferHub.Domain.Services;
using System.Net;

namespace TransferHub.API.Endpoints;

public static class TransactionEndpoints
{
    public static void AddTransactionEndpoints(this WebApplication app)
    {
        var transactions = app.NewVersionedApi("Transactions");

        var transactionsV1 = transactions
            .MapGroup("/api/v{version:apiVersion}")
            .HasApiVersion(1);

        transactionsV1.MapPost("/transactions/credit", async (CreateCreditTransactionRequest request, IValidator<CreateCreditTransactionRequest> validator, TransactionService transactionService, HttpContext context) =>
        {
            var validationResult = await validator.ValidateAsync(request);

            if (!validationResult.IsValid)
                return Results.BadRequest(CustomProblemDetails.CreateValidationProblemDetails(context.Request.Path, validationResult.ToCustomProblemDetailsError()));

            var creditTransactionResult = await transactionService.Credit(request);

            if (creditTransactionResult.IsError)
                return Results.UnprocessableEntity(CustomProblemDetails.CreateDomainProblemDetails(HttpStatusCode.UnprocessableEntity, context.Request.Path, creditTransactionResult.FirstError));

            return Results.Ok(new CreateCreditTransactionResponse(creditTransactionResult.Value));
        });

        transactionsV1.MapPost("/transactions/transfer", async (TransferRequest request, IValidator<TransferRequest> validator, TransactionService transactionService, HttpContext context) =>
        {
            var validationResult = await validator.ValidateAsync(request);

            if (!validationResult.IsValid)
                return Results.BadRequest(CustomProblemDetails.CreateValidationProblemDetails(context.Request.Path, validationResult.ToCustomProblemDetailsError()));

            var transferResult = await transactionService.Transfer(request);

            if (transferResult.IsError)
                return Results.UnprocessableEntity(CustomProblemDetails.CreateDomainProblemDetails(HttpStatusCode.UnprocessableEntity, context.Request.Path, transferResult.FirstError));

            return Results.Ok(new TransferResponse(transferResult.Value));
        });
    }
}


