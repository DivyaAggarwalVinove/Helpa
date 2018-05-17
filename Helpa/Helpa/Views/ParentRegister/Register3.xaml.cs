﻿using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using System;
using System.Collections.Generic;
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

            //mapRegister.MoveToRegion(MapSpan.FromCenterAndRadius(new Xamarin.Forms.Maps.Position(position.Latitude, position.Longitude), Distance.FromMiles(0.2)));
            mapRegister.IsShowingUser = true;
        }
    }
}