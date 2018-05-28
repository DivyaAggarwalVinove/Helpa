using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Helpa.Services
{
    public interface IFacebookServices
    {
         Task<JObject> GetFacebookProfileAsync(string accessToken);
    }
}
