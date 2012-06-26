namespace CustomerCare.Batch
{
    using System;

    public class Order
    {
        public Guid Id { get; set; }
        
        public double OrderValue { get; set; }

        public DateTime OrderDate { get; set; }
    }
}