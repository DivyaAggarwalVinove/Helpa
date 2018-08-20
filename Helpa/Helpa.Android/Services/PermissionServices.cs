using Android;
using Android.OS;
using Android.Support.Design.Widget;
using Helpa.Droid;
using Helpa.Services;
using Plugin.Geolocator;
using System;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(PermissionServices))]
namespace Helpa.Droid
{
    public class PermissionServices : IPermissionServices
    {
        #region local variable
        public Page page;
        #endregion

        #region constructors
        public PermissionServices()
        {

        }

        #endregion

        #region Requesting Runtime Location Permissions
        public async Task GetPermission(Page page)
        {
            this.page = page;

            if ((int)Build.VERSION.SdkInt < 23)
            {
                await MainActivity.Instance.GetLocationAsync();
                return;
            }

            await MainActivity.Instance.GetLocationPermissionAsync(this);
        }
        #endregion
    }
}