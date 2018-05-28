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
        public HelperRegister1(string userName, string gender, string phoneNumber, string email)
        {
            InitializeComponent();

            NavigationPage.SetHasNavigationBar(this, false);
            IEnumerable<string> genderList = new List<string>() { "Male", "Female", "Rather no to say" };
            rgHelperGender.ItemsSource = genderList;
            rgHelperGender.SelectedItem = Utils.ToTitleCase( gender);

            entryHelperRegUsername1.Text = userName;
            entryHelperRegPhone1.Text = phoneNumber;
            entryHelperRegEmail1.Text = email;

            SetLocation();

            MessagingCenter.Subscribe<HelperRegister3, string>(this, "Selected Address", (sender, address) =>
            {
                entryHelperRegSearch.Text = address;
            });

            MessagingCenter.Subscribe<HelperRegister2>(this, "Current Address", (sender) =>
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
            entryHelperRegSearch.Text = a.FeatureName + "," + a.SubLocality + "," + a.Locality + "," + a.CountryName + "," + a.PostalCode;
        }

        void OnSignUpNext(object sender, EventArgs args)
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
            else if (Utils.isValidMobileNo(entryHelperRegPhone1.Text))
            {
                DisplayAlert("Warning", "Please enter valid mobile number", "Ok");
            }
            else if (!Utils.isValidEmail(entryHelperRegEmail1.Text))
            {
                DisplayAlert("Warning", "Please enter valid email", "Ok");
            }
            else
            {
                lHelperSignUp.Text = "Helper Sign Up 2/2";
                gridHelperLocation.BackgroundColor = Color.FromHex("#FF748C");
                svHelperBasicInfo.IsVisible = false;
                svHelperLocationInfo.IsVisible = true;
                //Navigation.PushAsync(new Register1());
            }
        }

        void OnSignUpDone(object sender, EventArgs args)
        {
        }

        protected override bool OnBackButtonPressed()
        {
            if (svHelperLocationInfo.IsVisible)
            {
                lHelperSignUp.Text = "Parent Sign Up 1/2";
                gridHelperLocation.BackgroundColor = Color.FromHex("#818A8F");
                svHelperBasicInfo.IsVisible = true;
                svHelperLocationInfo.IsVisible = false;

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
    }
}