using Raven.Client.Extensions;

namespace CustomerCare.Batch
{
    using System;
    using System.Linq;
    using NServiceBus;
    using Raven.Client;
    using Raven.Client.Document;
    using Raven.Client.Linq;
    using Sales.Contracts;

    public class Runner : IWantToRunWhenBusStartsAndStops
    {
        public UpdateCustomerStatusBatch Batch { get; set; }

        void ListCustomerStatus()
        {
            using (var session = Store.OpenSession())
            {
                var preferedCustomers = session.Query<Customer>().Where(c => c.Prefered).ToList();

                Console.WriteLine("NumberOfPreferedCustomers: " + preferedCustomers.Count());
                Console.WriteLine("Our top five customers is: ");

                foreach (var preferedCustomer in preferedCustomers.OrderByDescending(c => c.Orders.Sum(o => o.OrderValue)).Take(5))
                {
                    Console.WriteLine(preferedCustomer.Id + " orderTotal: " + preferedCustomer.Orders.Sum(o => o.OrderValue));

                }
            }

        }

        public void Start()
        {
            string cmd;
            Console.WriteLine("Hit to S simulate orders");
            Console.WriteLine("Hit to B to run the batch");
            Console.WriteLine("Hit to O to create one order");

            while ((cmd = Console.ReadKey().Key.ToString().ToLower()) != "q")
            {
                switch(cmd.ToLower())
                {
                    case "s":
                        this.GenerateTestCustomers();
                        break;

                    case "b":
                        this.Batch.Run();
                        this.ListCustomerStatus();
                        break;


                    case "o":
                        this.GenerateTestCustomers(1);
                        break;
                }
            }
        }

        public void Stop()
        {
        }

        public IBus Bus { get; set; }
        public IDocumentStore Store { get; set; }
        public void GenerateTestCustomers(int numberOfCustomers = 0)
        {
            var random = new Random();
            if (numberOfCustomers == 0)
                numberOfCustomers = random.Next(25, 50);

            for (int i = 0; i < numberOfCustomers; i++)
            {
                var customerId = Guid.NewGuid();

                for (int j = 0; j < random.Next(1, 10); j++)
                {
                    Bus.SendLocal(new PlaceOrder
                                 {
                                     OrderId = Guid.NewGuid(),
                                     CustomerId = customerId,
                                     OrderValue = random.Next(100, 5000),
                                     OrderDate = DateTime.UtcNow
                                     
                                 });

                }
            }

        }
    }

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