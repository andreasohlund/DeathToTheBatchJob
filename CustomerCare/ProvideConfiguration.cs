namespace CustomerCare
{
    using System;
    using NServiceBus.Config;
    using NServiceBus.Config.ConfigurationSource;

    public class ProvideConfiguration :
        IProvideConfiguration<TransportConfig>
    {
        public TransportConfig GetConfiguration()
        {
            return new TransportConfig
            {
                MaximumConcurrencyLevel = Environment.ProcessorCount / 2,
            };
        }
    }
}