using System.Configuration;
using EPiServer.ServiceLocation;
using ProfileStore.Client.Interfaces;

namespace ProfileStore.Client.Impl
{
    /// <inheritdoc />
    [ServiceConfiguration(typeof(IProfileStoreConfig), Lifecycle = ServiceInstanceScope.Singleton)]
    public class ProfileStoreConfig : IProfileStoreConfig
    {
        public string RootApiUrl =>
            ConfigurationManager.AppSettings["profileStore.RootApiUrl"];

        public string SubscriptionKey =>
            ConfigurationManager.AppSettings["profileStore.SubscriptionKey"];

        public bool IsConfigured()
        {
            return !string.IsNullOrEmpty(RootApiUrl) &&
                   !string.IsNullOrEmpty(SubscriptionKey);
        }
    }
}
