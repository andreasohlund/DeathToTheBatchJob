namespace CustomerCare.Tests
{
    using CustomerCare.Contracts;
    using NServiceBus.Testing;
    using NUnit.Framework;
    using Sales;

    [TestFixture]
    public class When_previous_orders_expires: PreferedCustomerPolicySpecification
    {
        [Test]
        public void Should_become_a_regular_customer()
        {
            Test.Saga<PreferedCustomerPolicy>()
                .When(s => s.Handle(Order.For(customerId).WithValue(6000)))
                .WhenSagaTimesOut()
                .ExpectPublish<CustomerDegradedToRegularStatus>(m => m.CustomerId == customerId);

        }
    }
}