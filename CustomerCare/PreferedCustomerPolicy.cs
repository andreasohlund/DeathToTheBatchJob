namespace CustomerCare
{
    using System;
    using Contracts;
    using NServiceBus.Saga;
    using Sales.Contracts;

    public class PreferedCustomerPolicy : Saga<PreferedCustomerPolicyData>,
        IAmStartedByMessages<OrderAccepted>,
        IHandleTimeouts<OrderAccepted>
    {
        public void Handle(OrderAccepted message)
        {
            Data.CustomerId = message.CustomerId;

            AdjustRunningTotal(message.OrderValue);

            RequestTimeout(message.OrderDate.AddSeconds(20), message);
        }

        void AdjustRunningTotal(double orderValue)
        {
            Data.YearlyRunningTotal += orderValue;
            if (Data.YearlyRunningTotal > 5000)
            {
                if (!Data.IsPrefered)
                {
                    Bus.Publish<CustomerMadePrefered>(m => m.CustomerId = Data.CustomerId);
                    Bus.SendLocal<CustomerMadePrefered>(m => m.CustomerId = Data.CustomerId);
                }

                Data.IsPrefered = true;
            }
            else
            {
                if (Data.IsPrefered)
                {
                    Bus.Publish<CustomerDegradedToRegularStatus>(m => m.CustomerId = Data.CustomerId);
                    Bus.SendLocal<CustomerDegradedToRegularStatus>(m => m.CustomerId = Data.CustomerId);
                }

                Data.IsPrefered = false;
            }
        }

        public void Timeout(OrderAccepted state)
        {
            AdjustRunningTotal(-state.OrderValue);
        }

        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<PreferedCustomerPolicyData> mapper)
        {
            mapper.ConfigureMapping<OrderAccepted>(m => m.CustomerId).ToSaga(s => s.CustomerId);
        }
    }

    public class PreferedCustomerPolicyData : IContainSagaData
    {
        public Guid Id { get; set; }
        public string Originator { get; set; }
        public string OriginalMessageId { get; set; }

        [Unique]
        public Guid CustomerId { get; set; }

        public double YearlyRunningTotal { get; set; }

        public bool IsPrefered { get; set; }
    }
}