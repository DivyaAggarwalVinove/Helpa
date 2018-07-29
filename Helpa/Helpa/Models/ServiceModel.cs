using SQLite;
using System.Collections.Generic;

namespace Helpa.Models
{
    public class ServiceModel
    {
        [PrimaryKey]
        public string Id { get; set; }
        public string ServiceName { get; set; }
        public bool isSelected { get; set; }

        public int LocationType { get; set; }
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

        //[Ignore]
        //public List<ScopeModel> Scopes { get; set; }
    }
}