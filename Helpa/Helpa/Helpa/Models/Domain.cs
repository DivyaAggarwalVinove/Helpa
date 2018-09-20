using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpa.Models
{
  public static class Domain
    {
        public static string Url
        {
          
            get
            {
                 return "http://180.151.232.92:127/";
               
            }
        }

        public static string GetProfileInfoApiConstant
        {
            get
            {
                return "api/GetBasic";
            }
        }
        public static string GetAvtarApiConstant
        {
            get
            {
                return "api/MobileApi/GetAvatarImage";
            }
        }
        public static string SaveAvtarApiConstant
        {
            get
            {
                return "api/MobileApi/SaveAvatarImage";
            }
        }
        public static string SubmitMoodApiConstant
        {
            get
            {
                return "api/MobileApi/SaveMood";
            }
        }
        public static string SaveBehaviourApiConstant
        {
            get
            {
                return "api/MobileApi/SaveBehavioralLog";
            }
        }
        public static string GetChildApiConstant
        {
            get
            {
                return "api/MobileApi/GetChild";
            }
        }
    }
}
