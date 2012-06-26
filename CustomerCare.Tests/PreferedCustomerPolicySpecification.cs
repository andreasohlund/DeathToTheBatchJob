namespace CustomerCare.Tests
{
    using System;
    using NServiceBus.Testing;
    using NUnit.Framework;

    public class PreferedCustomerPolicySpecification
    {
        protected Guid customerId = Guid.NewGuid();

        [TestFixtureSetUp]
        public void Setup()
        {
            Test.Initialize();
        }
    }
}