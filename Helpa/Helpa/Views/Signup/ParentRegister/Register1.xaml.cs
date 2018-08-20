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

namespace Helpa
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Register1 : ContentPage
    {
        RegisterUserModel currentUser;
        private static Register1 instance;
        public static Register1 Instance
        {
            get { return instance; }
            set { instance = value; }
        }

        public Register1(RegisterUserModel user)
        {
            InitializeComponent();

            currentUser = user;
            instance = this;

            NavigationPage.SetHasNavigationBar(this, false);
            IEnumerable<string> genderList = new List<string>() { "Male", "Female", "Rather no to say" };
            rgGender.ItemsSource = genderList;
            rgGender.SelectedItem = Utils.ToTitleCase("Female");

            entryRegUsername1.Text = user.UserName;
            entryRegPhone1.Text = user.MobileNumber;
            entryRegEmail1.Text = user.Email;

            SetLocation();

            MessagingCenter.Subscribe<Register3, string>(this, "Selected Address", (sender, address) =>
            {
                entryRegSearch.Text = address;
            });

            MessagingCenter.Subscribe<Register2>(this, "Current Address", (sender) =>
            {
                SetLocation();
            });
        }

        async void SetLocation()
        { 
            var locator = CrossGeolocator.Current;
            TimeSpan ts = TimeSpan.FromTicks(1000000);
            Plugin.Geolocator.Abstractions.Position position = await locator.GetPositionAsync(ts);
            var addr = await locator.GetAddressesForPositionAsync(new Plugin.Geolocator.Abstractions.Position(position.Latitude, position.Longitude), "AIzaSyDminfXt_CoSb9UTXpPFZwQIG2lDduDMjs");

            var a = addr.FirstOrDefault();
            entryRegSearch.Text = a.FeatureName + "," + a.SubLocality + "," + a.Locality + "," + a.CountryName + "," + a.PostalCode;
        }

        void OnSignUpNext(object sender, EventArgs args)
        {
            if (string.IsNullOrEmpty(entryRegUsername1.Text))
            {
                DisplayAlert("Warning", "Please enter username", "Ok");
            }
            else if (string.IsNullOrEmpty(entryRegPhone1.Text))
            {
                DisplayAlert("Warning", "Please enter phone number", "Ok");
            }
            else if (string.IsNullOrEmpty(entryRegEmail1.Text))
            {
                DisplayAlert("Warning", "Please enter email", "Ok");
            }
            else if (Utils.IsValidMobileNo(entryRegPhone1.Text))
            {
                DisplayAlert("Warning", "Please enter valid mobile number", "Ok");
            }
            else if (!Utils.IsValidEmail(entryRegEmail1.Text))
            {
                DisplayAlert("Warning", "Please enter valid email", "Ok");
            }
            else
            {
                lParentSignUp.Text = "Parent Sign Up 2/2";
                gridLocation.BackgroundColor = Color.FromHex("#FF748C");
                svBasicInfo.IsVisible = false;
                svLocationInfo.IsVisible = true;

                currentUser.IsVerified = true;
                currentUser.UserName = entryRegUsername1.Text;
                currentUser.MobileNumber = entryRegPhone1.Text;
                try
                {
                    Gender g = (Gender)Enum.Parse(typeof(Gender), rgGender.SelectedItem.ToString());
                    currentUser.Gender = (int)g;
                }
                catch (Exception e)
                {
                    Console.Write(e.StackTrace);
                    currentUser.Gender = 3;
                }
                currentUser.Email = entryRegEmail1.Text;
                currentUser.Role = "Parent".ToUpper();
            }
        }

        async void OnSignUpDone(object sender, EventArgs args)
        {
            if (entryRegSearch.Text.Equals(""))
                await DisplayAlert("Warning", "Please select location.", "Ok");
            else
                await (new RegisterServices()).CompleteRegisterService(currentUser);
        }

        public void GotoNext(RegisterUserModel userModel)
        {
            DisplayAlert("Information", "Sign up successfully.", "Ok");
            
            List<Page> pages = Navigation.NavigationStack.ToList();
            for (int i = pages.Count - 1; i > 0; i--)
            {
                if (!pages[i].ToString().Contains("Login"))
                    Navigation.PopAsync();
                else
                    break;
            }
        }

        public void ShowError(string error)
        {
            DisplayAlert("Error", error, "Ok");
        }

        protected override bool OnBackButtonPressed()
        {
            if (svLocationInfo.IsVisible)
            {
                lParentSignUp.Text = "Parent Sign Up 1/2";
                gridLocation.BackgroundColor = Color.FromHex("#818A8F");
                svBasicInfo.IsVisible = true;
                svLocationInfo.IsVisible = false;

                return true;
            }
            else
            {
                return base.OnBackButtonPressed();
            }
        }

        void OnFocus(object sender, EventArgs args)
        {
            Navigation.PushAsync(new Register2());
        }
    }
}