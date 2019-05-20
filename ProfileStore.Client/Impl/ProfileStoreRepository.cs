using System;
using System.Collections.Generic;
using EPiServer.Logging.Compatibility;
using EPiServer.ServiceLocation;
using Newtonsoft.Json.Linq;
using ProfileStore.Client.Interfaces;
using RestSharp;

namespace ProfileStore.Client.Impl
{
    /// <inheritdoc />
    [ServiceConfiguration(typeof(IProfileStoreRepository), Lifecycle = ServiceInstanceScope.Singleton)]
    public class ProfileStoreRepository : IProfileStoreRepository
    {
        private static readonly ILog _log;

        private readonly IProfileStoreConfig _profileStoreConfig;

        private readonly string resourceGetProfile = "api/v1.0/Profiles";
        private readonly string resourceUpdateProfile = "api/v1.0/Profiles/{id}";

        static ProfileStoreRepository()
        {
            _log = LogManager.GetLogger(typeof(ProfileStoreRepository));
        }

        public ProfileStoreRepository(IProfileStoreConfig profileStoreConfig)
        {
            _profileStoreConfig = profileStoreConfig;
        }

        public JToken GetProfileByDeviceId(string deviceId)
        {
            if (_log.IsDebugEnabled)
                _log.DebugFormat("Attempting GetProfileByDeviceId with deviceid: {0}", deviceId);

            return GetProfileInternal(new List<Tuple<string, string>>()
            {
                new Tuple<string, string>("$filter", $"DeviceIds eq {deviceId}")
            }).First;
        }

        public JToken GetProfileByProfileId(string profileId)
        {
            if (_log.IsDebugEnabled)
                _log.DebugFormat("Attempting GetProfileByProfileId with profileid: {0}", profileId);

            return GetProfileInternal(new List<Tuple<string, string>>()
            {
                new Tuple<string, string>("$filter", $"ProfileId eq {profileId}")
            }).First;
        }

        public JArray GetProfilesByEmail(string email)
        {
            if (_log.IsDebugEnabled)
                _log.DebugFormat("Attempting GetProfilesByEmail with email: {0}", email);

            return GetProfileInternal(new List<Tuple<string, string>>()
            {
                new Tuple<string, string>("$filter", $"Info.Email eq {email}")
            });
        }

        public JArray GetProfilesByEmail(string email, string scope)
        {
            if (_log.IsDebugEnabled)
                _log.DebugFormat("Attempting GetProfilesByEmail with email: {0} and scope: {1}", email, scope);

            return GetProfileInternal(new List<Tuple<string, string>>()
            {
                new Tuple<string, string>("$filter", $"Info.Email eq {email} and Scope eq {scope}")
            });
        }

        public JArray GetProfilesByFilter(string filter)
        {
            if (_log.IsDebugEnabled)
                _log.DebugFormat("Attempting GetProfilesByFilter with filter: {0}", filter);

            return GetProfileInternal(new List<Tuple<string, string>>()
            {
                new Tuple<string, string>("$filter", filter)
            });
        }

        public IRestResponse UpdateProfile(JToken profileObject)
        {
            if (_log.IsDebugEnabled)
                _log.DebugFormat("Attempting update of profile object {0}", profileObject.ToString());

            // Set up the update profile request
            var client = new RestClient(_profileStoreConfig.RootApiUrl);
            var profileUpdateRequest = new RestRequest(resourceUpdateProfile, Method.PUT);
            profileUpdateRequest.AddHeader("Authorization", $"epi-single {_profileStoreConfig.SubscriptionKey}");
            profileUpdateRequest.AddUrlSegment("id", profileObject["ProfileId"]);

            // Populate the body to update the profile
            var updateBody = profileObject.ToString();
            profileUpdateRequest.AddParameter("application/json", updateBody, ParameterType.RequestBody);

            // PUT the update request to the API
            var updateResponse = client.Execute(profileUpdateRequest);

            if (_log.IsDebugEnabled)
                _log.DebugFormat("PUT response: {0}", updateResponse.ToString());

            return updateResponse;
        }

        private JArray GetProfileInternal(List<Tuple<string, string>> parameters)
        {
            try
            {
                // Set up the request
                var client = new RestClient(_profileStoreConfig.RootApiUrl);
                var request = new RestRequest(resourceGetProfile, Method.GET);
                request.AddHeader("Authorization", $"epi-single {_profileStoreConfig.SubscriptionKey}");

                // Add required parameters to the request
                parameters.ForEach(x => request.AddParameter(x.Item1, x.Item2));

                // Execute the request to get the profile
                var getProfileResponse = client.Execute(request);
                var getProfileContent = getProfileResponse.Content;

                // Get the results as a JArray object
                var profileResponseObject = JObject.Parse(getProfileContent);
                var profileArray = (JArray)profileResponseObject["items"];

                // Expecting an array of profiles
                return profileArray;
            }
            catch (Exception e)
            {
                if (_log.IsErrorEnabled)
                    _log.ErrorFormat("Exception in GetProfileInternal: {0}", e.ToString());

                throw;
            }
        }
    }
}
