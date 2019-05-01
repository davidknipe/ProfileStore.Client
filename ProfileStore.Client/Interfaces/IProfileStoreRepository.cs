using Newtonsoft.Json.Linq;
using RestSharp;

namespace ProfileStore.Client.Interfaces
{
    /// <summary>
    /// Used to access the profile in the Episerver profile store
    /// </summary>
    public interface IProfileStoreRepository
    {
        /// <summary>
        /// Get the profile based on the device Id
        /// </summary>
        /// <param name="deviceId">The device id assigned to the current device, typically this is assigned with a cookie called '__madid' in web scenarios</param>
        /// <returns>Object representing the profile</returns>
        JToken GetProfileByDeviceId(string deviceId);

        /// <summary>
        /// Get the profile based on the email address of the user
        /// </summary>
        /// <param name="email">The email to query for profiles on</param>
        /// <returns>A list of objects representing the profiles matching the email (possible to have more than one due to multiple scopes)</returns>
        JArray GetProfilesByEmail(string email);

        /// <summary>
        /// Get the profile based on the email address of the user
        /// </summary>
        /// <param name="email">The email to query for profiles on</param>
        /// <param name="scope">The scope to filter on when searching</param>
        /// <returns>A list of objects representing the profiles matching the email (possible to have more than one due to multiple scopes)</returns>
        JArray GetProfilesByEmail(string email, string scope);

        /// <summary>
        /// Get the profile based on the filter passed
        /// </summary>
        /// <param name="filter">The filter to query profiles on</param>
        /// <returns>A list of objects representing the profiles matching the filter</returns>
        JArray GetProfilesByFilter(string filter);

        /// <summary>
        /// Update the profile in the profile store
        /// </summary>
        /// <param name="profileObject">The object representing the profile</param>
        /// <returns>The response from the service</returns>
        IRestResponse UpdateProfile(JToken profileObject);
    }
}