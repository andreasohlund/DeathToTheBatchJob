namespace CustomerCare
{
    using System;
    using Contracts;
    using Sales.Contracts;
    using NServiceBus.Saga;

    public class PreferedCustomerPolicy : Saga<PreferedCustomerPolicyData>,
        IAmStartedByMessages<OrderAccepted>,
        IHandleTimeouts<OrderAccepted>
    {
        public void Handle(OrderAccepted message)
        {
            Data.CustomerId = message.CustomerId;

            AdjustRunningTotal(message.OrderValue);

            RequestUtcTimeout(message.OrderDate.AddSeconds(20), message);
        }

        void AdjustRunningTotal(double orderValue)
        {
            Data.YearlyRunningTotal += orderValue;
            if (Data.YearlyRunningTotal > 5000)
            {
                if (!Data.IsPrefered)
                    Bus.Publish<CustomerMadePrefered>(m => m.CustomerId = Data.CustomerId);

                Data.IsPrefered = true;
            }
            else
            {
                if (Data.IsPrefered)
                    Bus.Publish<CustomerDegradedToRegularStatus>(m => m.CustomerId = Data.CustomerId);

                Data.IsPrefered = false;
            }
        }

        public void Timeout(OrderAccepted state)
        {
            AdjustRunningTotal(-state.OrderValue);
        }

        public override void ConfigureHowToFindSaga()
        {
            ConfigureMapping<OrderAccepted>(s => s.CustomerId, m => m.CustomerId);
        }
    }

    public class PreferedCustomerPolicyData : IContainSagaData
    {
        public Guid Id { get; set; }
        public string Originator { get; set; }
        public string OriginalMessageId { get; set; }

        public Guid CustomerId { get; set; }

        public double YearlyRunningTotal { get; set; }

        public bool IsPrefered { get; set; }
    }
}