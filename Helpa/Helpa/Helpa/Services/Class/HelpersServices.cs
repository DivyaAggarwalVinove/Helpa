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
        public async Task<IEnumerable<HelperHomeModel>> GetHelpersList(int UserId)
        {
            IEnumerable<HelperHomeModel> helpers = new List<HelperHomeModel>();

            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    HttpClient httpClient = new HttpClient();
                    var uri = new Uri(string.Concat(Constants.baseUrl, "api/HomeCount?UserId=" + UserId));

                    var response = await httpClient.GetAsync(uri);
                    if (response.IsSuccessStatusCode)
                    {
                        string result = await response.Content.ReadAsStringAsync();
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

                    var response = await httpClient.GetAsync(uri);
                    if (response.IsSuccessStatusCode)
                    {
                        string result = await response.Content.ReadAsStringAsync();
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

                    var response = await httpClient.GetAsync(uri);
                    if (response.IsSuccessStatusCode)
                    {
                        string result = await response.Content.ReadAsStringAsync();
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
            UserId = 1257;
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    HttpClient httpClient = new HttpClient();
                    var uri = new Uri(string.Concat(Constants.baseUrl, "api/Favourates/MyNetwork?Id=" + UserId + "&PageNo=0"));

                    var response = await httpClient.GetAsync(uri);
                    if (response.IsSuccessStatusCode)
                    {
                        string result = await response.Content.ReadAsStringAsync();
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

                    var response = await httpClient.GetAsync(uri);
                    if (response.IsSuccessStatusCode)
                    {
                        string result = await response.Content.ReadAsStringAsync();
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
                    //object obj = new {
                    //                UserId = 1054,
                    //                Status = true,
                    //                Qualification = "MCA",
                    //                MinAge = 25,
                    //                MaxAge = 40,
                    //                Service = serviceModels
                    //};

                    var json = JsonConvert.SerializeObject(helperService);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    var response = await httpClient.PostAsync(uri, content);
                    string result = await response.Content.ReadAsStringAsync();
                    var message = JsonConvert.DeserializeObject<ResponseModel>(result);
                                        
                    if (response.IsSuccessStatusCode)
                    {
                        result = await response.Content.ReadAsStringAsync();
                        helpers = JsonConvert.DeserializeObject<HelperResponseModel>(result);
                        helperService.HelperId = helpers.helperid;     
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

                var response = await httpClient.PutAsync(uri, content);
                string result = await response.Content.ReadAsStringAsync();
                var message = JsonConvert.DeserializeObject<ResponseModel>(result);

                if (response.IsSuccessStatusCode)
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

                var response = await httpClient.PutAsync(uri, content);
                string result = await response.Content.ReadAsStringAsync();
                var message = JsonConvert.DeserializeObject<ResponseModel>(result);

                if (response.IsSuccessStatusCode)
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
    }
}
 
 