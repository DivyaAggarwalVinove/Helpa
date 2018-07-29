﻿using Helpa.Models;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
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

            SetLocationOnMap();            
        }

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
            mapRegister.Pins.Add(pin);
            //Pin pin = mapRegister.Pins.FirstOrDefault();

            mapRegister.MoveToRegion(MapSpan.FromCenterAndRadius(new Xamarin.Forms.Maps.Position(position.Latitude, position.Longitude), Distance.FromMiles(0.2)));
            mapRegister.IsShowingUser = true;
        }

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
                addressesView.ItemsSource = locations;

            }
            catch(Exception e)
            {
                Console.Write(e.StackTrace);
            }

            //BindingContext = lm;
        }

        void OnLocationSelected(object sender, EventArgs eventArgs)
        {            
            entryRegSearch3.Text = ((Label)sender).Text;

            ObservableCollection<LocationModel> locations = new ObservableCollection<LocationModel>();
            addressesView.ItemsSource = locations;
        }

        void OnClickNext(object sender, EventArgs eventArgs)
        {
            MessagingCenter.Send<Register3, string>(this, "Selected Address", entryRegSearch3.Text);
            
            Navigation.PopAsync();
            Navigation.PopAsync();
        }
    }
}