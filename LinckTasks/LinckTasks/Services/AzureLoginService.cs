using LinckTasks.Services;
using LinckTasks.Util;
using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(AzureLoginService))]
namespace LinckTasks.Services
{
    public class AzureLoginService
    {
        //TODO: refatorar de acordo cm PDFs
        static readonly string AppUrl = "https://lincktask.azurewebsites.net/";
        public MobileServiceClient Client { get; set; } = null;
        public static bool UseAuth { get; set; } = false;
        private IAuthenticationService authService;

        public void Initialize()
        {
            authService = DependencyService.Get<IAuthenticationService>();
            Client = new MobileServiceClient(AppUrl);

            if (!string.IsNullOrWhiteSpace(SettingsUtils.AuthToken) && !string.IsNullOrWhiteSpace(SettingsUtils.UserId))
            {
                Client.CurrentUser = new MobileServiceUser(SettingsUtils.UserId)
                {
                    MobileServiceAuthenticationToken = SettingsUtils.AuthToken
                };
            }
        }

        public async Task<bool> LoginAsync()
        {
            Initialize();

            var user = await authService.LoginAsync(Client, MobileServiceAuthenticationProvider.MicrosoftAccount);

            if (user == null)
            {
                SettingsUtils.AuthToken = string.Empty;
                SettingsUtils.UserId = string.Empty;

                Device.BeginInvokeOnMainThread(async () =>
                {
                    await Application.Current.MainPage.DisplayAlert("Erro", "Não foi possível efetuar login, tente novamente.", "OK");
                });
                return false;
            }
            else
            {
                SettingsUtils.AuthToken = user.MobileServiceAuthenticationToken;
                SettingsUtils.UserId = user.UserId;
            }
            return true;
        }

        public async Task<bool> LogoutAsync()
        {
            Initialize();
            SettingsUtils.AuthToken = string.Empty;
            SettingsUtils.UserId = string.Empty;
            await authService.LogoutAsync(Client);
            return true;
        }
    }
}
