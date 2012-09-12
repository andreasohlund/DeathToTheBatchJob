namespace CustomerCare.Tests
{
    using Contracts;
    using NServiceBus.Testing;
    using NUnit.Framework;

    [TestFixture]
    public class When_reaching_the_threshold_within_a_year : PreferedCustomerPolicySpecification
    {
        [Test]
        public void Should_become_a_prefered_customer()
        {
            Test.Saga<PreferedCustomerPolicy>()
                .ExpectNotPublish<CustomerMadePrefered>(m => m.CustomerId == customerId)
                .When(s => s.Handle(Order.For(customerId).WithValue(2000)))
                .ExpectPublish<CustomerMadePrefered>(m => m.CustomerId == customerId)
                .When(s => s.Handle(Order.For(customerId).WithValue(4000)));

        }
    }
}
