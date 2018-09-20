using Helpa.Services;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Helpa
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class HelperRegister2 : ContentPage
	{
		public HelperRegister2()
		{
			InitializeComponent ();

            NavigationPage.SetHasNavigationBar(this, false);
        }

        void OnSetLocationOnMap(object sender, EventArgs eventArgs)
        {
            Navigation.PushAsync(new HelperRegister3());
        }

        async void OnClickYourLocation(object sender, EventArgs eventArgs)
        {
            string currLoc = await GetCurrentLocation();

            MessagingCenter.Send<HelperRegister2, string>(this, "Current Address", currLoc);

            await Navigation.PopAsync();
        }

        async Task<string> GetCurrentLocation()
        {
            var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);
            if (status != PermissionStatus.Granted)
            {
                await DependencyService.Get<IPermissionServices>().GetPermission(this);
            }

            var locator = CrossGeolocator.Current;
            TimeSpan ts = TimeSpan.FromTicks(1000000);
            Position position = await locator.GetPositionAsync(ts);
            IEnumerable<Address> addresses = await locator.GetAddressesForPositionAsync(position, "AIzaSyDminfXt_CoSb9UTXpPFZwQIG2lDduDMjs");
            Address address = addresses.FirstOrDefault();

            return address.FeatureName + "," + address.SubLocality + "," + address.Locality + ";" + position.Latitude + ";" + position.Longitude;
        }
    }
}