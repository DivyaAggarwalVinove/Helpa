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

            //helpersViewModel = new HelpersViewModel();
            //helpersViewModel.context = this;

            //BindingContext = helpersViewModel;

            CreateMap();
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
                //pin.Clicked += (sender, e) =>
                //{
                //    DisplayAlert("This is a tag", "Click click, click", "Cancel");
                //    if (slHalfList.IsVisible == false)
                //        slHalfList.IsVisible = true;
                //    else
                //        slHalfList.IsVisible = false;
                //};
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

                    if (rlHalfList.IsVisible == false)
                        rlHalfList.IsVisible = true;
                    else
                        rlHalfList.IsVisible = false;
                });
            }
            catch (Exception e)
            {
                Debug.Write(e.Message);
            }
        }

        void OnHelpersListTapped(object sender, EventArgs args)
        {
            
            //Navigation.PushAsync(new HelpersListPage());
            //gridHelpersBody.Children.Remove(this.FindByName<RelativeLayout>("relativeLayout"));
            //gridHelpersBody.Children.Add(new HelpersList());

            //var mapView = this.FindByName<CustomMap>("mapHelper");
            if (mapHelper.IsVisible)
            {
                lvFullHelpa.IsVisible = true;
                mapHelper.IsVisible = false;
                lblCount.IsVisible = false;

                Rectangle rectangle = new Rectangle(relativeLayout.X, relativeLayout.Y, relativeLayout.Width, relativeLayout.Height);
                rlHalfList.Layout(rectangle);
                

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