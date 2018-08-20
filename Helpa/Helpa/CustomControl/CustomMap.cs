using System.Collections.Generic;
using Xamarin.Forms.Maps;
using Xamarin.Forms;
using Helpa.Models;

namespace Helpa
{
    public class CustomMap : Map
    {
        //public List<string> SiteList;
        public HelperHomeModel selectedHelper;

        public void ClickedOnPin(string selectedCluster)
        {
            MessagingCenter.Send<CustomMap, string>(this, "Hi", selectedCluster);
        }
    }
}