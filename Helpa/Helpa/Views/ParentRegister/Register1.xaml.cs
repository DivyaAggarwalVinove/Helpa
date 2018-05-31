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
        //RegisterViewModel1 vm = null;

        public Register1(string userName, string gender, string phoneNumber, string email)
        {
            InitializeComponent();

            NavigationPage.SetHasNavigationBar(this, false);
            IEnumerable<string> genderList = new List<string>() { "Male", "Female", "Rather no to say" };
            rgGender.ItemsSource = genderList;
            rgGender.SelectedItem = Utils.ToTitleCase( gender);

            entryRegUsername1.Text = userName;
            entryRegPhone1.Text = phoneNumber;
            entryRegEmail1.Text = email;

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
                //Navigation.PushAsync(new Register1());
            }
        }

        void OnSignUpDone(object sender, EventArgs args)
        {
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