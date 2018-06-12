using Helpa.ViewModels;
using Plugin.Geolocator;
using System;
using System.Diagnostics;
using System.Globalization;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace Helpa
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Helpers : ContentPage
    {
        HelpersViewModel helpersViewModel;
        public Helpers()
        {
            InitializeComponent();

            NavigationPage.SetHasNavigationBar(this, false);

            helpersViewModel = new HelpersViewModel(this);
            helpersViewModel.context = this;

            BindingContext = helpersViewModel;

            //CreateMap();
        }

        async public void CreateMap()
        {
            try
            {
                var locator = CrossGeolocator.Current;
                TimeSpan ts = TimeSpan.FromTicks(1000000);
                var position = await locator.GetPositionAsync(ts);

                mapHelper.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(position.Latitude, position.Longitude), Distance.FromMiles(0.2)));
                mapHelper.IsShowingUser = true;
                mapHelper.MapType = MapType.Street;

                var pin = new Pin()
                {
                    Type = PinType.Place,
                    Position = new Position(double.Parse("28.6139391", CultureInfo.InvariantCulture), double.Parse("77.2090212", CultureInfo.InvariantCulture)),
                    Label = "Site 1",
                    Address = "394 Pacific Ave, San Francisco CA",
                    Id = "1"
                };
                mapHelper.Pins.Add(pin);

                mapHelper.Pins.Add(new Pin()
                {
                    Type = PinType.Place,
                    Position = new Position(double.Parse("28.4514279", CultureInfo.InvariantCulture), double.Parse("77.0704678", CultureInfo.InvariantCulture)),
                    Label = "Site 2",
                    Address = "394 Pacific Ave, San Francisco CA",
                    Id = "2"
                });

                mapHelper.Pins.Add(new Pin()
                {
                    Type = PinType.Place,
                    Position = new Position(double.Parse("28.4510217", CultureInfo.InvariantCulture), double.Parse("77.0721232", CultureInfo.InvariantCulture)),
                    Label = "Site 3",
                    Address = "394 Pacific Ave, San Francisco CA",
                    Id = "Xamarin"
                });

                mapHelper.Pins.Add(new Pin()
                {
                    Type = PinType.Place,
                    Position = new Position(double.Parse("28.4527924", CultureInfo.InvariantCulture), double.Parse("77.0709329", CultureInfo.InvariantCulture)),
                    Label = "Site 4",
                    Address = "394 Pacific Ave, San Francisco CA",
                    Id = "Xamarin"
                });

                MessagingCenter.Subscribe<CustomMap>(this, "Hi", (sender) =>
                {
                    //lvHalfHelpa.ScrollTo(helpersViewModel.helperList, ScrollToPosition., false);
                    //lvHalfHelpa.SelectedItem

                    if (rlHalfView.IsVisible == false)
                        rlHalfView.IsVisible = true;
                    else
                        rlHalfView.IsVisible = false;
                });
            }
            catch (Exception e)
            {
                Debug.Write(e.Message);
            }
        }

        void OnClickPostJob(object sender, EventArgs args)
        {
            var user = App.Database.GetUsersAsync();
            if(user==null || user.Count==0)
                Navigation.PushAsync(new HelperRegister());
            else
            {
                if (user[0].IsCompleted)
                {
                    //post a job
                }
                else
                { 
                    Navigation.PushAsync(new HelperRegister1(user[0]));
                }
            }
        }

        void OnShowHelpersList(object sender, EventArgs args)
        {
            if (mapHelper.IsVisible)
            {
                lvFullHelpa.IsVisible = true;
                mapHelper.IsVisible = false;
                lblCount.IsVisible = false;

                imgHelpersList.Source = "location_filter.png";
            } else
            {
                lvFullHelpa.IsVisible = false;
                mapHelper.IsVisible = true;
                lblCount.IsVisible = true;

                imgHelpersList.Source = "filter.png";
            }
        }
    }
}