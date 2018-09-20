using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Helpa.Services
{
    public class FacebookServices : IFacebookServices
    {
        public async Task<JObject> GetFacebookProfileAsync(string accessToken)
        {
            //Helper
            var requestUrl = "https://graph.facebook.com/v2.7/me/?fields=name,id,email,gender,picture&access_token=" + accessToken;
            var httpClient = new HttpClient();
            var userDetails = await httpClient.GetStringAsync(requestUrl);

            JObject detailsInJson = JObject.Parse(userDetails);

            return detailsInJson;
            
        }        
    }
}
