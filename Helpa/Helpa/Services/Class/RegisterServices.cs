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
        public async Task RegisterService(RegisterUserModel userModel)
        {
            HttpClient httpClient = new HttpClient();

            var uri = new Uri(string.Concat(Constants.baseUrl, "api/Account/Register"));
            var json = JsonConvert.SerializeObject(userModel);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync(uri, content);
            string result = await response.Content.ReadAsStringAsync();
            var message = JsonConvert.DeserializeObject<ResponseModel>(result);
            if (response.IsSuccessStatusCode)
            {
                userModel.Id = int.Parse(message.Data);
                userModel.IsLogged = true;
                HelperRegister.Instance.GotoNext(userModel);
            }
            else
            {                
                Console.Write(message.Message);
            }
        }

        public async Task RegisterExternal(RegisterUserModel userModel)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri($"{Constants.baseUrl}/");            
                var json = JsonConvert.SerializeObject(userModel);

                var response = await httpClient.PostAsync($"api/Account/RegisterExternal", new StringContent(json, Encoding.UTF8, "application/json"));
                string message = response.RequestMessage.ToString();
                if (response.IsSuccessStatusCode)
                {
                    string id = response.RequestMessage.ToString();
                    Register.Instance.GotoNext(userModel);
                }
                else
                {
                    string result = await response.Content.ReadAsStringAsync();
                    var errorMsg = JsonConvert.DeserializeObject<ResponseModel>(result);
                    Console.Write(errorMsg.Message);
                }
            }
            catch(Exception e)
            {
                Console.Write( e.StackTrace.ToString());
            }
        }
    }
}