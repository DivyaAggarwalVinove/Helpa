using Helpa.iOS;
using Helpa.Services;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(PermissionServicesIos))]
namespace Helpa.iOS
{
    public class PermissionServicesIos : IPermissionServices
    {
        #region local variable
        public Page page;
        #endregion

        #region constructors
        public PermissionServicesIos()
        {

        }

        #endregion

        #region Requesting Runtime Location Permissions
        public async Task GetPermission(Page page)
        {
            //this.page = page;

            //if ((int)Build.VERSION.SdkInt < 23)
            //{
            //    await MainActivity.Instance.GetLocationAsync();
            //    return;
            //}

            //await MainActivity.Instance.GetLocationPermissionAsync(this);
        }
        #endregion
    }
}