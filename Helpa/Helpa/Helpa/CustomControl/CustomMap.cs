using System.Collections.Generic;
using Xamarin.Forms.Maps;
using Xamarin.Forms;
using Helpa.Models;
using System;

namespace Helpa
{
    public class CustomMap : Map
    {
        //public List<string> SiteList;
        public HelperHomeModel selectedHelper;
        public JobsHomeModel selectedJob;

        public void ClickedOnPin(string selectedCluster)
        {
            try
            {
                MessagingCenter.Send<CustomMap, string>(this, "Hi", selectedCluster);
            }
            catch(Exception e)
            {
                Console.Write(e.StackTrace);
            }
        }
    }
}