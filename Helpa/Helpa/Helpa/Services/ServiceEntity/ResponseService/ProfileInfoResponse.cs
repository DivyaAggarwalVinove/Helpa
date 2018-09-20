using System;
using System.Collections.Generic;
using System.Text;

namespace Helpa.Services.ServiceEntity.ResponseService
{
  public class ProfileInfoResponse 
    {
        public string PhoneNumber { get; set; }
        public string UserName { get; set; }
        public string LocationName { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public string selfintroduction { get; set; }
        public string ProfileImage { get; set; }
        public List<string> Carousel { get; set; }
    }
}
