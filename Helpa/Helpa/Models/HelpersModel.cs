using System;
using System.Collections.Generic;
using System.Text;

namespace Helpa.Models
{
    public class HelpersModel
    {
        public string LocalityName { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int NumberOfHelpersInLocality { get; set; }
        public List<Helper> HelpersInLocalties { get; set; }
        
        //public string price { get; set; }
        //public string location { get; set; }
        //public string status { get; set; }
        //public string profileImage { get; set; }
        //public float rating { get; set; }
        //public string rating_count { get; set; }
        //public int count1 { get; set; }
        //public int count2 { get; set; }
    }

    public class Helper
    {
        public string Price { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public string ProfilePicture { get; set; }
        public string AverageRating { get; set; }
        public string RatingCount { get; set; }
        public string AverageRatingCount { get; set; }
        public string Service { get; set; }
        //public int ConnectionCount { get; set; }
        //public int EnquiredUserCount { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}