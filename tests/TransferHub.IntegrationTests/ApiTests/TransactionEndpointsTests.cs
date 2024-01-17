using TransferHub.API.Endpoints.Shared;
using TransferHub.Domain.Entities;
using TransferHub.Domain.Responses;
using TransferHub.TestsShared.Builders.Domain.Requests;
using TransferHub.TestsShared.Builders.Domain.Responses;
using System.Net;
using System.Net.Http.Json;

namespace TransferHub.IntegrationTests.ApiTests;
public class TransactionEndpointsTests : BaseIntegrationTests
{
    private HttpClient _httpClient;
    private const string _baseUri = "/api/v1/transactions";

    [SetUp]
    public void SetUp()
    {
        _httpClient = TestApi.CreateClient();
    }

    [Test]
    public async Task CreditShouldReturnSuccessAndTransaction()
    {
        var participant = new ParticipantBuilder().GenerateInDatabase(Context);

        var createCreditTransactionRequest = new CreateCreditTransactionRequestBuilder()
            .WithParticipantId(participant.Id)
            .Generate();

        var response = await _httpClient.PostAsync($"{_baseUri}/credit", createCreditTransactionRequest.ToJsonContent());

        var transactionFromDb = ContextForAsserts.Transaction.First();

        var transaction = new CreateCreditTransactionResponse(transactionFromDb);

        var transactionResponse = await response.Content.ReadFromJsonAsync<CreateCreditTransactionResponse>();

        response.Should().HaveStatusCode(HttpStatusCode.OK);
        transactionResponse.Should().BeEquivalentTo(transaction);
    }

    [Test]
    public async Task CreditShouldReturnBadRequestWhenRequestIsInvalid()
    {
        var createCreditTransactionRequest = new CreateCreditTransactionRequestBuilder()
            .WithAmount(0)
            .Generate();

        var response = await _httpClient.PostAsync($"{_baseUri}/credit", createCreditTransactionRequest.ToJsonContent());

        var problemDetails = await response.Content.ReadFromJsonAsync<CustomProblemDetails>();

        var customProblemDetailsExpected = CustomProblemDetails.CreateValidationProblemDetails(
            $"{_baseUri}/credit",
            new List<CustomProblemDetailsError>()
            {
                new("Amount", "'Amount' must be greater than '0'.")
            });

        response.Should().HaveStatusCode(HttpStatusCode.BadRequest);
        problemDetails.Should().BeEquivalentTo(customProblemDetailsExpected, ctx => ctx.Excluding(p => p.Type));
    }

    [Test]
    public async Task CreditShouldReturnUnprocessableEntityWhenServiceFails()
    {
        var createCreditTransactionRequest = new CreateCreditTransactionRequestBuilder().Generate();

        var response = await _httpClient.PostAsync($"{_baseUri}/credit", createCreditTransactionRequest.ToJsonContent());

        var problemDetails = await response.Content.ReadFromJsonAsync<CustomProblemDetails>();

        var error = Error.Failure("ParticipantNotFound", $"The participant with Id {createCreditTransactionRequest.ParticipantId} was not found.");

        var customProblemDetailsExpected = CustomProblemDetails.CreateDomainProblemDetails(
            HttpStatusCode.UnprocessableEntity,
            $"{_baseUri}/credit",
            error);

        response.Should().HaveStatusCode(HttpStatusCode.UnprocessableEntity);
        problemDetails.Should().BeEquivalentTo(customProblemDetailsExpected, ctx => ctx.Excluding(p => p.Type));
    }

    [Test]
    public async Task TransferShouldReturnSuccess()
    {
        var fromParticipant = new ParticipantBuilder()
            .WithBalance(10)
            .GenerateInDatabase(Context);

        var toParticipant = new ParticipantBuilder().GenerateInDatabase(Context);

        var transferRequest = new TransferRequestBuilder()
            .WithFromParticipantId(fromParticipant.Id)
            .WithToParticipantId(toParticipant.Id)
            .WithAmount(10)
            .Generate();

        var response = await _httpClient.PostAsync($"{_baseUri}/transfer", transferRequest.ToJsonContent());

        var transferFromDb = ContextForAsserts.Transfer.First();

        var transfer = new TransferResponse(transferFromDb);

        var transferResponse = await response.Content.ReadFromJsonAsync<TransferResponse>();

        response.Should().HaveStatusCode(HttpStatusCode.OK);
        transferResponse.Should().BeEquivalentTo(transfer);
    }

    [Test]
    public async Task TransferShouldReturnBadRequestWhenRequestIsInvalid()
    {
        var transferRequest = new TransferRequestBuilder()
            .WithAmount(0)
            .Generate();

        var response = await _httpClient.PostAsync($"{_baseUri}/transfer", transferRequest.ToJsonContent());

        var problemDetails = await response.Content.ReadFromJsonAsync<CustomProblemDetails>();

        var customProblemDetailsExpected = CustomProblemDetails.CreateValidationProblemDetails(
            $"{_baseUri}/transfer",
            new List<CustomProblemDetailsError>()
            {
                new("Amount", "'Amount' must be greater than '0'.")
            });

        response.Should().HaveStatusCode(HttpStatusCode.BadRequest);
        problemDetails.Should().BeEquivalentTo(customProblemDetailsExpected, ctx => ctx.Excluding(p => p.Type));
    }

    [Test]
    public async Task TransferShouldReturnUnprocessableEntityWhenServiceFails()
    {
        var transferRequest = new TransferRequestBuilder().Generate();

        var response = await _httpClient.PostAsync($"{_baseUri}/transfer", transferRequest.ToJsonContent());

        var problemDetails = await response.Content.ReadFromJsonAsync<CustomProblemDetails>();

        var error = Error.Failure("ParticipantNotFound", $"The participant with Id {transferRequest.FromParticipantId} was not found.");

        var customProblemDetailsExpected = CustomProblemDetails.CreateDomainProblemDetails(
            HttpStatusCode.UnprocessableEntity,
            $"{_baseUri}/transfer",
            error);

        response.Should().HaveStatusCode(HttpStatusCode.UnprocessableEntity);
        problemDetails.Should().BeEquivalentTo(customProblemDetailsExpected, ctx => ctx.Excluding(p => p.Type));
    }
}
