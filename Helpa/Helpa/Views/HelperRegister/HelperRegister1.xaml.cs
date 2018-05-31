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

        public HelperRegister1(RegisterUserModel user)
        {
            InitializeComponent();

            NavigationPage.SetHasNavigationBar(this, false);
            currentUser = user;

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

            //MessagingCenter.Subscribe<HelperRegister3, string>(this, "Selected Address", (sender, address) =>
            //{
            //    entryHelperRegSearch.Text = address;
            //});

            //MessagingCenter.Subscribe<HelperRegister2>(this, "Current Address", (sender) =>
            //{
            //    SetLocation();
            //});
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
            IEnumerable<ServiceModel> services = await (new Services.Services()).GetServicesAsync();

            int servicesCount = services.Count();
            for (int i = 0; i < (servicesCount-1)/3+1; i++)
                gridServices.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            //gridServices.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            for (int i = 0; i < 3; i++)
                gridServices.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

            ServiceButton serviceButton;
            for (int i = 0; i < servicesCount; i++)
            {
                serviceButton = new ServiceButton();
                serviceButton.Text = services.ElementAt(i).ServiceName;
                gridServices.Children.Add(serviceButton, i % 3, i / 3);
            }

            //serviceButton = new ServiceButton();
            //serviceButton.Text = "      ";
            //gridServices.Children.Add(serviceButton, i % 3, i / 3);
            #endregion
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

        void OnHelperSignUpNext1(object sender, EventArgs args)
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

        void OnHelperSignUpNext2(object sender, EventArgs args)
        {
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
            lHelperSignUp.Text = "Helper Sign Up 1/2";
            gridHelperEditServices.BackgroundColor = Color.FromHex("#818A8F");
            svHelperBasicInfo.IsVisible = true;
            svHelperEditServices.IsVisible = false;
        }

        void GoToEditServices()
        {
            lHelperSignUp.Text = "Helper Sign Up 2/3";
            gridHelperEditServices.BackgroundColor = Color.FromHex("#FF748C");
            svHelperBasicInfo.IsVisible = false;
            svHelperEditServices.IsVisible = true;
        }

        void BuildTrust()
        {

        }
    }
}