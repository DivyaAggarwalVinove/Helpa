using Helpa.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Helpa.Services
{
    public class ParentRegisterServices : IParentRegisterServices
    {
        public async Task RegisterExternal(HelpersModel helpersModel)
        {
            HttpClient httpClient = new HttpClient();
            
            var uri = new Uri(string.Format(Constants.baseUrl, "api/Account/RegisterExternal"));
            var json = JsonConvert.SerializeObject(helpersModel);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync(uri, content);
            if (response.IsSuccessStatusCode)
            {
                //var content = await response.Content.ReadAsStringAsync();
                //Items = JsonConvert.DeserializeObject<List<TodoItem>>(content);
            }
        }
    }
}
