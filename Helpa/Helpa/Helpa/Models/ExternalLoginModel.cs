using System;
using System.Collections.Generic;
using System.Text;

namespace Helpa.Models
{
    public class ExternalModel
    {
        public string id { get; set; }
        public string email { get; set; }
        public string idToken { get; set; }
        public string provider { get; set; }
        public string name { get; set; }
        public string PhoneNumber { get; set; }
        public string LoginProvider { get; set; }
        public string Role { get; set; }
    }
}
