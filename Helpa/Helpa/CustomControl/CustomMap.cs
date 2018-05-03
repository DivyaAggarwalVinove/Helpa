using System.Collections.Generic;
using Xamarin.Forms.Maps;
using Xamarin.Forms;

namespace Helpa
{
    public class CustomMap : Map
    {
        //public List<string> SiteList;
        public HelpersModel selectedHelper;

        public void ClickedOnPin()
        {
            MessagingCenter.Send<CustomMap>(this, "Hi");
        }
    }
}