using Bogus;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using TransferHub.Infrastructure;
using TransferHub.IntegrationTests.ApiTests;

namespace TransferHub.IntegrationTests;

public class BaseIntegrationTests
{
    public Faker Faker;
    public IServiceProvider ServiceProvider;
    public TransferHubContext ContextForAsserts;
    public TransferHubContext Context;

    private IServiceScope _databaseScope;
    private IServiceScope _apiScope;

    protected TestApplication TestApi;

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        TestApi = new TestApplication();
        ServiceProvider = TestApi.Services;
        Faker = new Faker();

        ValidatorOptions.Global.LanguageManager.Enabled = false;
    }

    [SetUp]
    public async Task SetUpBase()
    {
        _apiScope = ServiceProvider.CreateScope();
        _databaseScope = ServiceProvider.CreateScope();
        Context = _apiScope.ServiceProvider.GetRequiredService<TransferHubContext>();
        ContextForAsserts = _databaseScope.ServiceProvider.GetService<TransferHubContext>()!;
        await DatabaseFixture.ResetDatabase();

        AssertionOptions.AssertEquivalencyUsing(options =>
        {
            options.Using<DateTime>(ctx => ctx.Subject.Should().BeCloseTo(ctx.Expectation, TimeSpan.FromMilliseconds(100))).WhenTypeIs<DateTime>();
            options.Using<DateTimeOffset>(ctx => ctx.Subject.Should().BeCloseTo(ctx.Expectation, TimeSpan.FromMilliseconds(100))).WhenTypeIs<DateTimeOffset>();

            return options;
        });
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        Context?.Dispose();
        ContextForAsserts?.Dispose();
        TestApi?.Dispose();
        _databaseScope?.Dispose();
        _apiScope?.Dispose();
    }
}
