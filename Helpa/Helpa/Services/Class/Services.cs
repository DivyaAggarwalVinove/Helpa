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
    class Services : IServices
    {
        public async Task<IEnumerable<ServiceModel>> GetServicesAsync()
        {
            IEnumerable<ServiceModel> services = new List<ServiceModel>();
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    HttpClient httpClient = new HttpClient();
                    var uri = new Uri(string.Concat(Constants.baseUrl, "api/Services"));
                    var response = await httpClient.GetAsync(uri);                    
                    if (response.IsSuccessStatusCode)
                    {
                        ServiceModel sm = new ServiceModel();
                        string result = await response.Content.ReadAsStringAsync();
                        services = JsonConvert.DeserializeObject<IEnumerable<ServiceModel>>(result);
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

            return services;
        }
    }
}
