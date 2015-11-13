using NServiceBus;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Extensions;

namespace CustomerCare.Batch
{
    public class ConfigureDeps : INeedInitialization
    {
        public void Customize(BusConfiguration configuration)
        {
            var store = new DocumentStore { Url = "http://localhost:8080", DefaultDatabase = "Customers" };
            store.Initialize();
            store.DatabaseCommands.EnsureDatabaseExists("Customers");

            configuration.RegisterComponents(c => c.RegisterSingleton<IDocumentStore>(store));
            configuration.RegisterComponents(c => c.ConfigureComponent<UpdateCustomerStatusBatch>(DependencyLifecycle.InstancePerCall));
        }
    }
}