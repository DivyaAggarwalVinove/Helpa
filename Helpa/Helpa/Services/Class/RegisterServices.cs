using Helpa.Models;
using Helpa.Utility;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Plugin.Connectivity;
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
            try
            {
                if (CrossConnectivity.Current.IsConnected)
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
                        userModel.Id = int.Parse(message.Id);
                        userModel.IsLogged = true;
                        HelperRegister.Instance.GotoNext(userModel);
                    }
                    else
                    {
                        Console.Write(message.Message);
                    }
                }
            }
            catch (Exception e)
            {
                Console.Write(e.StackTrace);
            }
        }

        public async Task RegisterExternal(RegisterUserModel userModel)
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    HttpClient httpClient = new HttpClient();

                    var uri = new Uri(string.Concat(Constants.baseUrl, "api/Account/RegisterExternal"));
                    var json = JsonConvert.SerializeObject(userModel);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    var response = await httpClient.PostAsync(uri, content);
                    string result = await response.Content.ReadAsStringAsync();
                    var message = JsonConvert.DeserializeObject<ResponseModel>(result);

                    if (response.IsSuccessStatusCode)
                    {
                        userModel.Id = int.Parse(message.Id);
                        userModel.IsLogged = true;
                        Register.Instance.GotoNext(userModel);
                    }
                    else
                    {
                        //string result = await response.Content.ReadAsStringAsync();
                        //var errorMsg = JsonConvert.DeserializeObject<ResponseModel>(result);
                        Console.Write(message.Message);
                    }
                }
            }
            catch(Exception e)
            {
                Console.Write( e.StackTrace.ToString());
            }
        }

        public async Task CompleteRegisterService(RegisterUserModel userModel)
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    HttpClient httpClient = new HttpClient();

                    var uri = new Uri(string.Concat(Constants.baseUrl, "api/Account/complete-registration/"+userModel.Id));
                    var json = JsonConvert.SerializeObject(userModel);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    var response = await httpClient.PostAsync(uri, content);

                    string result = await response.Content.ReadAsStringAsync();
                    var message = JsonConvert.DeserializeObject<ResponseModel>(result);
                    if (response.IsSuccessStatusCode)
                    {
                        userModel.Id = int.Parse(message.Id);
                        userModel.IsLogged = true;
                        HelperRegister.Instance.GotoNext(userModel);
                    }
                    else
                    {
                        Console.Write(message.Message);
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