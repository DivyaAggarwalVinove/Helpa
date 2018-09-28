using Helpa.iOS;
using Helpa.Services;
using System.Threading.Tasks;
using Xamarin.Forms;
using CoreLocation;

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
            /*
            if(!(CLLocationManager.LocationServicesEnabled))
            {

            } else
            {
                var status = CLLocationManager.Status;
            }
            */

            /*
            CLLocationManage;

            self.locationManager = [[CLLocationManager alloc] init];
            self.locationManager.delegate = self;
            self.locationManager.distanceFilter = 10.0f;
            self.locationManager.desiredAccuracy = kCLLocationAccuracyBest;
            [self.locationManager startUpdatingLocation];
            self.locations = [NSMutableArray arrayWithCapacity:32];

*/

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