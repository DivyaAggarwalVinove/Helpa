using Helpa.Models;
using Helpa.Utility;
using Newtonsoft.Json;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Helpa
{
    class HelpersServices : IHelpersServices<HelperHomeModel>
    {
        public async Task<IEnumerable<HelperHomeModel>> GetHelpersList(int radius, double latitude, double longitude)
        {
            IEnumerable<HelperHomeModel> helpers = new List<HelperHomeModel>();

            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    HttpClient httpClient = new HttpClient();
                    var uri = new Uri(string.Concat(Constants.baseUrl, "api/HelpersHome?Radius=" + radius + "&Latitude=" + latitude + "&Longitude=" + longitude));

                    var response = await httpClient.GetAsync(uri);
                    if (response.IsSuccessStatusCode)
                    {
                        string result = await response.Content.ReadAsStringAsync();
                        helpers = JsonConvert.DeserializeObject<IEnumerable<HelperHomeModel>>(result);
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

        public async Task SaveHelperServices(List<ServiceModel> serviceModels)
        {
            IEnumerable<ResponseModel> helpers = new List<ResponseModel>();

            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    HttpClient httpClient = new HttpClient();

                    var uri = new Uri(string.Concat(Constants.baseUrl, "api/HelperServices"));
                    object obj = new {
                                    UserId = 1054,
                                    Status = true,
                                    Qualification = "MCA",
                                    MinAge = 25,
                                    MaxAge = 40,
                                    Service = serviceModels
                    };

                    var json = JsonConvert.SerializeObject(obj);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    var response = await httpClient.PostAsync(uri, content);
                    string result = await response.Content.ReadAsStringAsync();
                    var message = JsonConvert.DeserializeObject<ResponseModel>(result);
                                        
                    if (response.IsSuccessStatusCode)
                    {
                        result = await response.Content.ReadAsStringAsync();
                        helpers = JsonConvert.DeserializeObject<IEnumerable<ResponseModel>>(result);
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
        }
    }
}