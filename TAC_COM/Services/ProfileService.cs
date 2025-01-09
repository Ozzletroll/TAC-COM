using TAC_COM.Audio.Profiles;
using TAC_COM.Models;
using TAC_COM.Services.Interfaces;

namespace TAC_COM.Services
{
    /// <summary>
    /// Class responsible for configuring and returning all 
    /// <see cref="Profile"/>s.
    /// </summary>
    /// <param name="uriService"> The <see cref="IUriService"/> to use.</param>
    public class ProfileService(IUriService uriService)
    {
        private readonly IUriService UriProvider = uriService;

        /// <summary>
        /// Method to return all <see cref="Profile"/>s.
        /// </summary>
        /// <returns> A list of all profiles.</returns>
        public List<Profile> GetAllProfiles()
        {
            List<Profile> defaultProfiles = [];

            defaultProfiles.Add(new GMSProfile(UriProvider));
            defaultProfiles.Add(new SSCProfile(UriProvider));
            defaultProfiles.Add(new IPSNProfile(UriProvider));
            defaultProfiles.Add(new HAProfile(UriProvider));
            defaultProfiles.Add(new HORUSProfile(UriProvider));

            return defaultProfiles;
        }
    }
}
