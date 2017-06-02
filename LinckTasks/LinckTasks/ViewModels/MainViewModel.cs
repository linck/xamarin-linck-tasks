using LinckTasks;
using LinckTasks.Models;
using LinckTasks.Services;
using LinckTasks.Util;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace LinckTasks.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly AzureLoginService _azureService;
        //INavigation navigation;

        private bool _isBusy;

        ICommand _loginCommand;

        public ICommand LogonCommand =>
            _loginCommand ?? (_loginCommand = new Command(async () => await ExecuteLoginCommandAsync()));


        public MainViewModel()
        {
            _azureService = DependencyService.Get<AzureLoginService>();
            //navigation = nav;

            Title = "Linck Tasks";
        }

        private async Task ExecuteLoginCommandAsync()
        {
            if (_isBusy || !(await LoginAsync()))
            {
                return;
            }
            else
            {
                //var mainPage = new MainPage();
                //await _navigation.PushAsync(mainPage);
                await PushAsync<TaskListViewModel>();

                //RemovePageFromStack();
            }
            _isBusy = false;
        }

        //private void RemovePageFromStack()
        //{
        //    var existingPages = _navigation.NavigationStack;
        //    foreach (var page in existingPages)
        //    {
        //        _navigation.RemovePage(page);
        //    }
        //}

        private Task<bool> LoginAsync()
        {
            if (SettingsUtils.IsLoggedIn)
                return Task.FromResult(true);

            return _azureService.LoginAsync();
        }
    }
}
