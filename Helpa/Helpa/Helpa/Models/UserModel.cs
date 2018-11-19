using System;
using System.Collections.Generic;

namespace Helpa.Models
{
    public class UserModel
    {
        public string UserName { get; set; }
        public int UserId { get; set; }
        public string ProfileImage { get; set; }
        public List<string> Carousel { get; set; }
        public int Gender { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public string LocationName { get; set; }
        public string Locality { get; set; }
        public string Distict { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string selfintroduction { get; set; }
        public bool PhoneVerification { get; set; }
    }
}
