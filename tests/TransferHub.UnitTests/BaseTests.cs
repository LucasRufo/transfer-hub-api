using Autofac.Extras.FakeItEasy;
using Bogus;
using FluentValidation;

namespace TransferHub.UnitTests;

public class BaseTests
{
    protected Faker Faker { get; private set; }
    protected AutoFake AutoFake { get; private set; }

    [OneTimeSetUp]
    public void AllTestsSetUp()
    {
        Faker = new();
        AutoFake = new();

        ValidatorOptions.Global.LanguageManager.Enabled = false;

        AssertionOptions.AssertEquivalencyUsing(options =>
        {
            options
                .Using<DateTime>(ctx => ctx.Subject.ToLocalTime().Should().BeCloseTo(ctx.Expectation.ToLocalTime(), new TimeSpan(10000)))
                .WhenTypeIs<DateTime>();

            return options;
        });
    }
}
