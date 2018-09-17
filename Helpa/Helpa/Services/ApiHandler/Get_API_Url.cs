using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpa.Services.ApiHandler
{
    public class Get_API_Url
    {
        public string CommonBaseApi(string BaseUsrl)
        {
            return BaseUsrl;
        }
        /// <summary>
        /// for Login Users
        /// </summary>
        /// <param name="BaseUsrl"></param>
        /// <returns>BaseUsrl</returns>
        public string GetProfileInfoApi(string BaseUsrl , int userID)
        {          
            return string.Format("{0}?id={1}", BaseUsrl, userID);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="BaseUsrl"></param>
        /// <param name="SurveyId"></param>
        /// <returns></returns>
        public string GetAnswerBySurveryIdApi(string BaseUsrl, long SurveyId)
        {
            return string.Format("{0}{1}", BaseUsrl, SurveyId);
        }

    }
}
