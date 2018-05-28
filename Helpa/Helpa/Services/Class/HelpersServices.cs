using Newtonsoft.Json;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Helpa
{
    class HelpersServices : IHelpersServices<HelpersModel>
    {
        HttpClient client;
        IEnumerable<HelpersModel> helpersModels;

        public HelpersServices()
        {
            client = new HttpClient();
            //client.BaseAddress = new Uri($"{App.BackendUrl}/");

            helpersModels = new List<HelpersModel>();
        }

        public async Task<IEnumerable<HelpersModel>> GetHelpersList(bool forceRefresh = false)
        {
            if (forceRefresh && CrossConnectivity.Current.IsConnected)
            {                
                var json = await client.GetStringAsync($"api/get-icon-list");
                //helpersModels = await Task.Run(() => JsonConvert.DeserializeObject<IEnumerable<HelpersModel>>(json));
            }

            return helpersModels;
        }
    }
}