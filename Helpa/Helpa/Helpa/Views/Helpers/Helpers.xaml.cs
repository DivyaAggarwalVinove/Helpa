using DurianCode.PlacesSearchBar;
using Helpa.Models;
using Helpa.Services;
using Helpa.Utility;
using Helpa.ViewModels;
using Helpa.Views.Helpers;
using Helpa.Views.Profile;
using Plugin.Share;
using Plugin.Share.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public HelpersViewModel helpersViewModel;

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

            entrySearch.ApiKey = Constants.googlePlaceApiKey;

            listView.ItemsSource = null;

            MessagingCenter.Subscribe<CustomMap, string>(this, "Hi", (sender, selectedCluster) =>
            {
                try
                {
                    aiFindHalfHelper.IsRunning = true;

                    lvHalfHelpa.ItemsSource = null;
                    lblHelperCount.Text = " Helpers found in ";

                    ShowHelperHalfList(selectedCluster);

                    aiFindHalfHelper.IsRunning = false;
                }
                catch (Exception e)
                {
                    Console.Write(e.StackTrace);
                }
            });
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (App.selectedPage != 4)
                return;

            RegisterUserModel loggedUser = App.Database.GetLoggedUser();
            if (loggedUser == null)
                return;

            ProfileAfterLoginPage profileAfterLoginPage = new ProfileAfterLoginPage(loggedUser);
            //profileAfterLoginPage.BindingContext = new ProfileAfterLoginViewModel(loggedUser);
            ProfilePage.pcv.Content = profileAfterLoginPage.Content;

            //await GetRuntimeLocationPermission(5000);
        }


        async void ShowHelperHalfList(string selectedCluster)
        {
            try
            {
                rlHalfView.IsVisible = true;

                var selectedHelpersInCluster = helpersViewModel.helperHomeList.Where(h => h.LocationName == (selectedCluster)).FirstOrDefault();
                mapHelper.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(selectedHelpersInCluster.Latitude - 0.0055, selectedHelpersInCluster.Longitude), Distance.FromKilometers(1)));

                IEnumerable<HelperHome> hs = new List<HelperHome>();
                HelpersServices helpersServices = new HelpersServices();

                RegisterUserModel loggedUser = App.Database.GetLoggedUser();
                if (loggedUser == null)
                    hs = await helpersServices.GetHelpersInLocation(selectedHelpersInCluster.Latitude, selectedHelpersInCluster.Longitude, 0);
                else
                    hs = await helpersServices.GetHelpersInLocation(selectedHelpersInCluster.Latitude, selectedHelpersInCluster.Longitude, loggedUser.Id);

               // var hs = await helpersServices.GetHelpersInLocation(selectedHelpersInCluster.Latitude, selectedHelpersInCluster.Longitude, 0);

                for (int i = 0; i < hs.Count(); i++)
                {
                    HelperHome h = hs.ElementAt(i);
                    if (h.Service != null && h.Service.Count() != 0)
                    {
                        HService hserv = h.Service.Where(x => x.ServiceName == "ChildCare").FirstOrDefault();
                        if (hserv == null)
                        {
                            hserv = h.Service.ElementAt(h.Service.Count() - 1);
                            h.ServiceName = hserv.ServiceName;
                            if (hserv.price.Daily)
                                h.ServicePriceLabel = "from $" + hserv.price.Min.Remove(hserv.price.Min.IndexOf(".")) + "-$" + hserv.price.Max.Remove(hserv.price.Max.IndexOf(".")) + "/Day";
                            else if (hserv.price.Monthly)
                                h.ServicePriceLabel = "from $" + hserv.price.Min.Remove(hserv.price.Min.IndexOf(".")) + "-$" + hserv.price.Max.Remove(hserv.price.Max.IndexOf(".")) + "/Month";
                            else if (hserv.price.Hours)
                                h.ServicePriceLabel = "from $" + hserv.price.Min.Remove(hserv.price.Min.IndexOf(".")) + "-$" + hserv.price.Max.Remove(hserv.price.Max.IndexOf(".")) + "/Hour";

                            if (hserv.Location != null && hserv.Location.Count() > 0)
                                h.ServiceLocationName = hserv.Location.ElementAt(0).LocationName;
                            /* source.Select(element => element == oldValue ? newValue : element) */

                            if (h.AverageRatingCount == null)
                            {
                                h.AverageRatingCount = "(0)";
                            }
                            else
                            {
                                h.AverageRatingCount = "(" + h.AverageRatingCount + ")";
                            }

                            if (h.Status)
                            {
                                h.bgcolor = "#32BDA0";
                                h.textcolor = "#FFFFFF";
                                h.helperStatus = "Available";
                            }
                            else
                            {
                                h.bgcolor = "#EAE9E9";
                                h.textcolor = "#000000";
                                h.helperStatus = "Not Available";
                            }

                            hs.Select(x => x.Name == h.Name ? h : x);
                        }
                    }

                }

                helpersViewModel.HelperHalfList = new ObservableCollection<HelperHome>(hs);
                lvHalfHelpa.ItemsSource = helpersViewModel.HelperHalfList;
                lblHelperCount.Text = hs.Count() + " Helpers found in " + selectedHelpersInCluster.LocationName;
            }
            catch (Exception e)
            {
                Console.Write(e.StackTrace);
            }
        }

        async void ShowHelperFullList()
        {
            try
            {
                aiFindHelper.IsRunning = true;

                HHomeModel hService = new HHomeModel();
                HelpersServices helpersServices = new HelpersServices();

                RegisterUserModel loggedUser = App.Database.GetLoggedUser();
                if (loggedUser == null)
                    hService = await helpersServices.GetAllHelpers(0);
                else
                    hService = await helpersServices.GetAllHelpers(loggedUser.Id);

                 
                var hs = hService.Data;

                lblHelperFullCount.Text = hService.Total + " Helpers found";

                for (int i = 0; i < hs.Count(); i++)
                {
                    HelperHome h = hs.ElementAt(i);

                    if (h.BookMark)
                        h.BookmarkImage = "save_filled.png";
                    else
                        h.BookmarkImage = "save.png";

                    if (h.Service != null && h.Service.Count() != 0)
                    {
                        HService hserv = h.Service.Where(x => x.ServiceName == "ChildCare").FirstOrDefault();
                        if (hserv == null)
                        {
                            hserv = h.Service.ElementAt(h.Service.Count() - 1);
                            h.ServiceName = hserv.ServiceName;
                            if (hserv.price.Daily)
                                h.ServicePriceLabel = "from $" + hserv.price.Min.Remove(hserv.price.Min.IndexOf(".")) + "-$" + hserv.price.Max.Remove(hserv.price.Max.IndexOf(".")) + "/Day";
                            else if (hserv.price.Monthly)
                                h.ServicePriceLabel = "from $" + hserv.price.Min.Remove(hserv.price.Min.IndexOf(".")) + "-$" + hserv.price.Max.Remove(hserv.price.Max.IndexOf(".")) + "/Month";
                            else if (hserv.price.Hours)
                                h.ServicePriceLabel = "from $" + hserv.price.Min.Remove(hserv.price.Min.IndexOf(".")) + "-$" + hserv.price.Max.Remove(hserv.price.Max.IndexOf(".")) + "/Hour";

                            if (hserv.Location != null && hserv.Location.Count() > 0)
                                h.ServiceLocationName = hserv.Location.ElementAt(0).LocationName;
                            /* source.Select(element => element == oldValue ? newValue : element) */

                            if (h.AverageRatingCount == null)
                            {
                                h.AverageRatingCount = "(0)";
                            }
                            else
                            {
                                h.AverageRatingCount = h.AverageRating + " (" + h.AverageRatingCount + ")";
                            }

                            if (h.Status)
                            {
                                h.bgcolor = "#32BDA0";
                                h.textcolor = "#FFFFFF";
                                h.helperStatus = "Available";
                            }
                            else
                            {
                                h.bgcolor = "#EAE9E9";
                                h.textcolor = "#000000";
                                h.helperStatus = "Not Available";
                            }

                            hs.Select(x => x.Name == h.Name ? h : x);
                        }
                    }
                }

                helpersViewModel.HelperFullList = new ObservableCollection<HelperHome>(hs);
                lvFullHelpa.ItemsSource = helpersViewModel.HelperFullList;
                //lblHelperCount.Text = hs.Count() + " Helpers found in " + selectedHelpersInCluster.LocationName;

                aiFindHelper.IsRunning = false;
            }
            catch (Exception e)
            {
                Console.Write(e.StackTrace);
                aiFindHelper.IsRunning = false;
            }
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
            //helpersViewModel.HelperFullList.ElementAt(selectedIndex).bgcolor = "#FADC54";

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
                await Application.Current.MainPage.Navigation.PushAsync(new PostJobPage() { loggedUser = user });
            }
        }

        void OnShowHelpersList(object sender, EventArgs args)
        {
            if (mapHelper.IsVisible)
            {
                slFullHelpa.IsVisible = true;
                mapHelper.IsVisible = false;
                lblCount.IsVisible = false;

                ShowHelperFullList();

                imgHelpersList.Source = "location_filter.png";
            }
            else
            {
                slFullHelpa.IsVisible = false;
                mapHelper.IsVisible = true;
                lblCount.IsVisible = true;

                lvFullHelpa.ItemsSource = null;
                lblHelperFullCount.Text = " Helpers found";

                imgHelpersList.Source = "filter.png";
            }
        }


        string filteredService = "";
        string filteredScope = "";
        char filteredLocationType = '\0';
        string filteredSortBy = "";
        private async void OnClickServiceFilter(object sender, EventArgs e)
        {
            aiFindHelper.IsRunning = true;
            IList<ServiceModel> servicesAsync = (await new Utilities().GetServicesAsync());
            var servicesName = servicesAsync.Select(x => x.ServiceName);
            filteredService = await DisplayActionSheet("Select Service", "Cancel", null, servicesName.ToArray());
            lblServiceFilter.Text = filteredService;
            aiFindHelper.IsRunning = false;
        }

        private async void OnClickLocationFilter(object sender, EventArgs e)
        {
            var result = await DisplayActionSheet("Select Service", "Cancel", null, "Home Helper", "Mobile's Helper", "All");

            if (result.Equals("Home Helper"))
            {
                var filteredList = helpersViewModel.helperHomeList.Where(x => x.LocationType == 'S');
                helpersViewModel.SetLocationOnMap(new ObservableCollection<HelperHomeModel>(filteredList));
                filteredLocationType = 'S';
            }
            else
            if (result.Equals("All"))
            {
                helpersViewModel.SetLocationOnMap(helpersViewModel.helperHomeList);
                filteredLocationType = '\0';
            }
            else
            {
                var filteredList = helpersViewModel.helperHomeList.Where(x => x.LocationType.Equals('M'));
                helpersViewModel.SetLocationOnMap(new ObservableCollection<HelperHomeModel>(filteredList));
                filteredLocationType = 'M';
            }
        }

        private void OnClickScopeFilter(object sender, EventArgs e)
        {

        }

        private void OnClickSortByFilter(object sender, EventArgs e)
        {

        }

        bool flag = true;
        List<AutoCompletePrediction> selectedPrediction;
        private void OnPlacesRetrieved(object sender, AutoCompleteResult result)
        {
            if (flag)
            {
                var searchBar = (PlacesBar)sender;
                if (!(result.Status == "OK"))
                    return;

                if (string.IsNullOrEmpty(searchBar.Text))
                {
                    listView.IsVisible = false;
                    listView.SelectedItem = null;

                    return;
                }

                IList<string> description = new List<string>();
                selectedPrediction = result.AutoCompletePlaces;
                foreach (AutoCompletePrediction autoCompletePlace in result.AutoCompletePlaces)
                {
                    //SfAutoCompleteItem autoCompleteItem = new SfAutoCompleteItem(autoCompletePlace.Description, "location.png");
                    description.Add(autoCompletePlace.Description);
                }

                if (description.Count == 0)
                    return;

                listView.IsVisible = true;
                listView.TranslationY = gridSearchBar.Height;
                listView.TranslationX = gridSearchBar.X;

                listView.ItemsSource = description;
            }
            else
            {
                flag = true;
            }
        }

        private void OnLocationSelected(object sender, ItemTappedEventArgs e)
        {
            string selectedLoc = ((ListView)sender).SelectedItem.ToString();
            entrySearch.Text = selectedLoc;

            listView.IsVisible = false;
            listView.SelectedItem = null;

            flag = false;
        }

        private async void OnHelperSearch(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(entrySearch.Text))
            {
                listView.IsVisible = false;
                listView.SelectedItem = null;

                string selectedLoc = entrySearch.Text;
                //selectedLoc = selectedLoc.Split(',')[0];
                var loc = selectedPrediction.Where(x => x.Description == selectedLoc).FirstOrDefault();
                Place place = await Places.GetPlace(loc.Place_ID, Constants.googlePlaceApiKey);

                /*var latitude = place.Latitude;
                var longitude = place.Longitude;
                IList<HelperHomeModel> filterList = helpersViewModel.helperHomeList.Where(x => (x.Latitude == latitude)).ToList();
                var c = filterList.Where(x => x.Longitude == longitude).ToList();*/

                var filterList = helpersViewModel.helperHomeList.Where(x => x.Latitude == place.Latitude && x.Longitude == place.Longitude);

                //filterList = filterList.Where(x => x.Longitude == longitude);
                //if (!string.IsNullOrEmpty(filteredService))
                //    filterList = filterList.Where(x =>);

                //if (!string.IsNullOrEmpty(filteredScope))
                //    filterList = filterList.Where(x =>);

                //if (!string.IsNullOrEmpty(filteredSortby))
                //    filterList = filterList.Where(x =>);

                //helpersViewModel.SetLocationOnMap(helpersViewModel.helperHomeList);

                if (filteredLocationType != '\0')
                {
                    //filterList = filterList.Where(x => x.LocationType.Equals(filteredLocationType));
                }

                if (filterList != null && filterList.Count() != 0)
                    helpersViewModel.SetLocationOnMap(new ObservableCollection<HelperHomeModel>(filterList));
            }
        }

        private async void OnClickBookmark(object sender, TappedEventArgs e)
        {
            aiFindHelper.IsRunning = true;

            var imgBookmark = ((Image)sender);
            if (imgBookmark.Source.ToString().Contains("save.png"))
            {
                if (e.Parameter != null)
                {
                    int helperId = int.Parse(e.Parameter.ToString());

                    RegisterUserModel loggedUser = App.Database.GetLoggedUser();
                    if (loggedUser == null)
                        return;

                    bool isSuccess = await (new HelpersServices()).BookMarkHelper(loggedUser.Id, helperId);
                    imgBookmark.Source = "save_filled.png";
                }
            }
            else
            {
                
                int helperId = int.Parse(e.Parameter.ToString());

                RegisterUserModel loggedUser = App.Database.GetLoggedUser();
                if (loggedUser == null)
                    return;

                bool isSuccess = await (new HelpersServices()).UnBookMarkHelper(loggedUser.Id, helperId);
                imgBookmark.Source = "save.png";
            }

            aiFindHelper.IsRunning = false;
        }

        private void OnClickShare(object sender, EventArgs e)
        {
            aiFindHelper.IsRunning = true;

            CrossShare.Current.Share(new ShareMessage {  Text="Test", Title="Test Title", Url="www.google.com"});

            aiFindHelper.IsRunning = false;
        }
    }
}