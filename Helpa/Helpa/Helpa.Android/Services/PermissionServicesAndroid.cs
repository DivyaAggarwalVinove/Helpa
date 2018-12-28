using Android.Gms.Common.Apis;
using Android.Gms.Location;
using Android.Locations;
using Android.OS;
using Helpa.Droid;
using Helpa.Services;
using System;
using System.Runtime.Remoting.Contexts;
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

            await MainActivity.Instance.GetLocationPermissionAsync(this, page);
        }
        #endregion

        #region Turned on GPS in Device setting programmatically
        public async Task CheckAndTurnOnGPS(Page page)
        {
            this.page = page;

            Int64 interval = 1000 * 60 * 1, fastestInterval = 1000 * 50;

            try
            {
                GoogleApiClient
                    googleApiClient = new GoogleApiClient.Builder(MainActivity.Instance)
                        .AddApi(LocationServices.API)
                        .Build();

                googleApiClient.Connect();

                LocationRequest
                    locationRequest = LocationRequest.Create()
                        .SetPriority(LocationRequest.PriorityBalancedPowerAccuracy)
                        .SetInterval(interval)
                        .SetFastestInterval(fastestInterval);

                LocationSettingsRequest.Builder
                    locationSettingsRequestBuilder = new LocationSettingsRequest.Builder()
                        .AddLocationRequest(locationRequest);

                locationSettingsRequestBuilder.SetAlwaysShow(false);

                LocationSettingsResult
                    locationSettingsResult = await LocationServices.SettingsApi.CheckLocationSettingsAsync(
                        googleApiClient, locationSettingsRequestBuilder.Build());

                if (locationSettingsResult.Status.StatusCode == LocationSettingsStatusCodes.ResolutionRequired)
                {
                    locationSettingsResult.Status.StartResolutionForResult(MainActivity.Instance, 0);
                }
            }
            catch (Exception exception)
            {
                Console.Write(exception.Message);
            }

            /*(if ((int)Build.VERSION.SdkInt < 23)
            {
                await MainActivity.Instance.GetLocationAsync();
                return;
            }

            await MainActivity.Instance.GetLocationPermissionAsync(this);*/
        }
        #endregion
    }
}