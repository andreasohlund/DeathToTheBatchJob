namespace Sales
{
    using NServiceBus;

    public class EndpointConfig:IConfigureThisEndpoint,AsA_Publisher,IWantCustomInitialization
    {
        public void Init()
        {
            Configure.With()
                .DefiningMessagesAs(t => t.Namespace != null && t.Namespace.EndsWith(".Contracts"));
        }
    }
}
