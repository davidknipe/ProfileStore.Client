namespace ProfileStore.Client.Interfaces
{
    /// <summary>
    /// Used to expose configuration information needed to connect to the Episerver profile store API
    /// </summary>
    public interface IProfileStoreConfig
    {
        /// <summary>
        /// The Root API Url for the profile store
        /// </summary>
        string RootApiUrl { get; }

        /// <summary>
        /// The subscription key required for access to the profile store
        /// </summary>
        string SubscriptionKey { get; }

        /// <summary>
        /// Returns true if config settings exist
        /// </summary>
        /// <returns></returns>
        bool IsConfigured();
    }
}
