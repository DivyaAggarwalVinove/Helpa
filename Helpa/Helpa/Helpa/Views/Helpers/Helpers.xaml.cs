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
        public ActivityIndicator activityIndicator;
        public CustomMap map;
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
            activityIndicator = aiFindHelper;

            NavigationPage.SetHasNavigationBar(this, false);

            helpersViewModel = new HelpersViewModel(this, mapHelper);

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

            
            //Page page=null;
            //MessagingCenter.Subscribe<Page>(page, "true", (sender) => {
            //    // do something whenever the "Hi" message is sent
            //});
            
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            if (App.selectedPage != 4)
                return;

            RegisterUserModel loggedUser = App.Database.GetLoggedUser();
            if (loggedUser == null)
                return;

            ProfileAfterLoginPage profileAfterLoginPage = new ProfileAfterLoginPage(loggedUser);
            // profileAfterLoginPage.BindingContext = new ProfileAfterLoginViewModel(loggedUser);
            ProfilePage.pcv.Content = profileAfterLoginPage.Content;

            // await GetRuntimeLocationPermission(5000);
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
                    if (h.ProfilePicture == null)
                        h.ProfilePicture = "profile_default.png";

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

                            if (h.AverageRating == null)
                                h.AverageRating = "0";

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

        void HelperSelectedFullList(object sender, SelectedItemChangedEventArgs args)
        {
            var selectedItem = args.SelectedItem as HelperHome;
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
            try
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
            catch(Exception e)
            {

            }
        }
        
        char filteredLocationType = '\0';
        Place selectedPlace = null;
        ServiceModel selectedService = new ServiceModel();
        ScopeModel selectedScope = new ScopeModel();
        // string filteredSortBy = "";
        private async void OnClickServiceFilter(object sender, EventArgs e)
        {
            aiFindHelper.IsRunning = true;

            IList<ServiceModel> servicesAsync = (await new Utilities().GetServicesAsync());
            var servicesName = servicesAsync.Select(x => x.ServiceName);

            if (servicesName != null)
            {
                string filteredService = await DisplayActionSheet("Select service", "Cancel", null, servicesName.ToArray());
                if (!filteredService.Equals("Cancel"))
                {
                    #region reset scope filter
                    lblScopeFilter.Text = "Scope";
                    selectedScope = new ScopeModel();
                    #endregion

                    //if (mapHelper.IsVisible)
                    //{
                    selectedService = servicesAsync.Where(x => x.ServiceName == filteredService).FirstOrDefault();

                    if (selectedService != null)
                    {
                        #region Update list on map
                        HelpersServices helpersServices = new HelpersServices();

                        IEnumerable<HelperHomeModel> h = new List<HelperHomeModel>();
                        if (selectedPlace != null)
                            h = await helpersServices.GetHelpersList(0, selectedService.Id, 0, selectedPlace.Latitude, selectedPlace.Longitude);
                        else
                            h = await helpersServices.GetHelpersList(0, selectedService.Id);

                        if (h != null)
                        {
                            if (filteredLocationType != '\0')
                                h = h.Where(x => x.LocationType.Equals(filteredLocationType));

                            if (h != null)
                            {
                                mapHelper.helperList = h.ToList();

                                //var h = helpersViewModel.helperHomeFilterList.Where(x => x.LocationType == filteredLocationType);
                                helpersViewModel.helperHomeFilterList = new ObservableCollection<HelperHomeModel>(h);
                                helpersViewModel.SetLocationOnMap(helpersViewModel.helperHomeFilterList);
                            }
                            else
                            {
                                await DisplayAlert("", "No Helper found.", "OK");
                            }
                        }
                        else
                        {
                            await DisplayAlert("", "No Helper found.", "OK");
                        }
                        #endregion

                        #region Update full list
                        helpersViewModel.helperFullFilterList = new ObservableCollection<HelperHome>();
                        foreach (HelperHome hh in helpersViewModel.HelperFullList)
                        {
                            var hServices = hh.Service.Where(x => x.ServiceName == selectedService.ServiceName);

                            if (hServices != null && filteredLocationType != '\0')
                                hServices = hServices.Where(x => x.LocationType == filteredLocationType.ToString());

                            if (hServices.Count() != 0)
                                helpersViewModel.helperFullFilterList.Add(hh);
                        }

                        lvFullHelpa.ItemsSource = helpersViewModel.helperFullFilterList;
                        lblHelperFullCount.Text = helpersViewModel.helperFullFilterList.Count + " Helpers found";
                        #endregion

                        lblServiceFilter.Text = filteredService;
                    }
                    //}
                    //else
                    //{
                    //    selectedService = servicesAsync.Where(x => x.ServiceName == filteredService).FirstOrDefault();

                    //    if (selectedService != null)
                    //    {
                    //        //var hServices = helpersViewModel.HelperFullList.Select(x => x.Service);
                    //        helpersViewModel.helperFullFilterList = new ObservableCollection<HelperHome>();
                    //        foreach (HelperHome h in helpersViewModel.HelperFullList)
                    //        {
                    //            var hServices = h.Service.Where(x => x.ServiceName == selectedService.ServiceName);

                    //            if (hServices != null && filteredLocationType != '\0')
                    //                hServices = hServices.Where(x => x.LocationType == filteredLocationType.ToString());

                    //            if (hServices.Count() != 0)
                    //                helpersViewModel.helperFullFilterList.Add(h);
                    //        }

                    //        lblServiceFilter.Text = filteredService;
                    //        lvFullHelpa.ItemsSource = helpersViewModel.helperFullFilterList;
                    //        lblHelperFullCount.Text = helpersViewModel.helperFullFilterList.Count + " Helpers found";
                    //    }
                }
                /*
                var filteredList = helpersViewModel.helperHomeList.Where(x => x.HelpersInLocalties.ServiceName == filteredService);
                helpersViewModel.SetLocationOnMap(new ObservableCollection<HelperHomeModel>(filteredList));
                */
            }

            aiFindHelper.IsRunning = false;
        }

        private async void OnClickLocationFilter(object sender, EventArgs e)
        {
            var result = await DisplayActionSheet("Select location type", "Cancel", null, "Home Helper", "Mobile's Helper", "All");
            
            if (result.Equals("Home Helper"))
            {
                var filteredList = helpersViewModel.helperHomeFilterList.Where(x => x.LocationType == 'S');

                helpersViewModel.SetLocationOnMap(new ObservableCollection<HelperHomeModel>(filteredList));

                //var filteredFullList = helpersViewModel.helperFullFilterList.Where(x => x.LocationType == 'S');
                //lvFullHelpa.ItemsSource = helpersViewModel.helperFullFilterList;
                //lblHelperFullCount.Text = helpersViewModel.helperFullFilterList.Count + " Helpers found";

                filteredLocationType = 'S';

                lblLocationFilter.Text = result;
            }
            else if (result.Equals("All"))
            {
                //helpersViewModel.helperHomeFilterList = helpersViewModel.helperHomeList;
                helpersViewModel.SetLocationOnMap(helpersViewModel.helperHomeFilterList);

                filteredLocationType = '\0';

                lblLocationFilter.Text = "Location types";

            }
            else if(result.Equals("Cancel"))
            {
            }
            else
            {
                var filteredList = helpersViewModel.helperHomeFilterList.Where(x => x.LocationType.Equals('M'));
                helpersViewModel.SetLocationOnMap(new ObservableCollection<HelperHomeModel>(filteredList));

                filteredLocationType = 'M';

                lblLocationFilter.Text = result;
            }
        }

        private async void OnClickScopeFilter(object sender, EventArgs e)
        {
            aiFindHelper.IsRunning = true;

            if (selectedService != null && selectedService.Id != 0)
            {
                IList<ScopeModel> scopeAsync = (await new Utilities().GetScpoesAsync(selectedService.Id));
                var scopesName = scopeAsync.Select(x => x.ScopeName);

                if (scopesName != null)
                {
                    string filteredScope = await DisplayActionSheet("Select Scope", "Cancel", null, scopesName.ToArray());

                    if (mapHelper.IsVisible)
                    {
                        selectedScope = scopeAsync.Where(x => x.ScopeName == filteredScope).FirstOrDefault();

                        if (selectedScope != null)
                        {
                            HelpersServices helpersServices = new HelpersServices();
                            //var h = await helpersServices.GetHelpersList(0, selectedService.Id, selectedScope.Id);
                            IEnumerable<HelperHomeModel> h = new List<HelperHomeModel>();
                            if (selectedPlace != null)
                                h = await helpersServices.GetHelpersList(0, selectedService.Id, selectedScope.Id, selectedPlace.Latitude, selectedPlace.Longitude);
                            else
                                h = await helpersServices.GetHelpersList(0, selectedService.Id, selectedScope.Id);

                            if (h != null)
                            {
                                if (filteredLocationType != '\0')
                                    h = h.Where(x => x.LocationType.Equals(filteredLocationType));

                                if (h != null)
                                {
                                    mapHelper.helperList = h.ToList();

                                    //var h = helpersViewModel.helperHomeFilterList.Where(x => x.LocationType == filteredLocationType);
                                    helpersViewModel.helperHomeFilterList = new ObservableCollection<HelperHomeModel>(h);
                                    helpersViewModel.SetLocationOnMap(helpersViewModel.helperHomeFilterList);
                                }
                                else
                                {
                                    await DisplayAlert("", "No Helper found.", "OK");
                                }
                            }
                            else
                            {
                                await DisplayAlert("", "No Helper found.", "OK");
                            }

                            lblScopeFilter.Text = filteredScope;
                        }
                    }
                    else
                    {
                    }
                    /*
                        var filteredList = helpersViewModel.helperHomeList.Where(x => x.Servicename == filteredService);
                        helpersViewModel.SetLocationOnMap(new ObservableCollection<HelperHomeModel>(filteredList));
                    */
                }
            }
            else
            {
                await DisplayAlert("", "Please select service", "Ok");
            }    

            aiFindHelper.IsRunning = false;
        }

        private async void OnClickSortByFilter(object sender, EventArgs e)
        {
            string sortByFilter = await DisplayActionSheet("Sort By", "Cancel", null, "Newest", "Rating","Popularity");
            if (sortByFilter == null || sortByFilter.Equals("Cancel"))
            {
            }
            else
            {
                lblSortByFilter.Text = sortByFilter;
            }
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

        private async void OnLocationSelected(object sender, ItemTappedEventArgs e)
        {
            string selectedLoc = ((ListView)sender).SelectedItem.ToString();
            entrySearch.Text = selectedLoc;

            AutoCompletePrediction  p = selectedPrediction.Where(x => x.Description == selectedLoc).FirstOrDefault();
            selectedPlace = await Places.GetPlace(p.Place_ID, Constants.googlePlaceApiKey);

            listView.IsVisible = false;
            listView.SelectedItem = null;

            flag = false;
        }

        private void OnSearchTextChange(object sender, TextChangedEventArgs e)
        {
            PlacesBar pb = (PlacesBar)sender;
            if (string.IsNullOrEmpty(pb.Text))
            {
                listView.IsVisible = false;
                listView.SelectedItem = null;

                selectedPlace = null;

                return;
            }
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
                selectedPlace= await Places.GetPlace(loc.Place_ID, Constants.googlePlaceApiKey);
               

                HelpersServices helpersServices = new HelpersServices();
                var h         = await helpersServices.GetHelpersList(0, selectedService.Id, selectedScope.Id, selectedPlace.Latitude, selectedPlace.Longitude);
                
                if (filteredLocationType != '\0')
                {
                    h = h.Where(x => x.LocationType.Equals(filteredLocationType));
                }

                if (h != null && h.Count() != 0)
                    helpersViewModel.SetLocationOnMap(new ObservableCollection<HelperHomeModel>(h));
            }
        }

        private async void OnClickBookmark(object sender, TappedEventArgs e)
        {
            aiFindHelper.IsRunning = true;

            RegisterUserModel loggedUser = App.Database.GetLoggedUser();
            if (loggedUser == null)
            {
                await Application.Current.MainPage.Navigation.PushAsync(new LoginPage());
            }
            else
            {
                var imgBookmark = ((Image)sender);
                if (imgBookmark.Source.ToString().Contains("save.png"))
                {
                    if (e.Parameter != null)
                    {
                        int helperId = int.Parse(e.Parameter.ToString());

                        bool isSuccess = await (new HelpersServices()).BookMarkHelper(loggedUser.Id, helperId);
                        imgBookmark.Source = "save_filled.png";
                    }
                }
                else
                {
                    int helperId = int.Parse(e.Parameter.ToString());
                    
                    bool isSuccess = await (new HelpersServices()).UnBookMarkHelper(loggedUser.Id, helperId);
                    imgBookmark.Source = "save.png";
                }
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