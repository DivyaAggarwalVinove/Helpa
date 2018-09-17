
using Helpa.Services.ServiceEntity;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Helpa.Services.ApiHandler
{
    public class RestApi
    {
        public WebRequest webRequest = null;
        private HttpClient client;
        private string _col = ":";


        public RestApi()

        {
            client = new HttpClient();
        }
        //*****************Get Generic Api's**************************
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="uri"></param>
        /// <param name="IsHeaderRequired"></param>
        /// <param name="objHeaderModel"></param>
        /// <param name="Tobject"></param>
        /// <returns></returns>
        public async Task<T> GetAsyncData_GetApi<T>(string uri, Boolean IsHeaderRequired, HeaderModel objHeaderModel, T Tobject) where T : new()
        {
            // var _storedToken=Settings;
            try
            {
                client.MaxResponseContentBufferSize = 256000;
                if (IsHeaderRequired)
                {

                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", objHeaderModel.TokenCode);
                    //client.DefaultRequestHeaders.Add("Content-Type", "application/json");
                    //client.DefaultRequestHeaders.Add("Authorization", "application/json");
                    //client.DefaultRequestHeaders.Add("Content-Length", "84");
                    //client.DefaultRequestHeaders.Add("User-Agent", "Fiddler");
                    //client.DefaultRequestHeaders.Add("Host", "localhost:49165");
                    // client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                }
                // var request = new HttpRequestMessage(HttpMethod.Get, uri);

                // request.Content = new FormUrlEncodedContent(keyValues);

                //  HttpResponseMessage response = await client.SendAsync(request);

                HttpResponseMessage response = await client.GetAsync(uri);


                if (response.IsSuccessStatusCode)
                {

                    var responseContent = response.Content;
                    var SucessResponse = await response.Content.ReadAsStringAsync();
                    //  SucessResponse = SucessResponse.Insert(1, "\"Status\"" + _col + "\"Success\",");
                    Tobject = JsonConvert.DeserializeObject<T>(SucessResponse);
                    return Tobject;

                }
                else
                {

                    //long ResonseStatus = Convert.ToInt64(response.StatusCode);
                    //switch (ResonseStatus)
                    //{
                    //    case 302:
                    //        _response = "{\"Status\"" + _col + "\"Invalid User Name and password..\"}";
                    //        break;
                    //    case 400:
                    //        _response = "{\"Status\"" + _col + "\"Bad Request\"}";
                    //        break;
                    //    case 401:
                    //        _response = "{\"Status\"" + _col + "\"Invalid User Name and password..\"}";
                    //        break;
                    //    case 404:
                    //        _response = "{\"Status\"" + _col + "\"Not Found\"}";
                    //        break;

                    //    default:
                    //        _response = "{\"Status\"" + _col + "\"Internal Server errror\"}";
                    //        break;

                    var responseContent = response.Content;
                    var ErrorResponse = await response.Content.ReadAsStringAsync();
                    ErrorResponse = ErrorResponse.Insert(1, "\"Status\"" + _col + "\"fail\",");
                    Tobject = JsonConvert.DeserializeObject<T>(ErrorResponse);
                    return Tobject;

                    // }
                }

                //Tobject = JsonConvert.DeserializeObject<T>(_response);
                //return Tobject;
            }
            catch (Exception ex)
            {

                throw;

            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="uri"></param>
        /// <param name="IsHeaderRequired"></param>
        /// <param name="objHeaderModel"></param>
        /// <param name="Tobject"></param>
        /// <returns></returns>
        public async Task<ObservableCollection<T>> GetAsyncData_GetApiList<T>(string uri, Boolean IsHeaderRequired, HeaderModel objHeaderModel, ObservableCollection<T> Tobject) where T : new()
        {
            // var _storedToken=Settings;
            try
            {
                HttpResponseMessage response = null;
                //  client.MaxResponseContentBufferSize = 256000;
                if (IsHeaderRequired)
                {

                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", objHeaderModel.TokenCode);

                }


                response = await client.GetAsync(uri);


                if (response.IsSuccessStatusCode)
                {

                    var responseContent = response.Content;
                    var SucessResponse = await response.Content.ReadAsStringAsync();
                    //  SucessResponse = SucessResponse.Insert(1, "\"Status\"" + _col + "\"Success\",");
                    Tobject = JsonConvert.DeserializeObject<ObservableCollection<T>>(SucessResponse);
                    return Tobject;

                }
                else
                {





                    var responseContent = response.Content;
                    var ErrorResponse = await response.Content.ReadAsStringAsync();
                    //  ErrorResponse = ErrorResponse.Insert(1, "\"Status\"" + _col + "\"fail\",");
                    Tobject = JsonConvert.DeserializeObject<ObservableCollection<T>>(ErrorResponse);
                    return Tobject;


                }

            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public async Task<ObservableCollection<T>> GetAsyncData_GetCountryApiList<T>(string uri, Boolean IsHeaderRequired, HeaderModel objHeaderModel, ObservableCollection<T> Tobject) where T : new()
        {

            // var _storedToken=Settings;
            try
            {
                // HttpResponseMessage response = null;
                //  client.MaxResponseContentBufferSize = 256000;
                if (IsHeaderRequired)
                {

                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", objHeaderModel.TokenCode);

                }

                using (var w = new WebClient())
                {
                    var json_data = string.Empty;
                    // attempt to download JSON data as a string
                    try
                    {
                        //json_data =  w.DownloadString(uri);
                        json_data = await w.DownloadStringTaskAsync(uri);
                    }
                    catch (Exception)
                    {

                    }
                    // if string with JSON data is not empty, deserialize it to class and return its instance 
                    //if (!string.IsNullOrEmpty(json_data))
                    //{
                    //    return JsonConvert.DeserializeObject<ObservableCollection<T>>(json_data);
                    //}
                    //else
                    //{
                    //    return null;
                    //}
                    //  return !string.IsNullOrEmpty(json_data) ? JsonConvert.DeserializeObject<ObservableCollection<T>>(json_data) : new T();

                    //  response = await client.GetAsync(uri);


                    if (!string.IsNullOrEmpty(json_data))
                    {

                        // var responseContent = response.Content;
                        // var SucessResponse = await response.Content.ReadAsStringAsync();
                        //  SucessResponse = SucessResponse.Insert(1, "\"Status\"" + _col + "\"Success\",");
                        var res = JsonConvert.DeserializeObject<ObservableCollection<T>>(json_data);
                        return res;

                    }
                    else
                    {





                        // var responseContent = response.Content;
                        // var ErrorResponse = await response.Content.ReadAsStringAsync();
                        //  ErrorResponse = ErrorResponse.Insert(1, "\"Status\"" + _col + "\"fail\",");
                        // Tobject = JsonConvert.DeserializeObject<ObservableCollection<T>>(json_data);
                        return null;


                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }


        //*********************************POST APIS**************************

        /// <summary>
        /// Login Api
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="IsHeaderRequired"></param>
        /// <param name="objHeaderModel"></param>
        /// <param name="_objLoginRequest"></param>
        /// <returns></returns>
        //public async Task<LoginResponse> LoginAsync(string uri, Boolean IsHeaderRequired, HeaderModel objHeaderModel, LoginRequest _objLoginRequest)
        //{

        //    client.MaxResponseContentBufferSize = 256000;
        //    LoginResponse _objLoginResponse = new LoginResponse();

        //    var keyValues = new List<KeyValuePair<string, string>>
        //    {
        //        new KeyValuePair<string, string>("UserId",_objLoginRequest.UserId),
        //        new KeyValuePair<string, string>("Password",_objLoginRequest.Password)

        //    };
        //    if (IsHeaderRequired)
        //    {
        //        // client.DefaultRequestHeaders.Add("Content-Type", "application/json");
        //        client.DefaultRequestHeaders.Add("UserId", _objLoginRequest.UserId);
        //        client.DefaultRequestHeaders.Add("Password", _objLoginRequest.Password);

        //    }
        //    var request = new HttpRequestMessage(HttpMethod.Post, uri);

        //    request.Content = new FormUrlEncodedContent(keyValues);
        //    HttpResponseMessage response = await client.SendAsync(request);
        //    // HttpResponseMessage response = await client.GetAsync(uri);
        //    var statuscode = Convert.ToInt64(response.StatusCode);
        //    if (response.IsSuccessStatusCode)
        //    {
        //        // ResponseStatus.StatusCode = Convert.ToInt32(response.StatusCode);
        //        var SucessResponse = await response.Content.ReadAsStringAsync();
        //        _objLoginResponse = JsonConvert.DeserializeObject<LoginResponse>(SucessResponse);
        //        return _objLoginResponse;
        //    }
        //    else
        //    {
        //        //  ResponseStatus.StatusCode = Convert.ToInt32(response.StatusCode);
        //        var ErrorResponse = await response.Content.ReadAsStringAsync();
        //        _objLoginResponse = JsonConvert.DeserializeObject<LoginResponse>(ErrorResponse);
        //        return _objLoginResponse;
        //    }

        //}


        /// <summary>
        /// 
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="IsHeaderRequired"></param>
        /// <param name="objHeaderModel"></param>
        /// <param name="_objAvatarRequestModel"></param>
        /// <returns></returns>
        //public async Task<AvatarResponseModel> GetAvatarAsync(string uri, Boolean IsHeaderRequired, HeaderModel objHeaderModel, AvatarRequestModel _objAvatarRequestModel)
        //{


        //    AvatarResponseModel objAvatarResponseModel;
        //    string s = JsonConvert.SerializeObject(_objAvatarRequestModel);
        //    HttpResponseMessage response = null;
        //    using (var stringContent = new StringContent(s, System.Text.Encoding.UTF8, "application/json"))
        //    {

        //        if (IsHeaderRequired)
        //        {
        //            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(objHeaderModel.TokenCode);

        //        }
        //        response = await client.PostAsync(uri, stringContent);


        //        if (response.IsSuccessStatusCode)
        //        {
        //            var SucessResponse = await response.Content.ReadAsStringAsync();
        //            objAvatarResponseModel = JsonConvert.DeserializeObject<AvatarResponseModel>(SucessResponse);
        //            return objAvatarResponseModel;
        //        }
        //        else
        //        {
        //            var ErrorResponse = await response.Content.ReadAsStringAsync();
        //            objAvatarResponseModel = JsonConvert.DeserializeObject<AvatarResponseModel>(ErrorResponse);
        //            return objAvatarResponseModel;
        //        }
        //    }

        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="uri"></param>
        ///// <param name="IsHeaderRequired"></param>
        ///// <param name="objHeaderModel"></param>
        ///// <param name="_objSaveAvtarRequestModel"></param>
        ///// <returns></returns>
        //public async Task<SaveAvtarResponseModel> SaveAvatarAsync(string uri, Boolean IsHeaderRequired, HeaderModel objHeaderModel, SaveAvtarRequestModel _objSaveAvtarRequestModel)
        //{

        //    SaveAvtarResponseModel objSaveAvtarResponseModel;
        //    string s = JsonConvert.SerializeObject(_objSaveAvtarRequestModel);
        //    HttpResponseMessage response = null;
        //    using (var stringContent = new StringContent(s, System.Text.Encoding.UTF8, "application/json"))
        //    {

        //        if (IsHeaderRequired)
        //        {
        //            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(objHeaderModel.TokenCode);

        //        }
        //        response = await client.PostAsync(uri, stringContent);


        //        if (response.IsSuccessStatusCode)
        //        {
        //            var SucessResponse = await response.Content.ReadAsStringAsync();
        //            objSaveAvtarResponseModel = JsonConvert.DeserializeObject<SaveAvtarResponseModel>(SucessResponse);
        //            return objSaveAvtarResponseModel;
        //        }
        //        else
        //        {
        //            var ErrorResponse = await response.Content.ReadAsStringAsync();
        //            objSaveAvtarResponseModel = JsonConvert.DeserializeObject<SaveAvtarResponseModel>(ErrorResponse);
        //            return objSaveAvtarResponseModel;
        //        }
        //    }
        //}


        //public async Task<SubmitMoodResponseModel> SubmitMoodAsync(string uri, Boolean IsHeaderRequired, HeaderModel objHeaderModel, SubmitMoodRequestModel _objSubmitMoodRequestModel)
        //{

        //    SubmitMoodResponseModel objSubmitMoodResponseModel;
        //    string s = JsonConvert.SerializeObject(_objSubmitMoodRequestModel);
        //    HttpResponseMessage response = null;
        //    using (var stringContent = new StringContent(s, System.Text.Encoding.UTF8, "application/json"))
        //    {

        //        if (IsHeaderRequired)
        //        {
        //            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(objHeaderModel.TokenCode);

        //        }
        //        response = await client.PostAsync(uri, stringContent);


        //        if (response.IsSuccessStatusCode)
        //        {
        //            var SucessResponse = await response.Content.ReadAsStringAsync();
        //            objSubmitMoodResponseModel = JsonConvert.DeserializeObject<SubmitMoodResponseModel>(SucessResponse);
        //            return objSubmitMoodResponseModel;
        //        }
        //        else
        //        {
        //            var ErrorResponse = await response.Content.ReadAsStringAsync();
        //            objSubmitMoodResponseModel = JsonConvert.DeserializeObject<SubmitMoodResponseModel>(ErrorResponse);
        //            return objSubmitMoodResponseModel;
        //        }
        //    }
        //}

        //public async Task<BehaviourLogResponse> SubmitBehaviourAsync(string uri, Boolean IsHeaderRequired, HeaderModel objHeaderModel, BehaviourLogRequest _objBehaviourLogRequest)
        //{

        //    BehaviourLogResponse objBehaviourLogResponse;
        //    string s = JsonConvert.SerializeObject(_objBehaviourLogRequest);
        //    HttpResponseMessage response = null;
        //    using (var stringContent = new StringContent(s, System.Text.Encoding.UTF8, "application/json"))
        //    {

        //        if (IsHeaderRequired)
        //        {
        //            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(objHeaderModel.TokenCode);

        //        }
        //        response = await client.PostAsync(uri, stringContent);


        //        if (response.IsSuccessStatusCode)
        //        {
        //            var SucessResponse = await response.Content.ReadAsStringAsync();
        //            objBehaviourLogResponse = JsonConvert.DeserializeObject<BehaviourLogResponse>(SucessResponse);
        //            return objBehaviourLogResponse;
        //        }
        //        else
        //        {
        //            var ErrorResponse = await response.Content.ReadAsStringAsync();
        //            objBehaviourLogResponse = JsonConvert.DeserializeObject<BehaviourLogResponse>(ErrorResponse);
        //            return objBehaviourLogResponse;
        //        }
        //    }
        //}


        /// <summary>
        /// 
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public async Task<string> PostAsyncData(string uri)
        {
            try
            {

                HttpClient httpClient = new HttpClient();
                HttpResponseMessage response = await httpClient.GetAsync(uri);
                return await response.Content.ReadAsStringAsync(); ;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        public void FetchData(string url)
        {

            // Create an HTTP web request using the URL:
            webRequest = WebRequest.Create(new Uri(url));
            webRequest.ContentType = "application/json";
            webRequest.Method = "GET";
            webRequest.BeginGetRequestStream(new AsyncCallback(RequestStreamCallback), webRequest);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="asynchronousResult"></param>
        private void RequestStreamCallback(IAsyncResult asynchronousResult)
        {
            webRequest = (HttpWebRequest)asynchronousResult.AsyncState;

            using (var postStream = webRequest.EndGetRequestStream(asynchronousResult))
            {
                //send yoour data here
            }
            webRequest.BeginGetResponse(new AsyncCallback(ResponseCallback), webRequest);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="asynchronousResult"></param>
        void ResponseCallback(IAsyncResult asynchronousResult)
        {
            HttpWebRequest myrequest = (HttpWebRequest)asynchronousResult.AsyncState;
            using (HttpWebResponse response = (HttpWebResponse)myrequest.EndGetResponse(asynchronousResult))
            {
                using (System.IO.Stream responseStream = response.GetResponseStream())
                {
                    using (var reader = new System.IO.StreamReader(responseStream))
                    {
                        var data = reader.ReadToEnd();
                    }
                }
            }
        }
    }
}
