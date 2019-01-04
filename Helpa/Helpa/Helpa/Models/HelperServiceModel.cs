using System;
using System.Collections.Generic;
using System.Text;

namespace Helpa.Models
{
    public class HelperServiceModel
    {
        public int UserId { get; set; }

        public int HelperId { get; set; }

        public bool Status { get; set; }

        public string Qualification { get; set; }

        public int ExperienceYears { get; set; }

        public int MinAge { get; set; }

        public int MaxAge { get; set; }
        public int ServiceCount { get; set; }
        public List<HelperServices> Service { get; set; }
    }
    
    public class HelperServices
    {
        public int ServiceId { get; set; }

        public string ServiceName { get; set; }

        public string LocationType { get; set; }

        public string LocationName { get; set; }

        public string Latitude { get; set; }

        public string Longitude { get; set; }

        public bool Hour { get; set; }

        public float MinPriceHour { get; set; }

        public float MaxPriceHour { get; set; }

        public bool Day { get; set; }

        public float MinDayPrice { get; set; }

        public float MaxDayPrice { get; set; }

        public bool Month { get; set; }

        public float MinMonthPrice { get; set; }

        public float MaxMonthPrice { get; set; }

        public List<HelperScopes> Scopes { get; set; }
    }

    public class HelperScopes
    {
        public int ScopeId { get; set; }
    }
}
