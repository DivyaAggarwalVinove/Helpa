using AsNum.XFControls;
using Helpa.Models;
using Helpa.Services;
using Helpa.Utility;
using Plugin.Geolocator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.RangeSlider.Forms;

namespace Helpa
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HelperCompleteRegister : ContentPage
    {
        RegisterUserModel currentUser;
        IList<ServiceModel> services;
        IList<ScopeModel> scopes;
        static int currentService;

        private static HelperCompleteRegister instance;
        public static HelperCompleteRegister Instance
        {
            get { return instance; }
            set { instance = value; }
        }

        public HelperCompleteRegister(RegisterUserModel user)
        {
            InitializeComponent();

            currentService = 0;
            instance = this;
            NavigationPage.SetHasNavigationBar(this, false);
            currentUser = user;

            btnHelperRegSelectedService.isSelected = true;
            btnHelperRegTbd.isSelected = true;

            entryHelperRegUsername1.Text = user.UserName;
            entryHelperRegPhone1.Text = user.MobileNumber;
            entryHelperRegEmail1.Text = user.Email;

            IEnumerable<string> genders = new List<string>() { "Male", "Female", "Rather no to say" };
            SetRadioList(genders, rgHelperGender);

            //IEnumerable<String> locationTypes = new List<string>() { "Helper's Home", "Mobile Helper"};
            //SetRadioList(locationTypes, rgHelperLocationType);

            IEnumerable<string> statusList = new List<string>() { "Available", "Not available" };
            SetRadioList(statusList, rgHelperStatus);

            SetServices();
            SetScopes();

            if (user.IsServiced)
                GoToBuildTrust();
            else if (user.IsCompleted)
                GoToEditServices();
            else
            if (user.IsRegistered)
            {
                GoToBasicInfo();
            }

            MessagingCenter.Subscribe<ServiceButton, bool>(this, "Select Or Unselect Service", (sender, isSelected) =>
            {
                try
                {
                    string serviceName = ((ServiceButton)sender).serviceName;

                    if (string.Equals(serviceName, "Hourly"))
                    {
                        if (((ServiceButton)sender).isSelected)
                            gridPriceHour.IsVisible = true;
                        else
                            gridPriceHour.IsVisible = false;
                    }
                    else if (string.Equals(serviceName, "Daily"))
                    {
                        if (((ServiceButton)sender).isSelected)
                            gridPriceDay.IsVisible = true;
                        else
                            gridPriceDay.IsVisible = false;
                    }
                    else if (string.Equals(serviceName, "Monthly"))
                    {
                        if (((ServiceButton)sender).isSelected)
                            gridPriceMonth.IsVisible = true;
                        else
                            gridPriceMonth.IsVisible = false;
                    }
                    else
                    {
                        var selectedService = services.Where(a => a.ServiceName == serviceName).FirstOrDefault();
                        int serviceIndex = services.ToList().IndexOf(selectedService);
                        var gService = gridServices.Children.Where(c => Grid.GetRow(c) == serviceIndex / 3 && Grid.GetColumn(c) == serviceIndex % 3);
                        gService.ElementAt(1).IsVisible = !gService.ElementAt(1).IsVisible;

                        if (isSelected)
                        {
                            AddGrid(gridHelperEditServices);
                            selectedService.isSelected = true;
                            App.Database.SaveServiceAsync(selectedService);
                        }
                        else
                        {
                            int count = gridHelperEditServices.ColumnDefinitions.Count;

                            for (int i = count - 1; i >= 0; i--)
                            {
                                gridHelperEditServices.ColumnDefinitions.RemoveAt(i);
                                gridHelperEditServices.Children.RemoveAt(i);
                            }

                            for (int i = 0; i < count - 1; i++)
                                AddGrid(gridHelperEditServices);

                            selectedService.isSelected = false;
                            App.Database.SaveServiceAsync(selectedService);
                        }

                        var g = gridHelperEditServices.Children.Where(c => Grid.GetRow(c) == 0 && Grid.GetColumn(c) == 0).First();
                        g.BackgroundColor = Color.FromHex("#FF748C");
                    }
                }
                catch (Exception e)
                {
                    Console.Write(e.StackTrace);
                }
            });

            MessagingCenter.Subscribe<ScopeButton, bool>(this, "Select Or Unselect Service", (sender, isSelected) =>
            {
                try
                {
                    string scopeName = ((ScopeButton)sender).scopeName;

                    var selectedScope = scopes.Where(a => a.ScopeName == scopeName).FirstOrDefault();
                    if (isSelected)
                        selectedScope.isSelected = true;
                    else
                        selectedScope.isSelected = false;

                    App.Database.SaveScopeAsync(selectedScope);
                }
                catch (Exception e)
                {
                    Console.Write(e.StackTrace);
                }
            });
        }

        void AddGrid(Grid grid)
        {
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.1, GridUnitType.Star) });

            Grid g = new Grid();
            g.HorizontalOptions = LayoutOptions.FillAndExpand;
            g.VerticalOptions = LayoutOptions.FillAndExpand;
            g.BackgroundColor = Color.FromHex("#818A8F");
            grid.Children.Add(g, grid.ColumnDefinitions.Count - 1, 0);
            grid.BackgroundColor = Color.Transparent;
        }

        void OnHomeLocationSelected(object sender, EventArgs args)
        {
            //rgHelperHomeLocation.Checked = !rgHelperHomeLocation.Checked;
            if (rgHelperHomeLocation.Checked)
            {
                rgHelperMobileLocation.Checked = !rgHelperMobileLocation.Checked;

                btnHelperRegDistrict.IsVisible = false;
                entryHelperRegLocation.IsVisible = true;
            }
            else
                rgHelperHomeLocation.Checked = !rgHelperHomeLocation.Checked;
        }

        void OnMobileLocationSelected(object sender, EventArgs args)
        {
            if (rgHelperMobileLocation.Checked)
            {
                rgHelperHomeLocation.Checked = !rgHelperHomeLocation.Checked;

                btnHelperRegDistrict.IsVisible = true;
                entryHelperRegLocation.IsVisible = false;
            }
            else
                rgHelperMobileLocation.Checked = !rgHelperMobileLocation.Checked;
            //rgHelperMobileLocation.Checked = !rgHelperMobileLocation.Checked;
        }

        void SetRadioList(IEnumerable<string> genderList, RadioGroup radioGroup)
        {
            radioGroup.ItemsSource = genderList;

            radioGroup.SelectedItem = genderList.ElementAt(0);

            StackLayout content = (StackLayout)radioGroup.Content;
            Radio rg = null;
            for (int i = 0; i < content.Children.Count; i++)
            {
                rg = (Radio)(content.Children[i]);
                rg.Margin = new Thickness(0, 0, 20, 0);
                rg.VerticalOptions = LayoutOptions.Center; 
            }
        }

        //void SetStatus()
        //{
        //    IEnumerable<string> statusList = new List<string>() { "Available", "Not available" };
        //    rgHelperStatus.ItemsSource = statusList;
        //    StackLayout content = (StackLayout)rgHelperStatus.Content;
        //    Radio rg = null;
        //    for (int i = 0; i < content.Children.Count; i++)
        //    {
        //        rg = (Radio)(content.Children[i]);
        //        rg.Margin = new Thickness(0, 0, 20, 0);
        //        rg.VerticalOptions = LayoutOptions.Center;
        //    }
        //}

        async void SetServices()
        {
            #region set services
            services = await (new Utilities()).GetServicesAsync();

            int servicesCount = services.Count();
            for (int i = 0; i < (servicesCount - 1) / 3 + 1; i++)
                gridServices.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            for (int i = 0; i < 3; i++)
                gridServices.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

            ServiceButton serviceButton;
            for (int i = 0; i < servicesCount; i++)
            {
                serviceButton = new ServiceButton();
                serviceButton.Text = services.ElementAt(i).ServiceName;
                serviceButton.Margin = new Thickness(10, 5, 10, 5);

                Image image = new Image();
                image.Source = "selected.png";
                image.Aspect = Aspect.AspectFit;
                image.Margin = new Thickness(20, 15, 0, 15);
                image.HorizontalOptions = LayoutOptions.End;
                image.IsVisible = false;

                gridServices.Children.Add(serviceButton, i % 3, i / 3);
                gridServices.Children.Add(image, i % 3, i / 3);                
                #endregion
            }

            await App.Database.SaveServicesAsync(services);
        }

        async void SetScopes()
        {
            #region set scopes
            scopes = await (new Utilities()).GetScpoesAsync();

            int scopesCount = scopes.Count();
            for (int i = 0; i < (scopesCount - 1) / 3 + 1; i++)
                gridScopes.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            for (int i = 0; i < 3; i++)
                gridScopes.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

            ScopeButton scopeButton;
            for (int i = 0; i < scopesCount; i++)
            {
                scopeButton = new ScopeButton();
                scopeButton.Text = scopes.ElementAt(i).ScopeName;
                scopeButton.Margin = new Thickness(10, 5, 10, 5);

                Image image = new Image();
                image.Source = "selected.png";
                image.Aspect = Aspect.AspectFit;
                image.Margin = new Thickness(20, 15, 0, 15);
                image.HorizontalOptions = LayoutOptions.End;
                image.IsVisible = false;

                gridScopes.Children.Add(scopeButton, i % 3, i / 3);
                gridScopes.Children.Add(image, i % 3, i / 3);
                #endregion
            }

            await App.Database.SaveScopeAsync(scopes);
        }
        //async void SetLocation()
        //{ 
        //    var locator = CrossGeolocator.Current;
        //    TimeSpan ts = TimeSpan.FromTicks(1000000);
        //    Plugin.Geolocator.Abstractions.Position position = await locator.GetPositionAsync(ts);
        //    var addr = await locator.GetAddressesForPositionAsync(new Plugin.Geolocator.Abstractions.Position(position.Latitude, position.Longitude), "AIzaSyDminfXt_CoSb9UTXpPFZwQIG2lDduDMjs");

        //    var a = addr.FirstOrDefault();
        //    entryHelperRegSearch.Text = a.FeatureName + "," + a.SubLocality + "," + a.Locality + "," + a.CountryName + "," + a.PostalCode;
        //}

        async void OnHelperRegBasicInfoNext(object sender, EventArgs args)
        {
            if (string.IsNullOrEmpty(entryHelperRegUsername1.Text))
            {
                await DisplayAlert("Warning", "Please enter username", "Ok");
            }
            else if (string.IsNullOrEmpty(entryHelperRegPhone1.Text))
            {
                await DisplayAlert("Warning", "Please enter phone number", "Ok");
            }
            else if (string.IsNullOrEmpty(entryHelperRegEmail1.Text))
            {
                await DisplayAlert("Warning", "Please enter email", "Ok");
            }
            else if (Utils.IsValidMobileNo(entryHelperRegPhone1.Text))
            {
                await DisplayAlert("Warning", "Please enter valid mobile number", "Ok");
            }
            else if (!Utils.IsValidEmail(entryHelperRegEmail1.Text))
            {
                await DisplayAlert("Warning", "Please enter valid email", "Ok");
            }
            else
            {
                currentUser.IsVerified = true;
                currentUser.MobileNumber = entryHelperRegPhone1.Text;
                try
                {
                    Gender g = (Gender)Enum.Parse(typeof(Gender), rgHelperGender.SelectedItem.ToString());
                    currentUser.Gender = (int)g;
                }
                catch(Exception e)
                {
                    Console.Write(e.StackTrace);
                    currentUser.Gender = 3;
                }
                //currentUser.Gender = rgHelperGender.SelectedItem.ToString();
                currentUser.Email = entryHelperRegEmail1.Text;

                await (new RegisterServices()).CompleteRegisterService(currentUser);
                //App.Database.SaveUserAsync(currentUser);
                //GoToEditServices();
            }
        }

        public void GotoNext(RegisterUserModel userModel)
        {
            userModel.IsCompleted = true;

            App.Database.SaveUserAsync(userModel);
            GoToEditServices();
            //Navigation.PushAsync(new HelperRegister1(userModel));
        }

        void OnHelperRegEditServicesNext(object sender, EventArgs args)
        {
            List<ServiceModel> selectedServices = App.Database.GetServicesAsync();
            if (selectedServices.Count == 0)
                DisplayAlert("Warning", "Please select at least any one service.", "Ok");
            else
            {
                //OnCallNextService(sender, args);
                GotoNextService(selectedServices, currentService);
            }
        }

        void OnCallNextService(object sender, EventArgs args)
        {
            List<ServiceModel> selectedServices = App.Database.GetServicesAsync();

            if (currentService >= selectedServices.Count)
            {
                GoToBuildTrust();
                return; // Call HelperServices and go to Build Trust
            }

            ServiceModel service = services.Where(i => i.Id == selectedServices.ElementAt(currentService).Id).FirstOrDefault();
            service = selectedServices.ElementAt(currentService);
            if (rgHelperHomeLocation.Checked)
                service.LocationType = 1;
            else
                service.LocationType = 2;

            if (btnHelperRegHourly.isSelected)
            {
                service.Hour = true;
                service.MinPriceHour = float.Parse(btnHelperRegPriceHrMin.Text.Substring(2));
                service.MaxPriceHour = float.Parse(btnHelperRegPriceHrMax.Text.Substring(2));
            }

            if (btnHelperRegDaily.isSelected)
            {
                service.Day = true;
                service.MinDayPrice = float.Parse(btnHelperRegPriceDayMin.Text.Substring(2));
                service.MaxDayPrice = float.Parse(btnHelperRegPriceDayMax.Text.Substring(2));
            }

            if (btnHelperRegMonthly.isSelected)
            {
                service.Month = true;
                service.MinMonthPrice = float.Parse(btnHelperRegPriceMonthMin.Text.Substring(2));
                service.MaxMonthPrice = float.Parse(btnHelperRegPriceMonthMax.Text.Substring(2));
            }

            scopes = App.Database.GetScopesAsync();

            //SaveService(service, currentService);
            //Save Service into database
            App.Database.SaveServiceAsync(service);

            GotoNextService(selectedServices, ++currentService);
        }
                
        void GotoNextService(List<ServiceModel> selectedServices, int i)
        {
            if (i >= selectedServices.Count)
                return;
            
            var g = gridHelperEditServices.Children.ElementAt(i+1);
            g.BackgroundColor = Color.FromHex("#FF748C");

            svHelperEditServices.IsVisible = false;
            svHelperRegHelperHome.IsVisible = true;

            ServiceModel service = selectedServices.ElementAt(i);
            btnHelperRegSelectedService.Text = service.ServiceName;

            //svHelperRegHelperHome.IsVisible = true;
        }

        void SaveService(ServiceModel serviceModel, int serviceIndex)
        {
            services[serviceIndex] = serviceModel;

            App.Database.SaveServiceAsync(serviceModel);
        }
        
        void SetYearOfExperience(object sender, EventArgs args) => 
            btnHelperRegExpCount.Text = "$ " + ((RangeSlider)sender).UpperValue.ToString();

        void SetMinHour(object sender, EventArgs args) => 
            btnHelperRegPriceHrMin.Text = "$ " + ((RangeSlider)sender).LowerValue.ToString();

        void SetMaxHour(object sender, EventArgs args) =>
            btnHelperRegPriceHrMax.Text = "$ " + ((RangeSlider)sender).UpperValue.ToString();
        
        void SetMinDay(object sender, EventArgs args) =>
            btnHelperRegPriceDayMin.Text = "$ " + ((RangeSlider)sender).LowerValue.ToString();
        
        void SetMaxDay(object sender, EventArgs args) =>
            btnHelperRegPriceDayMax.Text = "$ " + ((RangeSlider)sender).UpperValue.ToString();
        
        void SetMinMonth(object sender, EventArgs args) =>
            btnHelperRegPriceMonthMin.Text = "$ " + ((RangeSlider)sender).LowerValue.ToString();

        void SetMaxMonth(object sender, EventArgs args) =>
            btnHelperRegPriceMonthMax.Text = "$ " + ((RangeSlider)sender).UpperValue.ToString();
        
        void SetMinAge(object sender, EventArgs args) =>
            btnHelperRegAgeMin.Text = "$ " + ((RangeSlider)sender).LowerValue.ToString();

        void SetMaxAge(object sender, EventArgs args) =>
            btnHelperRegAgeMax.Text = "$ " + ((RangeSlider)sender).UpperValue.ToString();

        protected override bool OnBackButtonPressed()
        {
            if (svHelperEditServices.IsVisible)
            {
                GoToBasicInfo();
                return true;
            }
            else
            {
                return base.OnBackButtonPressed();
            }
        }

        void OnFocus(object sender, EventArgs args)
        {
            Navigation.PushAsync(new HelperRegister2());
        }

        void GoToBasicInfo()
        {
            helperCompleteRegisterTitle.Text = "Helper Sign Up 1/3";
            gridHelperEditServices.BackgroundColor = Color.FromHex("#818A8F");

            for (int i = 0; i < gridHelperEditServices.ColumnDefinitions.Count; i++)
            {
                var g = gridHelperEditServices.Children.ElementAt(i);
                g.BackgroundColor = Color.FromHex("#818A8F");
            }
            svHelperBasicInfo.IsVisible = true;
            svHelperEditServices.IsVisible = false;
        }

        void GoToEditServices()
        {
            helperCompleteRegisterTitle.Text = "Helper Sign Up 2/3";
            gridHelperEditServices.BackgroundColor = Color.Transparent;
            gridHelperEditServices.Children.ElementAt(0).BackgroundColor = Color.FromHex("#FF748C");
            svHelperBasicInfo.IsVisible = false;
            svHelperEditServices.IsVisible = true;
        }

        async void GoToBuildTrust()
        {
            //await (new HelpersServices()).get();

            helperCompleteRegisterTitle.Text = "Helper Sign Up 3/3";
            //svHelperEditServices.IsVisible = false;
            svHelperRegHelperHome.IsVisible = false;
            svHelperBuildTrust.IsVisible = true;
        }
    }
}