using Helpa.Models;
using Helpa.Services.Interface;
using Helpa.Utility;
using Newtonsoft.Json;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Helpa.Services.Class
{
    public class MyReviewServices : IMyReviewServices
    {
        public async Task<NetworkModel> GetReviewToReview(int UserId)
        {
            NetworkModel toReview = new NetworkModel();

            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    HttpClient httpClient = new HttpClient();
                    var uri = new Uri(string.Concat(Constants.baseUrl, "api/Favourates/ToReviewList?Id=" + UserId + "&PageNo=1"));

                    //var response = await httpClient.GetAsync(uri);

                    var requestTask = httpClient.GetAsync(uri);
                    var response = Task.Run(() => requestTask);

                    if (response.Result.IsSuccessStatusCode)
                    {
                        string result = await response.Result.Content.ReadAsStringAsync();
                        toReview = JsonConvert.DeserializeObject<NetworkModel>(result);
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

            return toReview;
        }

        public async Task<MyReviewModel> GetReviewAboutMe(int UserId)
        {
            MyReviewModel reviewAboutMe = new MyReviewModel();

            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    HttpClient httpClient = new HttpClient();
                    var uri = new Uri(string.Concat(Constants.baseUrl, "api/Favourates/GetReviewAboutMe?id=" + UserId + "&PageNo=1"));

                    //var response = await httpClient.GetAsync(uri);

                    var requestTask = httpClient.GetAsync(uri);
                    var response = Task.Run(() => requestTask);

                    if (response.Result.IsSuccessStatusCode)
                    {
                        string result = await response.Result.Content.ReadAsStringAsync();
                        reviewAboutMe = JsonConvert.DeserializeObject<MyReviewModel>(result);
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

            return reviewAboutMe;
        }
        public async Task<MyReviewModel> GetReviewByMe(int UserId)
        {
            MyReviewModel reviewByMe = new MyReviewModel();

            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    HttpClient httpClient = new HttpClient();
                    var uri = new Uri(string.Concat(Constants.baseUrl, "api/Favourates/GetReviewByMe?id=" + UserId + "&PageNo=1"));

                    //var response = await httpClient.GetAsync(uri);
                    
                    var requestTask = httpClient.GetAsync(uri);
                    var response = Task.Run(() => requestTask);
                    
                    if (response.Result.IsSuccessStatusCode)
                    {
                        string result = await response.Result.Content.ReadAsStringAsync();
                        reviewByMe = JsonConvert.DeserializeObject<MyReviewModel>(result);
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

            return reviewByMe;
        }
    }
}
