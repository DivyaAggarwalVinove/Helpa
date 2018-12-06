using Helpa.Models;
using Helpa.Utility;
using Helpa.Views.Profile;
using Helpa.Views.Profile.EditProfile;
using Newtonsoft.Json;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Helpa.Services
{
    /// <summary>
    /// RegisterServices: Define registered services.
    /// </summary>
    public class RegisterServices : IRegisterServices
    {
        #region Custom Signup
        /// <summary>
        /// RegisterService: Register user with basic info
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
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
                if (userModel.Role.Equals("HELPER"))
                    HelperRegister.Instance.ShowError(e.StackTrace);
                else
                    Register.Instance.ShowError(e.StackTrace);

                Console.Write(e.StackTrace);
            }
        }

        /// <summary>
        /// CompleteRegisterService: Save all other info of registered user
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
        public async Task CompleteRegisterService(RegisterUserModel userModel)
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    HttpClient httpClient = new HttpClient();

                    var uri = new Uri(string.Concat(Constants.baseUrl, "api/Account/complete-registration/" + userModel.Id));
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
        #endregion

        #region External Sign in
        /// <summary>
        /// RegisterExternal: External Signup i.e, Facebook & Google
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
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
        #endregion

        #region Other Utlity Register Api
        /// <summary>
        /// SendSmsCode: Send sms code to verify mobile no.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="mobileno"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        public async Task SendSmsCode(int userId, string mobileno, string role)
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    HttpClient httpClient = new HttpClient();

                    var uri = new Uri(string.Concat(Constants.baseUrl, "api/Account/PhoneVerification?PhoneNo=" + mobileno + "&UserId=" + userId));
                    var json = JsonConvert.SerializeObject(new { user = userId });
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    var response = await httpClient.PutAsync(uri, content);
                    string result = await response.Content.ReadAsStringAsync();
                    var message = JsonConvert.DeserializeObject<ResponseModel>(result);

                    if (role.Equals("HELPER"))
                        HelperCompleteRegister.Instance.ShowSmsMessage(message, response.IsSuccessStatusCode);
                    else if (role.Equals("PARENT"))
                    {
                        Register1.Instance.ShowSmsMessage(message, response.IsSuccessStatusCode);
                    }
                    else if (role.Equals("VERIFICATION"))
                    {
                        EditVerificationPage.Instance.ShowSmsMessage(message, response.IsSuccessStatusCode);
                    }
                    else if (role.Equals("EDITINFO"))
                    {
                        EditBasicInfo.Instance.ShowSmsMessage(message, response.IsSuccessStatusCode);
                    }
                }
            }
            catch (Exception e)
            {
                Console.Write(e.StackTrace);
            }
        }

        /// <summary>
        /// VerifyOtp: Verify OTP to verify mobile
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="otp"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        public async Task VerifyOtp(int userId, string otp, string role)
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    HttpClient httpClient = new HttpClient();

                    var uri = new Uri(string.Concat(Constants.baseUrl, "api/Account/PhoneNumberVerificationComplete?UserId=" + userId + "&OTP=" + otp));
                    var json = JsonConvert.SerializeObject(new { user = userId });
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    var response = await httpClient.PutAsync(uri, content);
                    string result = await response.Content.ReadAsStringAsync();
                    var message = JsonConvert.DeserializeObject<ResponseModel>(result);
                    //if (response.IsSuccessStatusCode)
                    //{
                    if (role.Equals("HELPER"))
                        HelperCompleteRegister.Instance.ShowVerificationMessage(message, response.IsSuccessStatusCode);
                    else if (role.Equals("PARENT"))
                    {
                        Register1.Instance.ShowVerificationMessage(message, response.IsSuccessStatusCode);
                    }
                    else if(role.Equals("VERIFICATION"))
                    {
                        EditVerificationPage.Instance.ShowVerificationMessage(message, response.IsSuccessStatusCode);
                    }
                    else if(role.Equals("EDITINFO"))
                    {
                        EditBasicInfo.Instance.ShowVerificationMessage(message, response.IsSuccessStatusCode);
                    }
                }
            }
            catch (Exception e)
            {
                Console.Write(e.StackTrace);
            }
        }

        /// <summary>
        /// SendLink: Send a link to reset password
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<ResponseModel> SendLink(string email)
        {
            ResponseModel message = new ResponseModel();
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    HttpClient httpClient = new HttpClient();

                    var uri = new Uri(string.Concat(Constants.baseUrl, "api/Account/ForgetPasword?EmailId=" + email));
                    
                    var response = await httpClient.GetAsync(uri);
                    string result = await response.Content.ReadAsStringAsync();
                    message = JsonConvert.DeserializeObject<ResponseModel>(result);
                }
            }
            catch (Exception e)
            {
                Console.Write(e.StackTrace);
                message.Message = e.StackTrace;
            }
            
            return message;
        }
        
        public async Task<ResponseModel> ResetPassword(int userId, string oldpwd, string newpwd)
        {
            ResponseModel message = new ResponseModel();
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    HttpClient httpClient = new HttpClient();

                    var uri = new Uri(string.Concat(Constants.baseUrl, "api/Account/ChangePassword"));
                    var json = JsonConvert.SerializeObject(new { OldPassword = oldpwd, NewPassword = newpwd, userId = userId });
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    var response = await httpClient.PostAsync(uri, content);
                    string result = await response.Content.ReadAsStringAsync();
                    message = JsonConvert.DeserializeObject<ResponseModel>(result);
                }
            }
            catch (Exception e)
            {
                Console.Write(e.StackTrace);
                message.Message = e.StackTrace;
            }

            return message;
        }

        public async Task EmailVerify(string Email)
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    HttpClient httpClient = new HttpClient();

                    var uri = new Uri(string.Concat(Constants.baseUrl, "api/Account/EmailVerification?EmailId=" + Email));
                    var json = JsonConvert.SerializeObject(new { user = Email });
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    var response = await httpClient.PutAsync(uri, content);
                    string result = await response.Content.ReadAsStringAsync();
                    var message = JsonConvert.DeserializeObject<ResponseModel>(result);
                    EditVerificationPage.Instance.ShowVerificationMessage(message, response.IsSuccessStatusCode);
                }
            }
            catch (Exception e)
            {
                Console.Write(e.StackTrace);
            }
        }
        #endregion
    }
}