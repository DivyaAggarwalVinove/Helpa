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
        public async Task<LoginErrorResponseModel> Login(string email, string pwd)
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
                        if(message!=null && !message.id.Equals(""))
                        {
                            message.userId = int.Parse(message.id);
                        }

                        RegisterUserModel user = await App.Database.GetUsersAsync(message.userId);
                        if (user == null)
                            user = new RegisterUserModel();

                        user.Id = message.userId;
                        user.UserName = message.userName;
                        user.Token = message.access_token;
                        user.isLoggedIn = true;
                        user.Role = message.roles;
                        user.Email = email;
                        user.profileImage = message.profileImage;
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

        #region User Basic Info Services 
        public async Task<UserModel> GetUserBasicInfo(int UserId)
        {
            UserModel savedUsers = new UserModel();

            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    HttpClient httpClient = new HttpClient();
                    var uri = new Uri(string.Concat(Constants.baseUrl, "api/GetBasic?id=" + UserId));

                    var response = await httpClient.GetAsync(uri);
                    if (response.IsSuccessStatusCode)
                    {
                        string result = await response.Content.ReadAsStringAsync();
                        savedUsers = JsonConvert.DeserializeObject<UserModel>(result);
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

            return savedUsers;
        }

        public async Task<bool> SaveUserBasicInfo(UserModel userInfo)
        {
            bool flag = false;
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    HttpClient httpClient = new HttpClient();

                    var uri = new Uri(string.Concat(Constants.baseUrl, "/api/Helpers?id=" + userInfo.UserId));

                    var json = JsonConvert.SerializeObject(userInfo);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    var response = await httpClient.PutAsync(uri, content);
                    string result = await response.Content.ReadAsStringAsync();
                    var message = JsonConvert.DeserializeObject<ResponseModel>(result);

                    if (response.IsSuccessStatusCode)
                    {
                        result = await response.Content.ReadAsStringAsync();

                        flag = true;
                        //helpers = JsonConvert.DeserializeObject<HelperResponseModel>(result);
                        //helperService.HelperId = helpers.helperid;
                    }
                    else
                    {
                        flag = false;
                    }
                }
            }
            catch (Exception e)
            {
                Console.Write(e.StackTrace);
                flag = false;
            }

            return flag;
        }
        #endregion

        #region User Verification Service
        public async Task<VerificationInfoModel> GetVerificationInfo(int UserId)
        {
            VerificationInfoModel VerificationInfo = new VerificationInfoModel();

            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    HttpClient httpClient = new HttpClient();
                    var uri = new Uri(string.Concat(Constants.baseUrl, "api/editVerification?id=" + UserId));

                    var response = await httpClient.GetAsync(uri);
                    if (response.IsSuccessStatusCode)
                    {
                        string result = await response.Content.ReadAsStringAsync();
                        VerificationInfo = JsonConvert.DeserializeObject<VerificationInfoModel>(result);
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

            return VerificationInfo;
        }

        #endregion
    }
}
