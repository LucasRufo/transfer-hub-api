using Autofac.Extras.FakeItEasy;
using Bogus;

namespace Money.UnitTests;

public class BaseTests
{
    protected Faker Faker { get; private set; }
    protected AutoFake AutoFake { get; private set; }

    [OneTimeSetUp]
    public void AllTestsSetUp()
    {
        Faker = new();
        AutoFake = new();

        AssertionOptions.AssertEquivalencyUsing(options =>
        {
            options
                .Using<DateTime>(ctx => ctx.Subject.ToLocalTime().Should().BeCloseTo(ctx.Expectation.ToLocalTime(), new TimeSpan(10000)))
                .WhenTypeIs<DateTime>();

            return options;
        });
    }
}
