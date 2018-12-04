using Helpa.Models;
using Helpa.Services;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Helpa
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Register2 : ContentPage
	{
        public static List<LocationModel> recentsearch = new List<LocationModel>(); 
		public Register2 ()
		{
			InitializeComponent ();

            NavigationPage.SetHasNavigationBar(this, false);

            if (recentsearch != null && recentsearch.Count >= 1)
            {
                gridRecent1.IsVisible = true;
                lblRecent1.Text = recentsearch.ElementAt(0).Address;
            }
            else
            {
                gridRecent1.IsVisible = false;
            }

            if (recentsearch != null && recentsearch.Count >= 2)
            {
                gridRecent2.IsVisible = true;
                lblRecent2.Text = recentsearch.ElementAt(1).Address;
            }
            else
            {
                gridRecent2.IsVisible = false;
            }

            if (recentsearch != null && recentsearch.Count ==3)
            {
                gridRecent3.IsVisible = true;
                lblRecent3.Text = recentsearch.ElementAt(2).Address;
            }
            else
            {
                gridRecent3.IsVisible = false;
            }
        }

        void OnSetLocationOnMap(object sender, EventArgs eventArgs)
        {
            Navigation.PushAsync(new Register3());
        }

       async void OnClickYourLocation(object sender, EventArgs eventArgs)
        {
            try
            {
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.LocationAlways);
                if (status != PermissionStatus.Granted)
                {
                    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Location))
                    {
                        await DisplayAlert("Need location", "Helpa need that location", "OK");
                    }

                    var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Location);

                    if (results.ContainsKey(Permission.Location))
                        status = results[Permission.Location];
                }

                if (status == PermissionStatus.Granted)
                {
                    await DependencyService.Get<IPermissionServices>().CheckAndTurnOnGPS(this);

                    Position position = await CrossGeolocator.Current.GetPositionAsync();

                    var addr = await CrossGeolocator.Current.GetAddressesForPositionAsync(new Position(position.Latitude, position.Longitude), "AIzaSyDminfXt_CoSb9UTXpPFZwQIG2lDduDMjs");
                    var a = addr.FirstOrDefault();
                    var loc = a.FeatureName + "," + a.SubLocality + "," + a.Locality + "," + a.CountryName + "," + a.PostalCode;
                    
                    MessagingCenter.Send<Register2, LocationModel>(this, "Current Address", new LocationModel() { Address = loc, Latitude = position.Latitude.ToString(), Longitude=position.Longitude.ToString() });
                    await Navigation.PopAsync();
                }
                else if (status != PermissionStatus.Unknown)
                {
                    await DisplayAlert("Location Denied", "Please allow location permission to get your location for Helpa app in Device setting", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private void OnClickRecentSearch(object sender, TappedEventArgs e)
        {
            var selLoc = recentsearch.ElementAt(int.Parse(e.Parameter.ToString()));
            MessagingCenter.Send<Register2, LocationModel>(this, "Current Address", new LocationModel() { Address = selLoc.Address, Latitude = selLoc.Latitude.ToString(), Longitude = selLoc.Longitude.ToString() });
        }

        private void OnClearRecentSearch(object sender, EventArgs e)
        {
            recentsearch.Clear();
            gridRecent1.IsVisible = false;
            gridRecent2.IsVisible = false;
            gridRecent3.IsVisible = false;
        }
    }   
}