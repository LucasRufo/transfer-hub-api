using Autofac.Extras.FakeItEasy;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Money.UnitTests.Configuration;

public static class InMemoryContextExtensions
{
    public static AutoFake WithInMemoryContext(this AutoFake autoFake)
    {
        var options = new DbContextOptionsBuilder<DbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;

        autoFake.Provide(new DbContext(options));

        return autoFake;
    }
}
