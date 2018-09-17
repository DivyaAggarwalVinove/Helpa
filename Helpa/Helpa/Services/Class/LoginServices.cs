﻿
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
    public class LoginServices : ILoginServices
    {
        public async Task<LoginErrorResponseModel> Login(string username, string email, string pwd)
        {
            LoginErrorResponseModel err_msg = new LoginErrorResponseModel();
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    var dict = new Dictionary<string, string>();
                    dict.Add("username", email);
                    dict.Add("password", pwd);
                    dict.Add("grant_type", "password");
                    dict.Add("deviceId", "Android");

                    var url = new Uri(string.Concat(Constants.baseUrl, "Token"));

                    HttpClient client = new HttpClient();
                    var req = new HttpRequestMessage(HttpMethod.Post, url) { Content = new FormUrlEncodedContent(dict) };
                    var res = await client.SendAsync(req);

                    string result = await res.Content.ReadAsStringAsync();
                    if (res.IsSuccessStatusCode)
                    {
                        LoginResponseModel message = JsonConvert.DeserializeObject<LoginResponseModel>(result);
                        RegisterUserModel user = await App.Database.GetUsersAsync(message.id);
                        if (user == null)
                            user = new RegisterUserModel();

                        user.Id = message.id;
                        Settings.UserID = user.Id;
                        user.UserName = message.userName;
                        user.Token = message.access_token;
                        Settings.TokenCode = user.Token;
                        user.isLoggedIn = true;
                        //user.Role = message.roles;
                        await App.Database.SaveUserAsync(user);

                        return null;
                    }
                    else
                    {
                        err_msg = JsonConvert.DeserializeObject<LoginErrorResponseModel>(result);
                        return err_msg;
                    }
                }
                else
                {
                    err_msg.error_description = "Please check your Internet Connection";
                }
            }
            catch (Exception e)
            {
                Console.Write(e.StackTrace);
                err_msg.error_description = "Something wrong! Please try again";
                
            }

            return err_msg;
        }
        
    }
}
