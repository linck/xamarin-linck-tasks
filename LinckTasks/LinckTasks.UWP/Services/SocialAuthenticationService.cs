using LinckTasks.Services;
using LinckTasks.Util;
using LinckTasks.UWP.Services;
using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: Xamarin.Forms.Dependency(typeof(SocialAuthenticationService))]
namespace LinckTasks.UWP.Services
{
    public class SocialAuthenticationService : IAuthenticationService
    {
        public async Task<MobileServiceUser> LoginAsync(MobileServiceClient client, MobileServiceAuthenticationProvider provider, IDictionary<string, string> parameters = null)
        {
            try
            {
                var user = await client.LoginAsync(provider);

                SettingsUtils.AuthToken = user?.MobileServiceAuthenticationToken ?? string.Empty;
                SettingsUtils.UserId = user?.UserId;

                return user;
            }
            catch (Exception)
            {
                //TODO: LogError
                return null;
            }
        }

        public async Task LogoutAsync(MobileServiceClient client)
        {
            await client.LogoutAsync();
        }
    }
}
