using LinckTasks.Models;
using LinckTasks.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LinckTasks.ViewModels
{
    class TaskEditionViewModel : BaseViewModel
    {
        private string taskTitle;
        public string TaskTitle { get { return taskTitle; } set { SetProperty(ref taskTitle, value); } }
        private string description;
        public string Description { get { return description; } set { SetProperty(ref description, value); } }
        public string author;
        public string Author { get { return author; } set { SetProperty(ref author, value); } }
        public string Id { get; set; }
        private DateTime date;
        public DateTime Date { get { return date; } set { SetProperty(ref date, value); } }
        private bool done;
        public bool Done { get { return done; } set { SetProperty(ref done, value); } }

        public bool Editing { get; set; }

        public ILinckTaskApiService apiService;
        public Command SaveTaskCommand { get; }
        public Command DeleteTaskCommand { get; }

        public TaskEditionViewModel(ILinckTaskApiService apiService, TaskModel taskModel)
        {
            if (taskModel != null)
            {
                TaskTitle = taskModel.Title;
                Description = taskModel.Description;
                Author = taskModel.Author;
                Title = $"Editar {taskModel.Title}";
                Id = taskModel.Id;
                Editing = true;
            }
            else
            {
                Title = "Criar Tarefa";
                Editing = false;
            }

            this.apiService = apiService;
            SaveTaskCommand = new Command(ExecuteSaveTaskCommand);
            DeleteTaskCommand = new Command(ExecuteDeleteTask, CanExecuteDeleteTask);
        }

        private bool CanExecuteDeleteTask()
        {
            return Editing;
        }

        private async void ExecuteDeleteTask()
        {
            var taskModel = new TaskModel();
            taskModel.Id = Id;

            await apiService.DeleteteTask(taskModel);
            await PushAsync<TaskListViewModel>();
        }

        private async void ExecuteSaveTaskCommand()
        {
            var taskModel = new TaskModel();
            taskModel = new TaskModel();
            taskModel.Title = TaskTitle;
            taskModel.Description = Description;
            taskModel.Author = Author;

            if (!Editing)
            {
                await apiService.CreateTask(taskModel);
            }
            else
            {
                taskModel.Id = Id;
                await apiService.UpdateTask(taskModel);
            }
            await PushAsync<TaskListViewModel>();
        }
    }
}
