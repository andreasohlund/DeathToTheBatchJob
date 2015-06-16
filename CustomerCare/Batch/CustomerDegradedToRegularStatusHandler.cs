namespace CustomerCare.Batch
{
    using System;
    using Contracts;
    using NServiceBus;

    public class CustomerDegradedToRegularStatusHandler : IHandleMessages<CustomerDegradedToRegularStatus>
    {
        public void Handle(CustomerDegradedToRegularStatus message)
        {
            using (new ColoredConsole(ConsoleColor.Yellow))
            {
                Console.Out.WriteLine("Customer {0} is now a regular customer", message.CustomerId);
            }
        }
    }
}