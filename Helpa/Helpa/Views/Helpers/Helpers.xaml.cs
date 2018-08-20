using Helpa.Models;
using Helpa.Services;
using Helpa.ViewModels;
using Plugin.Geolocator;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace Helpa
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Helpers : ContentPage
    {
        HelpersViewModel helpersViewModel;

        #region HelperInstance
        static Helpers instance;
        public static Helpers Instance
        {
            get
            {
                return instance;
            }
        }
        #endregion

        public Helpers()
        {
            InitializeComponent();

            instance = this;
            NavigationPage.SetHasNavigationBar(this, false);

            helpersViewModel = new HelpersViewModel(this);
            helpersViewModel.mapHelper = mapHelper;
            BindingContext = helpersViewModel;

            MessagingCenter.Subscribe<CustomMap, string>(this, "Hi", (sender, selectedCluster) =>
            {
                //lvHalfHelpa.ScrollTo(helpersViewModel.helperList, ScrollToPosition., false);
                //lvHalfHelpa.SelectedItem

               // if (rlHalfView.IsVisible == false)
                {
                    rlHalfView.IsVisible = true;
                    
                    var selectedHelpersInCluster = helpersViewModel.helperHomeList.Where(h => h.LocalityName == (selectedCluster)).FirstOrDefault();
                    helpersViewModel.HelperHalfList = new ObservableCollection<HelperHome>(selectedHelpersInCluster.HelpersInLocalties);
                    lvHalfHelpa.ItemsSource = helpersViewModel.HelperHalfList;
                    lblHelperCount.Text = selectedHelpersInCluster.NumberOfHelpersInLocality + " Helpers found in " + selectedHelpersInCluster.LocalityName;
                }
               // else
                   // rlHalfView.IsVisible = false;
            });

            //Action action = (async() =>  await GetRuntimeLocationPermission(5000));
        }
        
        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await GetRuntimeLocationPermission(5000);
        }
        
        public async Task GetRuntimeLocationPermission(int milisec)
        {
            await Task.Delay(milisec);

            await DependencyService.Get<IPermissionServices>().GetPermission(this);

            helpersViewModel.LoadItemsCommand = new Command(async () => await helpersViewModel.ExecuteLoadItemsCommand());
            helpersViewModel.LoadItemsCommand.Execute(null);
        }

        /*
        //async public void CreateMap()
        //{
        //    try
        //    {
        //        var locator = CrossGeolocator.Current;
        //        TimeSpan ts = TimeSpan.FromTicks(1000000);
        //        var position = await locator.GetPositionAsync(ts);

        //        mapHelper.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(position.Latitude, position.Longitude), Distance.FromKilometers(5)));
        //        mapHelper.IsShowingUser = true;
        //        mapHelper.MapType = MapType.Street;

        //        var pin = new Pin()
        //        {
        //            Type = PinType.Place,
        //            Position = new Position(double.Parse("28.6139391", CultureInfo.InvariantCulture), double.Parse("77.2090212", CultureInfo.InvariantCulture)),
        //            Label = "Site 1",
        //            Address = "394 Pacific Ave, San Francisco CA",
        //            Id = "1"
        //        };
        //        mapHelper.Pins.Add(pin);

        //        mapHelper.Pins.Add(new Pin()
        //        {
        //            Type = PinType.Place,
        //            Position = new Position(double.Parse("28.4514279", CultureInfo.InvariantCulture), double.Parse("77.0704678", CultureInfo.InvariantCulture)),
        //            Label = "Site 2",
        //            Address = "394 Pacific Ave, San Francisco CA",
        //            Id = "2"
        //        });

        //        mapHelper.Pins.Add(new Pin()
        //        {
        //            Type = PinType.Place,
        //            Position = new Position(double.Parse("28.4510217", CultureInfo.InvariantCulture), double.Parse("77.0721232", CultureInfo.InvariantCulture)),
        //            Label = "Site 3",
        //            Address = "394 Pacific Ave, San Francisco CA",
        //            Id = "Xamarin"
        //        });

        //        mapHelper.Pins.Add(new Pin()
        //        {
        //            Type = PinType.Place,
        //            Position = new Position(double.Parse("28.4527924", CultureInfo.InvariantCulture), double.Parse("77.0709329", CultureInfo.InvariantCulture)),
        //            Label = "Site 4",
        //            Address = "394 Pacific Ave, San Francisco CA",
        //            Id = "Xamarin"
        //        });

        //        MessagingCenter.Subscribe<CustomMap>(this, "Hi", (sender) =>
        //        {
        //            //lvHalfHelpa.ScrollTo(helpersViewModel.helperList, ScrollToPosition., false);
        //            //lvHalfHelpa.SelectedItem

        //            if (rlHalfView.IsVisible == false)
        //                rlHalfView.IsVisible = true;
        //            else
        //                rlHalfView.IsVisible = false;
        //        });
        //    }
        //    catch (Exception e)
        //    {
        //        Debug.Write(e.Message);
        //    }
        //}
        */

        void HelperSelectedFullList(object sender, ItemTappedEventArgs args)
        {
            var selectedItem = (HelperHome)args.Item;
            int selectedIndex = helpersViewModel.HelperFullList.IndexOf(helpersViewModel.HelperFullList.Where(h => h.UserId == selectedItem.UserId).FirstOrDefault());
            helpersViewModel.HelperFullList.ElementAt(selectedIndex).color = "#FADC54";
            //slOuterFull.
            //lvFullHelpa.GetChildAt(args.)
        }

        void OnCloseHalfList(object sender, EventArgs args)
        {
            rlHalfView.IsVisible = false;
        }

        async void OnClickPostJob(object sender, EventArgs args)
        {
            var user = App.Database.GetLoggedUser();
            if (user == null)
                await Application.Current.MainPage.Navigation.PushAsync(new LoginPage());
            // await Navigation.PushAsync(new LoginPage());
            else
            {
                //post a job
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