namespace Sales.Contracts
{
    using System;

    public class PlaceOrder
    {
        public Guid OrderId { get; set; }

        public string CustomerId { get; set; }

        public double OrderValue { get; set; }

        public DateTime OrderDate { get; set; }
    }

    public class OrderAccepted
    {
        public Guid OrderId { get; set; }

        public string CustomerId { get; set; }

        public double OrderValue { get; set; }

        public DateTime OrderDate { get; set; }
    }
}