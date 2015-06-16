namespace CustomerCare.Tests
{
    using System;
    using Sales.Contracts;

    public class Order
    {
        readonly string customerId;
        public Order(string customerId)
        {
            this.customerId = customerId;
        }

        public static Order For(string customerid)
        {
            return new Order(customerid);
        }

        public OrderAccepted WithValue(double value)
        {
            return new OrderAccepted
                       {
                           CustomerId = customerId,
                           OrderId = Guid.NewGuid(),
                           OrderDate = DateTime.UtcNow,
                           OrderValue = value
                       };
        }
    }
}