namespace CustomerCare.Batch
{
    using System;
    using System.Linq;
    using Sales.Contracts;
    using NServiceBus;
    using NServiceBus.Config;
    using Raven.Client;
    using Raven.Client.Document;
    using Raven.Client.Linq;

    public class Runner : IWantToRunAtStartup
    {
        public UpdateCustomerStatusBatch Batch { get; set; }


        public void Run()
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
                        GenerateTestCustomers();
                        break;

                    case "b":
                        Batch.Run();
                        ListCustomerStatus();
                        break;


                    case "o":
                        GenerateTestCustomers(1);
                        break;
                }
            }
            

        }

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
                                     OrderDate = DateTime.UtcNow.AddDays(-random.Next(0, 600))
                                     
                                 });

                }
            }

        }


    }

    public class ConfigureDeps : INeedInitialization
    {
        public void Init()
        {
            var store = new DocumentStore { Url = "http://localhost:8080" };

            store.Initialize();

            Configure.Instance.Configurer.RegisterSingleton<IDocumentStore>(store);
            Configure.Instance.Configurer
                .ConfigureComponent<UpdateCustomerStatusBatch>(DependencyLifecycle.InstancePerCall);
        }

    }
}