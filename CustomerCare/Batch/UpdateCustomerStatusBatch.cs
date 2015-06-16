namespace CustomerCare.Batch
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Raven.Client;

    public class UpdateCustomerStatusBatch
    {
        public IDocumentStore Store { get; set; }
        public void Run()
        {
           
            IEnumerable<Customer> customers;

            using(var session = Store.OpenSession())
                customers = session.Query<Customer>().ToList();

            foreach(var customer in customers)
                UpdateStatusFor(customer.Id);
        }

        void UpdateStatusFor(string customerId)
        {
            using (var session = Store.OpenSession())
            {
                var customer = session.Load<Customer>(customerId);

                var ordersWithinTheLastYear = customer.Orders
                    .Where(o=>(DateTime.UtcNow -  o.OrderDate) < TimeSpan.FromDays(365))
                    .Sum(order => order.OrderValue);

                customer.Prefered = ordersWithinTheLastYear > 5000;

                session.SaveChanges();
            }
                
        }
    }
}