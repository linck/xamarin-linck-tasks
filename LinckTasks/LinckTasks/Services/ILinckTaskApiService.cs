using LinckTasks.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LinckTasks.Services
{
    public interface ILinckTaskApiService
    {
        Task<List<TaskModel>> GetTasksAsync();
        Task<TaskModel> CreateTask(TaskModel taskModel);
        Task<TaskModel> UpdateTask(TaskModel taskModel);
        Task<TaskModel> DeleteteTask(TaskModel taskModel);
    }
}
