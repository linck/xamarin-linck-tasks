using System.Collections.Generic;
using System.Threading.Tasks;
using LinckTasks.Models;
using System.Net.Http;
using Newtonsoft.Json;
using System.IO;
using System.Net.Http.Headers;
using Xamarin.Forms;
using LinckTasks.Services;
using System.Text;
using System;
using System.Diagnostics;

[assembly: Dependency(typeof(LinckTaskApiService))]
namespace LinckTasks.Services
{
    public class LinckTaskApiService : ILinckTaskApiService
    {
        private const string BaseUrl = "https://lincktask.azurewebsites.net/tables/";

        public HttpClient GetHttpClient()
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Add("ZUMO-API-VERSION", "2.0.0");

            return httpClient;
        }

        public async Task<List<TaskModel>> GetTasksAsync()
        {
            var response = await GetHttpClient().GetAsync($"{BaseUrl}task").ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                using (var responseStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false))
                {
                    return JsonConvert.DeserializeObject<List<TaskModel>>(
                        await new StreamReader(responseStream)
                            .ReadToEndAsync().ConfigureAwait(false));
                }
            }

            return null;
        }

        public async Task<TaskModel> CreateTask(TaskModel taskModel)
        {
            StringContent content = new StringContent(JsonConvert.SerializeObject(taskModel), Encoding.UTF8, "application/json");
            await GetHttpClient().PostAsync($"{BaseUrl}task", content).ConfigureAwait(false); ;

            return null;
        }

        public async Task<TaskModel> UpdateTask(TaskModel taskModel)
        {

            StringContent content = new StringContent(JsonConvert.SerializeObject(taskModel), Encoding.UTF8, "application/json");
            Debug.WriteLine(JsonConvert.SerializeObject(taskModel));
            var method = new HttpMethod("PATCH");
            var request = new HttpRequestMessage(method, $"{BaseUrl}task/{taskModel.Id}")
            {
                Content = content
            };

            await GetHttpClient().SendAsync(request);

            return null;
        }

        public async Task<TaskModel> DeleteteTask(TaskModel taskModel)
        {
            StringContent content = new StringContent(JsonConvert.SerializeObject(taskModel), Encoding.UTF8, "application/json");
            Debug.WriteLine(JsonConvert.SerializeObject(taskModel));
            var method = new HttpMethod("DELETE");
            var request = new HttpRequestMessage(method, $"{BaseUrl}task/{taskModel.Id}")
            {
                Content = content
            };

            await GetHttpClient().SendAsync(request);

            return null;
        }
    }
}
