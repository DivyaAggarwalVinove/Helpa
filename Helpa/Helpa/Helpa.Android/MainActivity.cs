using System;
using System.IO;
using System.Threading.Tasks;
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Util;
using Android.Widget;
using AsNum.XFControls.Droid;
using Plugin.FacebookClient;
using Plugin.Geolocator;
using Plugin.GoogleClient;
using Plugin.Media;
using Xamarin.Facebook;
using Xamarin.Forms;

namespace Helpa.Droid
{
    [Activity(Label = "Helpa", Icon = "@drawable/icon", Theme = "@style/MainTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
    {
        public static ICallbackManager CallbackManager = CallbackManagerFactory.Create();
        public static readonly string[] PERMISSIONS = new[] { "publish_actions" };
        public static readonly int PickImageId = 1000;
        public TaskCompletionSource<Stream> PickImageTaskCompletionSource { set; get; }

        #region Database
        static MainActivity instance;
        public static MainActivity Instance
        {
            get
            {
                return instance;
            }
        }
        #endregion
        
        protected override async void OnCreate(Bundle bundle)
        {
            try
            {
                //TabLayoutResource = Resource.Layout.Tabbar;
                //ToolbarResource = Resource.Layout.Toolbar;

                base.OnCreate(bundle);

                //FacebookSdk.SdkInitialize(this.ApplicationContext);
                AsNumAssemblyHelper.HoldAssembly();

                FacebookClientManager.Initialize(this);
                GoogleClientManager.Initialize(this);

                await CrossMedia.Current.Initialize();
                Plugin.CurrentActivity.CrossCurrentActivity.Current.Init(this, bundle);

                Forms.Init(this, bundle);
                Xamarin.FormsMaps.Init(this, bundle);

                instance = this;

                LoadApplication(new App());
            }
            catch (Exception e)
            {
                Log.Debug("Error", e.Message);
            }
        }


        #region Requesting Runtime Location Permissions
        readonly string[] PermissionsLocation =
        {
            Manifest.Permission.AccessCoarseLocation,
            Manifest.Permission.AccessFineLocation
        };
        const int RequestLocationId = 0;

        Page page;
        public async Task GetLocationPermissionAsync(PermissionServicesAndroid permissionServices, Page page)
        {
            this.page = page;
            // Check to see if any permission in our group is available, if one, then all are
            const string permission = Manifest.Permission.AccessFineLocation;
            Permission perm = CheckSelfPermission(permission);
            if (CheckSelfPermission(permission) == (int)Permission.Granted)
            {
                //await GetLocationAsync();
                return;
            }

            // Need to request permission
            if (ShouldShowRequestPermissionRationale(permission))
            {
                //TextView tv = new TextView(this);
                //// Explain to the user why we need to read the contacts
                //Snackbar.Make(tv, "Location access is required to show Helpers nearby.", Snackbar.LengthIndefinite)
                //        .SetAction("OK", v => RequestPermissions(PermissionsLocation, RequestLocationId))
                //        .Show();

                bool result = await permissionServices.page.DisplayAlert("Warning", "Location access is required to show Helpers nearby.", "Ok", "Cancel");

                if(result)
                {
                    RequestPermissions(PermissionsLocation, RequestLocationId);
                }
                else
                {
                    instance.Finish();
                }

                return;
            }

            // Finally request permissions with the list of permissions and Id
            RequestPermissions(PermissionsLocation, RequestLocationId);
        }

        public override async void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            try
            {
                switch (requestCode)
                {
                    case RequestLocationId:
                        {
                            if (grantResults[0] == Permission.Granted)
                            {
                                //TextView tv = new TextView(this);
                                //// Permission granted
                                //var snack = Snackbar.Make(tv, "Location permission is available, getting lat/long.", Snackbar.LengthShort);
                                //snack.Show();

                                //Toast.MakeText(this, "Location permission is available, getting lat/long.", ToastLength.Long).Show();
                                
                                //MessagingCenter.Send<Page>(page, "true");
                                if(page!=null && page.GetType() == typeof(Helpers))
                                {
                                    Helpers p = (Helpers)page;
                                    p.helpersViewModel.SetLocationOnMap(p.helpersViewModel.helperHomeList);
                                }
                                await GetLocationAsync();
                            }
                            else
                            {
                                //TextView tv = new TextView(this);
                                //// Permission Denied :(
                                //// Disabling location functionality
                                //var snack = Snackbar.Make(tv, "Location permission is denied.", Snackbar.LengthShort);
                                //snack.Show();
                                
                                //await page.DisplayAlert("", "Location permission is denied. Please allow location permission in app setting to show locations on map.", "Ok");

                                Toast.MakeText(this, "Location permission is denied.", ToastLength.Long).Show();
                                instance.Finish();
                            }
                        }
                        break;

                    default:
                        Plugin.Permissions.PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
                        break;
                }
            }
            catch(Exception e)
            {
                Console.Write(e.Message);
            }
        }

        public async Task GetLocationAsync()
        {
            //textLocation.Text = "Getting Location";
            try
            {
                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 100;
                TimeSpan ts = TimeSpan.FromTicks(1000000);
                var position = await locator.GetPositionAsync(ts);

                //textLocation.Text = string.Format("Lat: {0}  Long: {1}", position.Latitude, position.Longitude);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                
                //textLocation.Text = "Unable to get location: " + ex.ToString();
            }
        }
        #endregion

        #region Turned on GPS in Device setting programmatically

        #endregion
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent intent)
        {
            base.OnActivityResult(requestCode, resultCode, intent);
            FacebookClientManager.OnActivityResult(requestCode, resultCode, intent);
            GoogleClientManager.OnAuthCompleted(requestCode, resultCode, intent);

            CallbackManager.OnActivityResult(requestCode, (int)resultCode, intent);

            if (requestCode == PickImageId)
            {
                if ((resultCode == Result.Ok) && (intent != null))
                {
                    Android.Net.Uri uri = intent.Data;
                   
                    Stream stream = ContentResolver.OpenInputStream(uri);
                    PickImageTaskCompletionSource.SetResult(stream);
                }
                else
                {
                    PickImageTaskCompletionSource.SetResult(null);
                }
            }
        }
    }
}

