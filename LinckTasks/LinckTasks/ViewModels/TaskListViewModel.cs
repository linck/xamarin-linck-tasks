using LinckTasks.Models;
using LinckTasks.Services;
using LinckTasks.Util;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LinckTasks.ViewModels
{
    public class TaskListViewModel : BaseViewModel
    {
        private readonly AzureLoginService azureService;
        private ILinckTaskApiService apiService;
        public ObservableCollection<TaskModel> TaskModels { get; }
        public Command<TaskModel> TaskEditionCommand { get; }
        public Command LogoutCommand { get; }
        private bool isBusy;

        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        private bool listViewVisible;

        public bool ListViewVisible
        {
            get { return listViewVisible; }
            set { SetProperty(ref listViewVisible, value); }
        }



        public TaskListViewModel(ILinckTaskApiService apiService)
        {
            azureService = DependencyService.Get<AzureLoginService>();
            this.apiService = apiService;
            TaskModels = new ObservableCollection<TaskModel>();
            TaskEditionCommand = new Command<TaskModel>(ExecuteTaskEditionCommand);
            LogoutCommand = new Command(ExecuteLogoutCommand);
            Title = "Minhas Tarefas";
        }

        private async void ExecuteLogoutCommand(object obj)
        {
            await azureService.LogoutAsync();
            await PushAsync<MainViewModel>();
        }

        private async void ExecuteTaskEditionCommand(TaskModel taskModel)
        {
            await PushAsync<TaskEditionViewModel>(taskModel);
        }

        public async Task LoadAsync()
        {
            listViewVisible = false;
            IsBusy = true;
            var taskModels = await apiService.GetTasksAsync();
            TaskModels.Clear();

            foreach (var taskModel in taskModels)
            {
                TaskModels.Add(taskModel);
            }
            IsBusy = false;
            listViewVisible = true;
        }
    }
}
