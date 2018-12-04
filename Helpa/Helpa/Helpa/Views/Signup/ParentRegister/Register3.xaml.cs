using DurianCode.PlacesSearchBar;
using Helpa.Models;
using Helpa.Services;
using Helpa.Utility;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace Helpa
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Register3 : ContentPage
	{
		public Register3 ()
		{
			InitializeComponent ();

            NavigationPage.SetHasNavigationBar(this, false);
            entryRegSearch3.ApiKey = Constants.googlePlaceApiKey;

            SetLocationOnMap();            
        }

        /*
        async void SetLocationOnMap()
        {
            var locator = CrossGeolocator.Current;
            TimeSpan ts = TimeSpan.FromTicks(1000000);
            Plugin.Geolocator.Abstractions.Position position = await locator.GetPositionAsync(ts);
            IEnumerable<Address> addresses =  await locator.GetAddressesForPositionAsync(position, "AIzaSyDminfXt_CoSb9UTXpPFZwQIG2lDduDMjs");
            Address address = addresses.FirstOrDefault();
            //Address address = (addresses.ToArray())[0];
            entryRegSearch3.Placeholder = address.FeatureName + "," + address.SubLocality + "," + address.Locality;
            var pin = new Pin()
            {
                Type = PinType.Place,
                Position = new Xamarin.Forms.Maps.Position(position.Latitude, position.Longitude),
                Label = address.FeatureName,
                Address = address.FeatureName + "," + address.SubLocality + "," + address.Locality,
                Id = "1"
            };
           // mapRegister.Pins.Add(pin);
            //Pin pin = mapRegister.Pins.FirstOrDefault();

           // mapRegister.MoveToRegion(MapSpan.FromCenterAndRadius(new Xamarin.Forms.Maps.Position(position.Latitude, position.Longitude), Distance.FromMiles(0.2)));
           // mapRegister.IsShowingUser = true;
        }
        */

        async void SetLocationOnMap()
        {
            try
            {
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.LocationAlways);
                if (status != PermissionStatus.Granted)
                {
                    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Location))
                    {
                        //await DisplayAlert("Need location", "Helpa need that location", "OK");
                    }

                    var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Location);

                    if (results.ContainsKey(Permission.Location))
                        status = results[Permission.Location];
                }

                if (status == PermissionStatus.Granted)
                {
                    await DependencyService.Get<IPermissionServices>().CheckAndTurnOnGPS(this);

                    Plugin.Geolocator.Abstractions.Position position = await CrossGeolocator.Current.GetPositionAsync();

                    var addr = await CrossGeolocator.Current.GetAddressesForPositionAsync(new Plugin.Geolocator.Abstractions.Position(position.Latitude, position.Longitude), "AIzaSyDminfXt_CoSb9UTXpPFZwQIG2lDduDMjs");
                    var a = addr.FirstOrDefault();
                    var loc = a.FeatureName + "," + a.SubLocality + "," + a.Locality + "," + a.CountryName + "," + a.PostalCode;

                    mapRegister.Pins.Clear();
                    mapRegister.Pins.Add(new Pin()
                    {
                        Type = PinType.Place,
                        Position = new Xamarin.Forms.Maps.Position(position.Latitude, position.Longitude),
                        Label = a.FeatureName,
                        Address = a.FeatureName + "," + a.SubLocality + "," + a.Locality,
                        Id = "1"
                    });

                    mapRegister.MoveToRegion(MapSpan.FromCenterAndRadius(new Xamarin.Forms.Maps.Position(position.Latitude, position.Longitude), Distance.FromMiles(0.2)));
                    mapRegister.IsShowingUser = true;
                    //MessagingCenter.Send<Register2, string>(this, "Current Address", loc);
                }
                else if (status != PermissionStatus.Unknown)
                {
                    //await DisplayAlert("Location Denied", "Please allow location permission to get your location for Helpa app in Device setting", "OK");
                }
            }
            catch (Exception ex)
            {
                //await DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private async void OnPlacesRetrieved(object sender, AutoCompleteResult result)
        {
            if (!(result.Status == "OK"))
                return;

            ObservableCollection<LocationModel> addresses = new ObservableCollection<LocationModel>();
            //List<string> addresses = new List<string>();
            List<AutoCompletePrediction> selectedPrediction = result.AutoCompletePlaces;
            foreach (AutoCompletePrediction autoCompletePlace in result.AutoCompletePlaces)
            {
                Place place = await Places.GetPlace(autoCompletePlace.Place_ID, Constants.googlePlaceApiKey);
                addresses.Add(new LocationModel { Address = autoCompletePlace.Description, Latitude=place.Latitude.ToString(), Longitude=place.Longitude.ToString() });
            }

            if (addresses.Count == 0)
                return;

            lvAddresses.ItemsSource = addresses;
        }

        /*
        async void OnLocationSearch(object sender, EventArgs eventArgs)
        {
            try
            {
                ObservableCollection<LocationModel> locations = new ObservableCollection<LocationModel>();
                //var locator = CrossGeolocator.Current;
                var locator = new Geocoder();
                var address = ((CustomEntry)sender).Text;
                List<string> addresses = new List<string>();

                if (string.IsNullOrEmpty(address))
                    return;

                IEnumerable<Xamarin.Forms.Maps.Position> positions = await locator.GetPositionsForAddressAsync(address);
                foreach (Xamarin.Forms.Maps.Position item in positions)
                {
                    IEnumerable<string> addr = await locator.GetAddressesForPositionAsync(new Xamarin.Forms.Maps.Position(item.Latitude, item.Longitude));
                    var a = addr.FirstOrDefault();
                    //locations.Add(new LocationModel { Address = a.FeatureName + "," + a.SubLocality + "," + a.Locality + "," + a.CountryName + "," + a.PostalCode });
                    locations.Add(new LocationModel { Address = a });

                    //addresses.Add(a.FeatureName + "," + a.SubLocality + "," + a.Locality + "," + a.CountryName + "," + a.PostalCode);
                }
                lvAddresses.ItemsSource = locations;

            }
            catch(Exception e)
            {
                Console.Write(e.StackTrace);
            }

            //BindingContext = lm;
        }
        */

        //void OnLocationSelected(object sender, EventArgs eventArgs)
        //{            
        //    entryRegSearch3.Text = ((Label)sender).Text;

        //    ObservableCollection<LocationModel> locations = new ObservableCollection<LocationModel>();
        //    lvAddresses.ItemsSource = locations;
        //}

        void OnClickNext(object sender, EventArgs eventArgs)
        {
            MessagingCenter.Send<Register3, LocationModel>(this, "Selected Address", lm);
            
            Navigation.PopAsync();
            Navigation.PopAsync();
        }

        LocationModel lm = new LocationModel();
        private void OnLocationSelected(object sender, SelectedItemChangedEventArgs e)
        {
            lm = (LocationModel)e.SelectedItem;

            entryRegSearch3.Text = (lm).Address;
            if (Register2.recentsearch.Count >= 3)
                Register2.recentsearch.RemoveAt(0);
            Register2.recentsearch.Add(lm);

            ObservableCollection<LocationModel> locations = new ObservableCollection<LocationModel>();
            lvAddresses.ItemsSource = locations;
        }
    }
}