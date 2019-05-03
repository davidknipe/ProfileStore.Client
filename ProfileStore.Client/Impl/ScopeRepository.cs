using System;
using System.Collections.Generic;
using EPiServer.Logging.Compatibility;
using EPiServer.ServiceLocation;
using Newtonsoft.Json.Linq;
using ProfileStore.Client.Interfaces;
using ProfileStore.Client.Models;
using RestSharp;

namespace ProfileStore.Client.Impl
{
    /// <inheritdoc />
    [ServiceConfiguration(typeof(IScopeRepository), Lifecycle = ServiceInstanceScope.Singleton)]
    public class ScopeRepository : IScopeRepository
    {
        private static readonly ILog _log;

        private readonly IProfileStoreConfig _profileStoreConfig;

        private readonly string resourceScope = "api/v1.0/scopes/";

        static ScopeRepository()
        {
            _log = LogManager.GetLogger(typeof(ScopeRepository));
        }

        public ScopeRepository(IProfileStoreConfig profileStoreConfig)
        {
            _profileStoreConfig = profileStoreConfig;
        }

        public IList<Scope> GetAllScopes()
        {
            if (_log.IsDebugEnabled)
                _log.Debug("Attempting GetAllScopes");

            var allScopes = GetScopesInternal(resourceScope);
            var scopes = new List<Scope>();
            foreach (var scope in allScopes)
            {
                scopes.Add(
                    new Scope()
                    {
                        ScopeId = scope["ScopeId"].ToString(),
                        Description = scope["Description"].ToString()
                    });
            }
            return scopes;
        }

        public Scope GetScope(string scopeId)
        {
            if (_log.IsDebugEnabled)
                _log.DebugFormat("Attempting GetScope with scopeId: {0}", scopeId);

            var scope = GetScopeInternal(resourceScope + scopeId);
            if (scope != null)
            {
                return new Scope()
                {
                    ScopeId = scope["ScopeId"].ToString(),
                    Description = scope["Description"].ToString()
                };
            }

            return null;
        }

        public bool UpsertScope(Scope scope)
        {
            if (_log.IsDebugEnabled)
                _log.DebugFormat("Attempting update of scope object {0}", scope.ToString());

            // Set up the update profile request
            var client = new RestClient(_profileStoreConfig.RootApiUrl);
            var profileUpdateRequest = new RestRequest(resourceScope + scope.ScopeId, Method.PUT);
            profileUpdateRequest.AddHeader("Authorization", $"epi-single {_profileStoreConfig.SubscriptionKey}");

            // Populate the body to update the profile
            var updateBody = string.Format("{{ \"Description\": \"{0}\" }}", scope.Description);
            profileUpdateRequest.AddParameter("application/json", updateBody, ParameterType.RequestBody);

            // PUT the update request to the API
            var updateResponse = client.Execute(profileUpdateRequest);

            if (_log.IsDebugEnabled)
                _log.DebugFormat("PUT response: {0}", updateResponse.ToString());

            return updateResponse.IsSuccessful;
        }

        private JArray GetScopesInternal(string resource)
        {
            try
            {
                // Set up the request
                var client = new RestClient(_profileStoreConfig.RootApiUrl);
                var request = new RestRequest(resource, Method.GET);
                request.AddHeader("Authorization", $"epi-single {_profileStoreConfig.SubscriptionKey}");

                // Execute the request to get the profile
                var getProfileResponse = client.Execute(request);
                var getProfileContent = getProfileResponse.Content;

                // Get the results as a JArray object
                var scopesResponseObject = JObject.Parse(getProfileContent);
                var scopeArray = (JArray) scopesResponseObject["items"];

                // Expecting an array of scopes
                return scopeArray;
            }
            catch (Exception e)
            {
                if (_log.IsErrorEnabled)
                    _log.ErrorFormat("Exception in GetScopesInternal: {0}", e.ToString());

                throw;
            }
        }

        private JObject GetScopeInternal(string resource)
        {
            try
            {
                // Set up the request
                var client = new RestClient(_profileStoreConfig.RootApiUrl);
                var request = new RestRequest(resource, Method.GET);
                request.AddHeader("Authorization", $"epi-single {_profileStoreConfig.SubscriptionKey}");

                // Execute the request to get the profile
                var getProfileResponse = client.Execute(request);
                var getProfileContent = getProfileResponse.Content;

                // Get the results as a JObject
                var scopeResponseObject = JObject.Parse(getProfileContent);

                return scopeResponseObject;
            }
            catch (Exception e)
            {
                if (_log.IsErrorEnabled)
                    _log.ErrorFormat("Exception in GetScopeInternal: {0}", e.ToString());

                throw;
            }
        }
    }
}