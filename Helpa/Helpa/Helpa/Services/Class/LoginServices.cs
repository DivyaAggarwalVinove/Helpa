
using Helpa.Models;
using Helpa.Utility;
using Newtonsoft.Json;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
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
                        if (message != null && !message.id.Equals(""))
                        {
                            message.userId = int.Parse(message.id);
                        }

                        RegisterUserModel user = App.Database.GetUsers(message.userId);
                        if (user == null)
                            user = new RegisterUserModel();

                        user.Id = message.userId;

                        if (message != null && !message.HelperId.Equals(""))
                        {
                            user.HelperId = int.Parse(message.HelperId);
                        }

                        user.UserName = message.userName;
                        user.Token = message.access_token;
                        user.isLoggedIn = true;
                        user.Role = message.roles;
                        user.Email = email;
                        user.profileImage = message.profileImage;
                        App.Database.SaveUser(user);

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

        public async Task<bool> ExternalLogin(ExternalUserModel userModel)
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    HttpClient httpClient = new HttpClient();

                    var uri = new Uri(string.Concat(Constants.baseUrl, "/api/Account/GoogleExternalSignUp"));
                    var json = JsonConvert.SerializeObject(userModel);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    // var response = await httpClient.PostAsync(uri, content);

                    var requestTask = httpClient.PostAsync(uri, content);
                    var response = Task.Run(() => requestTask);

                    string result = await response.Result.Content.ReadAsStringAsync();
                    var message = JsonConvert.DeserializeObject<ExternalLoginResponseModel>(result);

                    if (response.Result.IsSuccessStatusCode)
                    {
                        RegisterUserModel rum = new RegisterUserModel();

                        rum.Id = int.Parse(message.id);
                        rum.UserName = message.userName;
                        rum.Email = message.email;
                        rum.LoginProvider = userModel.LoginProvider;
                        rum.Role = message.roles;
                        rum.profileImage = message.profileImage;

                        rum.isLoggedIn = true;

                        App.Database.SaveUser(rum);

                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception e)
            {
                Console.Write(e.StackTrace.ToString());
                return false;
            }

            return false;
        }

        #region User Basic Info Services 
        public async Task<UserModel> GetUserBasicInfo(int UserId)
        {
            UserModel userInfo = new UserModel();

            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    HttpClient httpClient = new HttpClient();
                    var uri = new Uri(string.Concat(Constants.baseUrl, "api/GetBasic?id=" + UserId));

                    //var response = await httpClient.GetAsync(uri);

                    var requestTask = httpClient.GetAsync(uri);
                    var response = Task.Run(() => requestTask);

                    if (response.Result.IsSuccessStatusCode)
                    {
                        string result = await response.Result.Content.ReadAsStringAsync();
                        userInfo = JsonConvert.DeserializeObject<UserModel>(result);


                        RegisterUserModel rum = App.Database.GetLoggedUser();

                        rum.Id = UserId;
                        rum.UserName = userInfo.UserName;
                        rum.profileImage = userInfo.ProfileImage;
                        rum.Role = userInfo.Role;
                        rum.Gender = userInfo.Gender;
                        rum.MobileNumber = userInfo.MobileNumber;
                        rum.locationName = userInfo.LocationName;
                        rum.latitude = userInfo.Latitude;
                        rum.longitude = userInfo.Longitude;
                        rum.description = userInfo.selfintroduction;

                        App.Database.SaveUser(rum);
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

            return userInfo;
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
                    httpClient.Timeout = Timeout.InfiniteTimeSpan;

                    var tokenSource = new CancellationTokenSource();
                    tokenSource.CancelAfter(TimeSpan.FromMilliseconds(Timeout.Infinite));

                    var response = await httpClient.PutAsync(uri, content, tokenSource.Token);

                    //var requestTask = httpClient.PutAsync(uri, content, tokenSource.Token);
                    //var response = Task.Run(() => requestTask);

                    //string result = await response.Content.ReadAsStringAsync();
                    //var message = JsonConvert.DeserializeObject<ResponseModel>(result);

                    if (response.IsSuccessStatusCode)
                    {
                        string result = await response.Content.ReadAsStringAsync();

                        flag = true;
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

                    //var response = await httpClient.GetAsync(uri);

                    var requestTask = httpClient.GetAsync(uri);
                    var response = Task.Run(() => requestTask);

                    if (response.Result.IsSuccessStatusCode)
                    {
                        string result = await response.Result.Content.ReadAsStringAsync();
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

        public async Task<bool> UploadCertificate(int hid, string cert)
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    HttpClient httpClient = new HttpClient();

                    var uri = new Uri(string.Concat(Constants.baseUrl, "api/PostCertificates"));

                    var json = JsonConvert.SerializeObject(new { HelperId = hid, certificate = cert });
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    //var response = await httpClient.PostAsync(uri, content);

                    var requestTask = httpClient.PostAsync(uri, content);
                    var response = Task.Run(() => requestTask);

                    string result = await response.Result.Content.ReadAsStringAsync();
                    var message = JsonConvert.DeserializeObject<ResponseModel>(result);
                    
                    if (response.Result.IsSuccessStatusCode)
                    {
                        result = await response.Result.Content.ReadAsStringAsync();
                        return true;
                        //helpers = JsonConvert.DeserializeObject<HelperResponseModel>(result);
                        //helperService.HelperId = helpers.helperid;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception e)
            {
                Console.Write(e.StackTrace);
                return false;
            }

            return false;
        }

        public async Task<bool> UploadID(int hid, string cert)
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    HttpClient httpClient = new HttpClient();

                    var uri = new Uri(string.Concat(Constants.baseUrl, "api/PostIdentity"));

                    var json = JsonConvert.SerializeObject(new { HelperId = hid, certificate = cert });
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    
                    var requestTask = httpClient.PostAsync(uri, content);
                    var response = Task.Run(() => requestTask);

                    string result = await response.Result.Content.ReadAsStringAsync();
                    var message = JsonConvert.DeserializeObject<ResponseModel>(result);

                    if (response.Result.IsSuccessStatusCode)
                    {
                        result = await response.Result.Content.ReadAsStringAsync();
                        return true;
                        //helpers = JsonConvert.DeserializeObject<HelperResponseModel>(result);
                        //helperService.HelperId = helpers.helperid;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception e)
            {
                Console.Write(e.StackTrace);
                return false;
            }

            return false;
        }
        #endregion
    }
}
