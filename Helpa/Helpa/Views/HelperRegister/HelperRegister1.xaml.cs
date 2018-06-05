using AsNum.XFControls;
using Helpa.Models;
using Helpa.Utility;
using Plugin.Geolocator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Helpa
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HelperRegister1 : ContentPage
    {
        RegisterUserModel currentUser;
        IEnumerable<ServiceModel> services;
        public HelperRegister1(RegisterUserModel user)
        {
            InitializeComponent();

            NavigationPage.SetHasNavigationBar(this, false);
            currentUser = user;

            btnHelperRegSelectedService.isSelected = true;

            entryHelperRegUsername1.Text = user.UserName;
            entryHelperRegPhone1.Text = user.PhoneNumber;
            entryHelperRegEmail1.Text = user.Email;

            SetGender(user.Gender);
            SetStatus();
            SetServices();
            //SetLocation();

            if (user.IsVerified)
            {
                GoToEditServices();
            }
            else
            {
                GoToBasicInfo();
            }

            MessagingCenter.Subscribe<ServiceButton, bool>(this, "Select Or Unselect Service", (sender, isSelected) =>
            {
                try
                {
                    string serviceName = ((ServiceButton)sender).serviceName;
                    var selectedService = services.Where(a => a.ServiceName == serviceName).First();
                    int serviceIndex = services.ToList().IndexOf(selectedService);
                    var gService = gridServices.Children.Where(c => Grid.GetRow(c) == serviceIndex / 3 && Grid.GetColumn(c) == serviceIndex % 3);
                    gService.ElementAt(1).IsVisible = !gService.ElementAt(1).IsVisible;

                    if (isSelected)
                    {
                        AddGrid();
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

                        for(int i=0;i<count-1;i++)
                            AddGrid();

                        selectedService.isSelected = false;
                        App.Database.SaveServiceAsync(selectedService);
                    }

                    var g = gridHelperEditServices.Children.Where(c => Grid.GetRow(c) == 0 && Grid.GetColumn(c) == 0).First();
                    g.BackgroundColor = Color.FromHex("#FF748C");
                }
                catch(Exception e)
                {
                    Console.Write(e.StackTrace);
                }
            });
        }

        void AddGrid()
        {
            gridHelperEditServices.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.1, GridUnitType.Star) });

            Grid grid = new Grid();
            grid.HorizontalOptions = LayoutOptions.FillAndExpand;
            grid.VerticalOptions = LayoutOptions.FillAndExpand;
            grid.BackgroundColor = Color.FromHex("#818A8F");
            gridHelperEditServices.Children.Add(grid, gridHelperEditServices.ColumnDefinitions.Count - 1, 0);
            gridHelperEditServices.BackgroundColor = Color.Transparent;
        }

        void SetGender(string gender)
        {
            IEnumerable<string> genderList = new List<string>() { "Male", "Female", "Rather no to say" };
            rgHelperGender.ItemsSource = genderList;

            if (gender != null)
                rgHelperGender.SelectedItem = Utils.ToTitleCase(gender);

            StackLayout content = (StackLayout)rgHelperGender.Content;
            Radio rg = null;
            for (int i = 0; i < content.Children.Count; i++)
            {
                rg = (Radio)(content.Children[i]);
                rg.Margin = new Thickness(0, 0, 20, 0);
                rg.VerticalOptions = LayoutOptions.Center;
            }
        }
        
        void SetStatus()
        {
            IEnumerable<string> statusList = new List<string>() { "Available", "Not available" };
            rgHelperStatus.ItemsSource = statusList;
            StackLayout content = (StackLayout)rgHelperStatus.Content;
            Radio rg = null;
            for (int i = 0; i < content.Children.Count; i++)
            {
                rg = (Radio)(content.Children[i]);
                rg.Margin = new Thickness(0, 0, 20, 0);
                rg.VerticalOptions = LayoutOptions.Center;
            }
        }

        async void SetServices()
        {
            #region set services
            services = await (new Services.Services()).GetServicesAsync();

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

        async void SetLocation()
        { 
            var locator = CrossGeolocator.Current;
            TimeSpan ts = TimeSpan.FromTicks(1000000);
            Plugin.Geolocator.Abstractions.Position position = await locator.GetPositionAsync(ts);
            var addr = await locator.GetAddressesForPositionAsync(new Plugin.Geolocator.Abstractions.Position(position.Latitude, position.Longitude), "AIzaSyDminfXt_CoSb9UTXpPFZwQIG2lDduDMjs");

            var a = addr.FirstOrDefault();
            entryHelperRegSearch.Text = a.FeatureName + "," + a.SubLocality + "," + a.Locality + "," + a.CountryName + "," + a.PostalCode;
        }

        void OnHelperRegBasicInfoNext(object sender, EventArgs args)
        {
            if (string.IsNullOrEmpty(entryHelperRegUsername1.Text))
            {
                DisplayAlert("Warning", "Please enter username", "Ok");
            }
            else if (string.IsNullOrEmpty(entryHelperRegPhone1.Text))
            {
                DisplayAlert("Warning", "Please enter phone number", "Ok");
            }
            else if (string.IsNullOrEmpty(entryHelperRegEmail1.Text))
            {
                DisplayAlert("Warning", "Please enter email", "Ok");
            }
            else if (Utils.IsValidMobileNo(entryHelperRegPhone1.Text))
            {
                DisplayAlert("Warning", "Please enter valid mobile number", "Ok");
            }
            else if (!Utils.IsValidEmail(entryHelperRegEmail1.Text))
            {
                DisplayAlert("Warning", "Please enter valid email", "Ok");
            }
            else
            {
                currentUser.IsVerified = true;
                currentUser.PhoneNumber = entryHelperRegPhone1.Text;
                currentUser.Gender = rgHelperGender.SelectedItem.ToString();
                currentUser.Email = entryHelperRegEmail1.Text;
                
                App.Database.SaveUserAsync(currentUser);
                GoToEditServices();
                //Navigation.PushAsync(new Register1());
            }
        }

        void OnHelperRegEditServicesNext(object sender, EventArgs args)
        {
            List<ServiceModel> selectedServices = App.Database.GetServicesAsync();
            if (selectedServices.Count == 0)
                DisplayAlert("Warning", "Please select at least any one service.", "Ok");
            else
            {
                GotoNextService(selectedServices, 0);
            }
        }

        void GotoNextService(List<ServiceModel> selectedServices, int i)
        {
            if (i >= selectedServices.Count)
                return;

            var g = gridHelperEditServices.Children.ElementAt(i + 1);
            g.BackgroundColor = Color.FromHex("#FF748C");

            svHelperEditServices.IsVisible = false;
            svHelperRegHelperHome.IsVisible = true;

            ServiceModel service = selectedServices.ElementAt(i);
            //svHelperRegHelperHome.IsVisible = true;
        }

        void OnSignUpDone(object sender, EventArgs args)
        {
        }

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
            lHelperSignUp.Text = "Helper Sign Up 1/3";
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
            lHelperSignUp.Text = "Helper Sign Up 2/3";
            gridHelperEditServices.BackgroundColor = Color.Transparent;
            gridHelperEditServices.Children.ElementAt(0).BackgroundColor = Color.FromHex("#FF748C");
            svHelperBasicInfo.IsVisible = false;
            svHelperEditServices.IsVisible = true;
        }

        void BuildTrust()
        {
        }
    }
}