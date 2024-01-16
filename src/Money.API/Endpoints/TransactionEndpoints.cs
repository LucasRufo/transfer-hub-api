using FluentValidation;
using Money.API.Endpoints.Shared;
using Money.API.Extensions;
using Money.Domain.Requests;
using Money.Domain.Responses;
using Money.Domain.Services;
using System.Net;

namespace Money.API.Endpoints;

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

            return Results.Ok();
        });
    }
}


