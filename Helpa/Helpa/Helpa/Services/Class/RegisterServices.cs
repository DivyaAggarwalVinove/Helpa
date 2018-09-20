using Helpa.Models;
using Helpa.Utility;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Plugin.Connectivity;
using System;
using System.Collections;
using System.Collections.Generic;
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
                        userModel.IsRegistered = true;
                        if (userModel.Role.Equals("HELPER"))
                            HelperRegister.Instance.GotoNext(userModel);
                        else
                            Register.Instance.GotoNext(userModel);
                    }
                    else
                    {
                        if (userModel.Role.Equals("HELPER"))
                            HelperRegister.Instance.ShowError(message.Message);
                        else
                            Register.Instance.ShowError(message.Message);

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
                        userModel.IsRegistered = true;
                        HelperRegister.Instance.GotoNext(userModel);
                    }
                    else
                    {
                        userModel.IsRegistered = true;
                        HelperRegister.Instance.GotoNext(userModel);
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
                        userModel.IsCompleted = true;

                        if (userModel.Role.Equals("PARENT"))
                            Register1.Instance.GotoNext(userModel);
                        else
                            HelperCompleteRegister.Instance.GotoNext(userModel);
                        
                    }
                    else
                    {
                        if (userModel.Role.Equals("PARENT"))
                            Register1.Instance.ShowError(message.Message);
                        else
                            HelperCompleteRegister.Instance.ShowError(message.Message);
                        Console.Write(message.Message);
                    }
                }
            }
            catch (Exception e)
            {
                Console.Write(e.StackTrace);
            }
        }

        public async Task FacebookSignUp(ExternalLoginViewModel externalDetail)
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    HttpClient httpClient = new HttpClient();
                    //FormUrlEncodedContent
                    var uri = new Uri(string.Concat(Constants.baseUrl, externalDetail.Url));

                    var response = await httpClient.GetAsync(uri);

                    if (response.IsSuccessStatusCode)
                    {
                        var result = response.Content.GetType();
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

        public async Task<IEnumerable<ExternalLoginViewModel>> GetExternalDetails()
        {
            IEnumerable<ExternalLoginViewModel> extrenalLoginDetail = new List<ExternalLoginViewModel>();

            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    HttpClient httpClient = new HttpClient();

                    var uri = new Uri(string.Concat(Constants.baseUrl, "/api/Account/ExternalLogins?returnUrl=%2F&generateState=true"));

                    var response = await httpClient.GetAsync(uri);

                    if (response.IsSuccessStatusCode)
                    {
                        string result = await response.Content.ReadAsStringAsync();
                        extrenalLoginDetail = JsonConvert.DeserializeObject<IEnumerable<ExternalLoginViewModel>>(result);
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

            return extrenalLoginDetail;
        }
    }
}