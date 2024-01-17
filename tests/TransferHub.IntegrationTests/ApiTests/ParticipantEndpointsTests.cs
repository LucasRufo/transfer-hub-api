using TransferHub.API.Endpoints.Shared;
using TransferHub.Domain.Entities;
using TransferHub.Domain.Responses;
using TransferHub.TestsShared.Builders.Domain.Requests;
using System.Net;
using System.Net.Http.Json;

namespace TransferHub.IntegrationTests.ApiTests;

public class ParticipantEndpointsTests : BaseIntegrationTests
{
    private HttpClient _httpClient;
    private const string _baseUri = "/api/v1/participants";

    [SetUp]
    public void SetUp()
    {
        _httpClient = TestApi.CreateClient();
    }

    [Test]
    public async Task CreateShouldReturnSuccessAndParticipant()
    {
        var createParticipantRequest = new CreateParticipantRequestBuilder().Generate();

        var response = await _httpClient.PostAsync(_baseUri, createParticipantRequest.ToJsonContent());

        var participantFromDb = ContextForAsserts.Participant.First();

        var participant = new CreateParticipantResponse(participantFromDb);

        var participantFromResponse = await response.Content.ReadFromJsonAsync<CreateParticipantResponse>();

        response.Should().HaveStatusCode(HttpStatusCode.Created);
        participantFromResponse.Should().BeEquivalentTo(participant);
    }

    [Test]
    public async Task CreateShouldReturnBadRequestWhenRequestIsInvalid()
    {
        var createParticipantRequest = new CreateParticipantRequestBuilder()
            .WithName("")
            .Generate();

        var response = await _httpClient.PostAsync(_baseUri, createParticipantRequest.ToJsonContent());

        var problemDetails = await response.Content.ReadFromJsonAsync<CustomProblemDetails>();

        var customProblemDetailsExpected = CustomProblemDetails.CreateValidationProblemDetails(
            _baseUri,
            new List<CustomProblemDetailsError>()
            {
                new("Name", "'Name' must not be empty.")
            });

        response.Should().HaveStatusCode(HttpStatusCode.BadRequest);
        //still need to find a way to apply the exclude globally for this project
        problemDetails.Should().BeEquivalentTo(customProblemDetailsExpected, ctx => ctx.Excluding(p => p.Type));
    }

    [Test]
    public async Task CreateShouldReturnUnprocessableEntityWhenServiceFails()
    {
        var createParticipantRequest = new CreateParticipantRequestBuilder().Generate();

        new ParticipantBuilder()
            .WithCPF(createParticipantRequest.CPF)
            .GenerateInDatabase(Context);

        var response = await _httpClient.PostAsync(_baseUri, createParticipantRequest.ToJsonContent());

        var problemDetails = await response.Content.ReadFromJsonAsync<CustomProblemDetails>();

        var error = Error.Failure("ParticipantAlreadyExists", "A participant with this CPF already exists.");

        var customProblemDetailsExpected = CustomProblemDetails.CreateDomainProblemDetails(
            HttpStatusCode.UnprocessableEntity,
            _baseUri,
            error);

        response.Should().HaveStatusCode(HttpStatusCode.UnprocessableEntity);
        problemDetails.Should().BeEquivalentTo(customProblemDetailsExpected, ctx => ctx.Excluding(p => p.Type));
    }

    [Test]
    public async Task StatementShouldReturnSuccessAndStatement()
    {
        var participant = new ParticipantBuilder().GenerateInDatabase(Context);

        var transactions = new TransactionBuilder()
            .WithParticipantId(participant.Id)
            .GenerateInDatabase(Context, 5);

        var url = $"{_baseUri}/{participant.Id}/statement?page=1&pageSize=20";

        var response = await _httpClient.GetAsync(url);

        var statementTransactions = new StatementTransactionResponse().Convert(transactions);

        var statementToCompare = new StatementResponse(
            1,
            20,
            participant.Id,
            participant.Name,
            participant.Balance,
            statementTransactions);

        var statement = await response.Content.ReadFromJsonAsync<StatementResponse>();

        response.Should().HaveStatusCode(HttpStatusCode.OK);
        statement.Should().BeEquivalentTo(statementToCompare);
    }

    [Test]
    public async Task StatementShouldReturnNotFoundWhenParticipantIsNotFound()
    {
        var fakeParticipantId = Guid.NewGuid();

        var url = $"{_baseUri}/{fakeParticipantId}/statement";

        var response = await _httpClient.GetAsync(url);

        var problemDetails = await response.Content.ReadFromJsonAsync<CustomProblemDetails>();

        var error = Error.Failure("ParticipantNotFound", $"The participant with Id {fakeParticipantId} was not found.");

        var customProblemDetailsExpected = CustomProblemDetails.CreateDomainProblemDetails(
            HttpStatusCode.NotFound,
            url,
            error);

        response.Should().HaveStatusCode(HttpStatusCode.NotFound);
        problemDetails.Should().BeEquivalentTo(customProblemDetailsExpected, ctx => ctx.Excluding(p => p.Type));
    }
}
