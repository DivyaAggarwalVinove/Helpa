using Helpa.Models;
using Helpa.Views.Profile.EditProfile;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Helpa.Views.Profile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditProfilePage : ContentPage
    {
        public RegisterUserModel LoggedinUser { get; set; }

        public EditProfilePage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        public void OnClickBasicInfo(object sender, EventArgs e)
        {
            App.NavigationPage.Navigation.PushAsync(new EditBasicInfo() { LoggedinUser = this.LoggedinUser});
        }

        public void OnClickServices(object sender, EventArgs e)
        {
            App.NavigationPage.Navigation.PushAsync(new EditService());
        }

        public void OnClickVerification(object sender, EventArgs e)
        {
            App.NavigationPage.Navigation.PushAsync(new EditVerificationPage() { LoggedinUser = this.LoggedinUser });
        }

        private void OnBackPress(object sender, EventArgs e)
        {
            App.NavigationPage.Navigation.PopAsync();
        }
    }
}