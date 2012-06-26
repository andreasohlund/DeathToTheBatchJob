namespace CustomerCare.Tests
{
    using System;
    using Sales.Contracts;

    public class Order
    {
        readonly Guid customerId;
        public Order(Guid customerId)
        {
            this.customerId = customerId;
        }

        public static Order For(Guid customerid)
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