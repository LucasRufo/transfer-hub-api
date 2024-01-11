using FluentValidation;
using Money.API.Endpoints.Shared;
using Money.API.Extensions;
using Money.Domain.Requests;

namespace Money.API.Endpoints;

public static class TransactionEndpoints
{
    public static void AddTransactionEndpoints(this WebApplication app)
    {
        var transactions = app.NewVersionedApi("Transactions");

        var transactionsV1 = transactions
            .MapGroup("/api/v{version:apiVersion}")
            .HasApiVersion(1);

        transactionsV1.MapPost("/transactions/credit", async (CreateCreditTransactionRequest request, IValidator<CreateCreditTransactionRequest> validator, HttpContext context) =>
        {
            var validationResult = await validator.ValidateAsync(request);

            if (!validationResult.IsValid)
                return Results.BadRequest(CustomProblemDetails.CreateValidationProblemDetails(context.Request.Path, validationResult.ToCustomProblemDetailsError()));

            return Results.Ok();
        });
    }
}


