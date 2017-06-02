using LinckTasks.Models;
using LinckTasks.Services;
using LinckTasks.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LinckTasks
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TaskListPage : ContentPage
    {
        private TaskListViewModel ViewModel => BindingContext as TaskListViewModel;

        public TaskListPage()
        {
            InitializeComponent();
            var apiService = DependencyService.Get<ILinckTaskApiService>();
            BindingContext = new TaskListViewModel(apiService);
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (ViewModel != null)
            {
                await ViewModel.LoadAsync();
            }
        }

        private async void FetchTasks()
        {
            List<TaskModel> tasksModels = await new LinckTaskApiService().GetTasksAsync();

            Debug.WriteLine("Minhas tasks");
            foreach (var task in tasksModels)
            {
                Debug.WriteLine(task.Title);
            }

        }

        private void ListView_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ViewModel.TaskEditionCommand.Execute(e.SelectedItem);
        }
    }
}