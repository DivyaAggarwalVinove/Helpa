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
    public class HelpersServices : IHelpersServices
    {
        public async Task<IEnumerable<HelperHomeModel>> GetHelpersList(int UserId=0, int ServiceId=0, int ScopeId=0, double latitude = 0, double longitude = 0)
        {
            IEnumerable<HelperHomeModel> helpers = new List<HelperHomeModel>();

            /* http://180.151.232.92:127/api/HomeCount?UserId=133&ServiceId=20&ScopeId=68 */
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    HttpClient httpClient = new HttpClient();
                    var uri = new Uri(string.Concat(Constants.baseUrl, "api/HomeCount?UserId=" + UserId + "&latitude=" + latitude + "&longitude=" + longitude + "&ServiceId=" + ServiceId + "&ScopeId=" + ScopeId));

                    //var response = await httpClient.GetAsync(uri);

                    var requestTask = httpClient.GetAsync(uri);
                    var response = Task.Run(() => requestTask);

                    if (response.Result.IsSuccessStatusCode)
                    {
                        string result = await response.Result.Content.ReadAsStringAsync();
                        var h = JsonConvert.DeserializeObject<HHome>(result);
                        helpers = h.Data;
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

            return helpers;
        }

        public async Task<IEnumerable<HelperHome>> GetHelpersInLocation(double Latitude, double Longitude, int UserId)
        {
            IEnumerable<HelperHome> helpers = new List<HelperHome>();

            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    HttpClient httpClient = new HttpClient();
                    var uri = new Uri(string.Concat(Constants.baseUrl, "api/HelpersHomeMap?Latitude=" + Latitude + "&Longitude=" + Longitude +"&UserId=" + UserId ));

                    //var response = await httpClient.GetAsync(uri);

                    var requestTask = httpClient.GetAsync(uri);
                    var response = Task.Run(() => requestTask);

                    if (response.Result.IsSuccessStatusCode)
                    {
                        string result = await response.Result.Content.ReadAsStringAsync();
                        helpers = JsonConvert.DeserializeObject<IEnumerable<HelperHome>>(result);
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

            return helpers;
        }

        public async Task<HHomeModel> GetAllHelpers(int UserId)
        {
            HHomeModel helpers = new HHomeModel();

            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    HttpClient httpClient = new HttpClient();
                    var uri = new Uri(string.Concat(Constants.baseUrl, "api/Home?UserId=" + UserId + "&PageNo=0"));

                    //var response = await httpClient.GetAsync(uri);

                    var requestTask = httpClient.GetAsync(uri);
                    var response = Task.Run(() => requestTask);

                    if (response.Result.IsSuccessStatusCode)
                    {
                        string result = await response.Result.Content.ReadAsStringAsync();
                        helpers = JsonConvert.DeserializeObject<HHomeModel>(result);
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

            return helpers;
        }

        public async Task<NetworkModel> GetMyNetworks(int UserId)
        {
            NetworkModel myNetwork = new NetworkModel();

            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    HttpClient httpClient = new HttpClient();
                    var uri = new Uri(string.Concat(Constants.baseUrl, "api/Favourates/MyNetwork?Id=" + UserId + "&PageNo=0"));

                    //var response = await httpClient.GetAsync(uri);

                    var requestTask = httpClient.GetAsync(uri);
                    var response = Task.Run(() => requestTask);

                    if (response.Result.IsSuccessStatusCode)
                    {
                        string result = await response.Result.Content.ReadAsStringAsync();
                        myNetwork = JsonConvert.DeserializeObject<NetworkModel>(result);
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

            return myNetwork;
        }

        public async Task<NetworkModel> GetSavedUsers(int UserId)
        {
            NetworkModel savedUsers = new NetworkModel();

            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    HttpClient httpClient = new HttpClient();
                    var uri = new Uri(string.Concat(Constants.baseUrl, "api/Helpers/GetSavedUsers?UserId=" + UserId + "&PageNo=1"));

                    //var response = await httpClient.GetAsync(uri);

                    var requestTask = httpClient.GetAsync(uri);
                    var response = Task.Run(() => requestTask);

                    if (response.Result.IsSuccessStatusCode)
                    {
                        string result = await response.Result.Content.ReadAsStringAsync();
                        savedUsers = JsonConvert.DeserializeObject<NetworkModel>(result);
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

            return savedUsers;
        }

        public async Task<HelperServiceModel> SaveHelperServices(HelperServiceModel helperService)
        {
            HelperResponseModel helpers = new HelperResponseModel();

            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    HttpClient httpClient = new HttpClient();

                    var uri = new Uri(string.Concat(Constants.baseUrl, "api/HelperServices"));

                    var json = JsonConvert.SerializeObject(helperService);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    var response = await httpClient.PostAsync(uri, content);

                    //var requestTask = httpClient.PostAsync(uri, content);
                    //var response = Task.Run(() => requestTask);
                    
                    if (response.IsSuccessStatusCode)
                    {
                        string result = await response.Content.ReadAsStringAsync();
                        helpers = JsonConvert.DeserializeObject<HelperResponseModel>(result);
                        helperService.HelperId = helpers.helperid;     
                    }
                    else
                    {
                        string result = await response.Content.ReadAsStringAsync();
                        var message = JsonConvert.DeserializeObject<ResponseModel>(result);
                    }
                }
            }
            catch (Exception e)
            {
                Console.Write(e.StackTrace);
            }

            return helperService;
        }

        public async Task<bool> BookMarkHelper(int userId, int helperId)
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                HttpClient httpClient = new HttpClient();

                // http://180.151.232.92:127/api/Helpers/BookMarkHelper?UserId=1376&HelperUserId=1374
                var uri = new Uri(string.Concat(Constants.baseUrl, "api/Helpers/BookMarkHelper?UserId=" + userId + "&HelperUserId=" + helperId));

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

        public async Task<bool> UnBookMarkHelper(int userId, int helperId)
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                HttpClient httpClient = new HttpClient();

                var uri = new Uri(string.Concat(Constants.baseUrl, "api/Helpers/UnBookMarkHelper?UserId=" + userId + "&HelperUserId=" + helperId));

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

        /* GetAllHelperServices*/
        public async Task<HelperServiceModel> GetHelperServices(int UserId)
        {
            HelperServiceModel helperServices = new HelperServiceModel();

            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    HttpClient httpClient = new HttpClient();
                    var uri = new Uri(string.Concat(Constants.baseUrl, "api/GetAllHelperServices?id=" + UserId));
                    
                    //var response = await httpClient.GetAsync(uri);

                    var requestTask = httpClient.GetAsync(uri);
                    var response = Task.Run(() => requestTask);

                    if (response.Result.IsSuccessStatusCode)
                    {
                        string result = await response.Result.Content.ReadAsStringAsync();
                        helperServices = JsonConvert.DeserializeObject<HelperServiceModel>(result);
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

            return helperServices;
        }
    }
}
 
 
 
 