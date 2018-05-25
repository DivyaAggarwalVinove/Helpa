using Helpa.Utility;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Helpa.Services
{
    public class ParentRegisterServices : IParentRegisterServices
    {
        public async Task ParentRegister(HelpersModel helpersModel)
        {
            HttpClient httpClient = new HttpClient();

            var uri = new Uri(string.Concat(Constants.baseUrl, "api/Account/Register"));
            var json = JsonConvert.SerializeObject(helpersModel);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync(uri, content);
            if (response.IsSuccessStatusCode)
            {
                Register.Instance.GotoNext(helpersModel.userName, helpersModel.gender
                        , helpersModel.phoneNumber, helpersModel.email);
            }
        }

        public async Task RegisterExternal(HelpersModel helpersModel)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri($"{Constants.baseUrl}/");            
                var json = JsonConvert.SerializeObject(helpersModel);

                var response = await httpClient.PostAsync($"api/Account/RegisterExternal", new StringContent(json, Encoding.UTF8, "application/json"));
                string message = response.RequestMessage.ToString();

                string result = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    string id = response.RequestMessage.ToString();
                    Register.Instance.GotoNext( helpersModel.userName, helpersModel.gender
                        ,helpersModel.phoneNumber, helpersModel.email);
                }
                else
                {
                    Register.Instance.GotoNext(helpersModel.userName, helpersModel.gender
                        , helpersModel.phoneNumber, helpersModel.email);
                }
            }
            catch(Exception e)
            {
                Console.Write( e.StackTrace.ToString());
            }
        }
    }
}
