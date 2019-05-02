# ProfileStore.Client for Episerver
Simple .NET client library for accessing the [Episerver Profile Store](https://world.episerver.com/documentation/developer-guides/profile-store/)

## Example usage
```csharp
using Newtonsoft.Json.Linq;
using ProfileStore.Client.Interfaces;

namespace YourProject
{
    public class ProfileClientDemo
    {
        private readonly IProfileStoreRepository _profileStoreRepository;

        public ProfileClientDemo(IProfileStoreRepository profileStoreRepository)
        {
            _profileStoreRepository = profileStoreRepository;
        }

        public JToken GetProfile(string deviceId)
        {
            // Note one profile can have more than one deviceId, 
            // where default tracking is used the deviceId is 
            // in the __madid cookie
            return _profileStoreRepository.GetProfileByDeviceId(deviceId);
        }

        public bool UpdateProfile(string deviceId)
        {
            var profile = _profileStoreRepository.GetProfileByDeviceId(deviceId);

            // Set the company name
            profile["Info"]["Company"] = "Episerver";
            
            // Set the person name
            profile["Name"] = "David Knipe";

            return _profileStoreRepository.UpdateProfile(profile).IsSuccessful;
        }
    }
}
```
