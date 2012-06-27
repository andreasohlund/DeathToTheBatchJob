namespace CustomerCare.Batch
{
    using System;
    using Sales.Contracts;
    using NServiceBus;
    using Raven.Client;

    public class PlaceOrderHandler : IHandleMessages<PlaceOrder>
    {
        public IDocumentStore Store { get; set; }
        public IBus Bus { get; set; }

        public void Handle(PlaceOrder message)
        {
            var orderValue = message.OrderValue;
            using (var session = Store.OpenSession())
            {
                var customer = session.Load<Customer>(message.CustomerId) ?? new Customer
                                                                                 {
                                                                                     Id = message.CustomerId
                                                                                 };

                if (customer.Prefered)
                    orderValue = orderValue*0.9;//10% discount

                customer.Orders.Add(new Batch.Order
                                        {
                                            Id = message.OrderId,
                                            OrderValue = orderValue,
                                            OrderDate = message.OrderDate
                                        });
                session.Store(customer);
                session.SaveChanges();
            }
            
            Bus.Publish<OrderAccepted>(m =>
                                           {
                                               m.CustomerId = message.CustomerId;
                                               m.OrderId = message.OrderId;
                                               m.OrderValue = orderValue;
                                               m.OrderDate = message.OrderDate;
                                           });
            Console.WriteLine("Order accepted for " + message.CustomerId + ", Ordervalue: "+message.OrderValue);
        }
    }
}