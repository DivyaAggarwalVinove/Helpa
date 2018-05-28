using Helpa.Models;
using Helpa.Utility;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Helpa.Services
{
    public class RegisterServices : IRegisterServices
    {
        public async Task RegisterService(HelpersModel helpersModel)
        {
            HttpClient httpClient = new HttpClient();

            var uri = new Uri(string.Concat(Constants.baseUrl, "api/Account/Register"));
            var json = JsonConvert.SerializeObject(helpersModel);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync(uri, content);            
            string result = await response.Content.ReadAsStringAsync();
            var errorMsg = JsonConvert.DeserializeObject<ResponseModel>(result);
            if (response.IsSuccessStatusCode)
            {
                HelperRegister.Instance.GotoNext(helpersModel.userName, helpersModel.gender, helpersModel.phoneNumber, helpersModel.email);
            }
            else
            {
                Console.Write(errorMsg.message);
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
                    HelperRegister.Instance.GotoNext(helpersModel.userName, helpersModel.gender
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