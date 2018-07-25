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

    }
}