using System;
using System.Collections.Generic;
using Microsoft.WindowsAzure.MobileServices;
using System.Threading.Tasks;
using Xamarin.Forms;
using LinckTasks.Services;
using LinckTasks.Util;
using LinckTasks.Droid.Services;

[assembly: Xamarin.Forms.Dependency(typeof(SocialAuthenticationService))]
namespace LinckTasks.Droid.Services
{
    public class SocialAuthenticationService : IAuthenticationService
    {
        public async Task<MobileServiceUser> LoginAsync(MobileServiceClient client, MobileServiceAuthenticationProvider provider, IDictionary<string, string> parameters = null)
        {
            try
            {
                var user = await client.LoginAsync(Forms.Context, provider);

                SettingsUtils.AuthToken = user?.MobileServiceAuthenticationToken ?? string.Empty;
                SettingsUtils.UserId = user?.UserId;

                return user;
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.Message);
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