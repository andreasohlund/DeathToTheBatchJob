namespace CustomerCare.Tests
{
    using System;
    using NServiceBus.Testing;
    using NUnit.Framework;

    public class PreferedCustomerPolicySpecification
    {
        protected string customerId = Guid.NewGuid().ToString();

        [TestFixtureSetUp]
        public void Setup()
        {
            Test.Initialize();
        }
    }
}