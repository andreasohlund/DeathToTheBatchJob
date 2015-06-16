namespace CustomerCare.Batch
{
    using System;
    using System.Collections.Generic;

    public class Customer
    {
        public string Id { get; set; }

        public List<Order> Orders
        {
            get { return orders ?? (orders = new List<Order>()); }
            set { orders = value; }
        }
        List<Order> orders; 
        public bool Prefered { get; set; }
    }
}