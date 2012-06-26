namespace CustomerCare.Tests
{
    using Contracts;
    using NServiceBus.Testing;
    using NUnit.Framework;
    using Sales;

    [TestFixture]
    public class When_a_prefered_customer_places_an_order : PreferedCustomerPolicySpecification
    {
        [Test]
        public void Should_not_republish_a_customer_prefered_event()
        {
            Test.Saga<PreferedCustomerPolicy>()
                .ExpectPublish<CustomerMadePrefered>(m => m.CustomerId == customerId)
                .When(s => s.Handle(Order.For(customerId).WithValue(6000)))
                .ExpectNotPublish<CustomerMadePrefered>(m => m.CustomerId == customerId)
                .When(s => s.Handle(Order.For(customerId).WithValue(4000)));

        }
    }
}