using Android.OS;
using Helpa.Droid;
using Helpa.Services;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(PermissionServicesAndroid))]
namespace Helpa.Droid
{
    public class PermissionServicesAndroid : IPermissionServices
    {
        #region local variable
        public Page page;
        #endregion

        #region constructors
        public PermissionServicesAndroid()
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