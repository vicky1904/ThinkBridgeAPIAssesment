using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using ShopBridgeDataModel;

namespace ShopBridgeTests
{

    public class BaseSpecs
    {
        protected DataContext ShopBridgeDbContext;
        protected ILoggerFactory LoggerFactory;

        protected void SetupDbAndLogger()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "ShopBridgeInMemoryDB")
                .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;

            ShopBridgeDbContext = new DataContext(options);
            var serviceProvider = new ServiceCollection().AddLogging().BuildServiceProvider();
            LoggerFactory = serviceProvider.GetService<ILoggerFactory>();
        }
    }
}
