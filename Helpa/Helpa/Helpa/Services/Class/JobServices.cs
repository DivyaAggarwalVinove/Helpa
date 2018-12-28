using Helpa.Models;
using Helpa.Utility;
using Newtonsoft.Json;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Helpa.Services
{
    public class JobServices : IJobServices
    {
        public async Task<IEnumerable<JobsHomeModel>> GetJobsList(int UserId)
        {
            IEnumerable<JobsHomeModel> jobs = new List<JobsHomeModel>();

            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    HttpClient httpClient = new HttpClient();
                    var uri = new Uri(string.Concat(Constants.baseUrl, "api/JobCount?UserId=" + UserId));

                    //var response = await httpClient.GetAsync(uri);

                    var requestTask = httpClient.GetAsync(uri);
                    var response = Task.Run(() => requestTask);

                    if (response.Result.IsSuccessStatusCode)
                    {
                        string result = await response.Result.Content.ReadAsStringAsync();
                        var j = JsonConvert.DeserializeObject<JHome>(result);
                        jobs = j.Data;
                    }
                    else
                    {
                    }
                }
            }
            catch (Exception e)
            {
                Console.Write(e.StackTrace);
            }

            return jobs;
        }

        public async Task<IEnumerable<JobsHome>> GetJobsInLocation(double Latitude, double Longitude, int UserId)
        {
            IEnumerable<JobsHome> jobs = new List<JobsHome>();

            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    HttpClient httpClient = new HttpClient();
                    var uri = new Uri(string.Concat(Constants.baseUrl, "api/JobMapData?Latitude=" + Latitude + "&Longitude=" + Longitude +"&UserId=" + UserId ));

                    //var response = await httpClient.GetAsync(uri);

                    var requestTask = httpClient.GetAsync(uri);
                    var response = Task.Run(() => requestTask);

                    if (response.Result.IsSuccessStatusCode)
                    {
                        string result = await response.Result.Content.ReadAsStringAsync();
                        jobs = JsonConvert.DeserializeObject<IEnumerable<JobsHome>>(result);
                    }
                    else
                    {
                    }
                }
            }
            catch (Exception e)
            {
                Console.Write(e.StackTrace);
            }

            return jobs;
        }

        public async Task<JHomeModel> GetAllJobs(int UserId)
        {
            JHomeModel jobs = new JHomeModel();

            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    HttpClient httpClient = new HttpClient();
                    var uri = new Uri(string.Concat(Constants.baseUrl, "api/Jobs?UserId=" + UserId + "&PageNo=0"));

                    //var response = await httpClient.GetAsync(uri);
                    var requestTask = httpClient.GetAsync(uri);
                    var response = Task.Run(() => requestTask);

                    if (response.Result.IsSuccessStatusCode)
                    {
                        string result = await response.Result.Content.ReadAsStringAsync();
                        jobs = JsonConvert.DeserializeObject<JHomeModel>(result);
                    }
                    else
                    {
                    }
                }
            }
            catch (Exception e)
            {
                Console.Write(e.StackTrace);
            }

            return jobs;
        }

        public async Task<JHomeModel> GetMyJobs(int UserId)
        {
            JHomeModel jobs = new JHomeModel();

            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    HttpClient httpClient = new HttpClient();
                    var uri = new Uri(string.Concat(Constants.baseUrl, "api/Jobs/MyJobPost?id=" + UserId + "&PageNo=1"));

                    //var response = await httpClient.GetAsync(uri);

                    var requestTask = httpClient.GetAsync(uri);
                    var response = Task.Run(() => requestTask);

                    if (response.Result.IsSuccessStatusCode)
                    {
                        string result = await response.Result.Content.ReadAsStringAsync();
                        jobs = JsonConvert.DeserializeObject<JHomeModel>(result);
                    }
                    else
                    {
                    }
                }
            }
            catch (Exception e)
            {
                Console.Write(e.StackTrace);
            }

            return jobs;
        }

        public async Task<JHomeModel> GetSavedJobs(int UserId)
        {
            JHomeModel jobs = new JHomeModel();
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    HttpClient httpClient = new HttpClient();
                    var uri = new Uri(string.Concat(Constants.baseUrl, "api/Jobs/GetSavedJobs?id=" + UserId + "&PageNo=1"));

                    // var response = await httpClient.GetAsync(uri);

                    var requestTask = httpClient.GetAsync(uri);
                    var response = Task.Run(() => requestTask);

                    if (response.Result.IsSuccessStatusCode)
                    {
                        string result = await response.Result.Content.ReadAsStringAsync();
                        jobs = JsonConvert.DeserializeObject<JHomeModel>(result);
                    }
                    else
                    {
                    }
                }
            }
            catch (Exception e)
            {
                Console.Write(e.StackTrace);
            }

            return jobs;
        }

        public async Task<bool> SaveJob(JobPostModel jobPost)
        {
            bool flag = false;
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    HttpClient httpClient = new HttpClient();
                    Uri uri1 = new Uri(Constants.baseUrl + "api/Jobs");
                    StringContent stringContent1 = new StringContent(JsonConvert.SerializeObject((object)jobPost), Encoding.UTF8, "application/json");
                    Uri uri2 = uri1;
                    StringContent stringContent2 = stringContent1;
                    //flag = (await httpClient.PostAsync(uri2, (HttpContent)stringContent2)).IsSuccessStatusCode;

                    var requestTask = httpClient.PostAsync(uri2, (HttpContent)stringContent2);
                    var response = Task.Run(() => requestTask);
                    flag = response.Result.IsSuccessStatusCode;
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.StackTrace);
                flag = false;
            }
            return flag;
        }

        public async Task<bool> BookMarkJob(int userId, int jobId)
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                HttpClient httpClient = new HttpClient();
                
                var uri = new Uri(string.Concat(Constants.baseUrl, "/api/Jobs/BookMarkJobs?id=" + jobId + "&UserId=" + userId));

                var json = JsonConvert.SerializeObject("");
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // var response = await httpClient.PutAsync(uri, content);

                var requestTask = httpClient.PutAsync(uri, content);
                var response = Task.Run(() => requestTask);

                //string result = await response.Result.Content.ReadAsStringAsync();
                //var message = JsonConvert.DeserializeObject<ResponseModel>(result);

                if (response.Result.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            return false;
        }

        public async Task<bool> UnBookMarkJob(int userId, int jobId)
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                HttpClient httpClient = new HttpClient();

                var uri = new Uri(string.Concat(Constants.baseUrl, "/api/Helpers/UnBookMarkJobs?id=" + jobId + "&UserId=" + userId));

                var json = JsonConvert.SerializeObject("");
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                //var response = await httpClient.PutAsync(uri, content);

                var requestTask = httpClient.PutAsync(uri, content);
                var response = Task.Run(() => requestTask);

                //string result = await response.Content.ReadAsStringAsync();
                //var message = JsonConvert.DeserializeObject<ResponseModel>(result);

                if (response.Result.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            return false;
        }

        //public async Task<HelperServiceModel> SaveHelperServices(HelperServiceModel helperService)
        //{
        //    HelperResponseModel helpers = new HelperResponseModel();

        //    try
        //    {
        //        if (CrossConnectivity.Current.IsConnected)
        //        {
        //            HttpClient httpClient = new HttpClient();

        //            var uri = new Uri(string.Concat(Constants.baseUrl, "api/HelperServices"));
        //            //object obj = new {
        //            //                UserId = 1054,
        //            //                Status = true,
        //            //                Qualification = "MCA",
        //            //                MinAge = 25,
        //            //                MaxAge = 40,
        //            //                Service = serviceModels
        //            //};

        //            var json = JsonConvert.SerializeObject(helperService);
        //            var content = new StringContent(json, Encoding.UTF8, "application/json");

        //            var response = await httpClient.PostAsync(uri, content);
        //            string result = await response.Content.ReadAsStringAsync();
        //            var message = JsonConvert.DeserializeObject<ResponseModel>(result);

        //            if (response.IsSuccessStatusCode)
        //            {
        //                result = await response.Content.ReadAsStringAsync();
        //                helpers = JsonConvert.DeserializeObject<HelperResponseModel>(result);
        //                helperService.HelperId = helpers.helperid;     
        //            }
        //            else
        //            {
        //            }
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Console.Write(e.StackTrace);
        //    }

        //    return helperService;
        //}
    }
}
 
 