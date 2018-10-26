using Helpa.Models;
using Helpa.Views.OtherPages;
using Helpa.Views.Profile.ProfileSettings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Helpa.Views.Profile.UserProfile
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ProfileAfterLoginPage : ContentPage
	{
        public RegisterUserModel currentUser { get; set; }

		public ProfileAfterLoginPage ()
		{
			InitializeComponent ();
            try
            {
                NavigationPage.SetHasNavigationBar(this, false);
            }
            catch(Exception e)
            {
                Console.Write(e.StackTrace);
            }
		}

        private void XFLBLViewProfile_Tapped(object sender, EventArgs e)
        {
            App.NavigationPage.Navigation.PushAsync(new ProfilePage());
        }

        private void OnClickSetting(object sender, EventArgs e)
        {
            App.NavigationPage.Navigation.PushAsync(new UserSettingsPage());
        }

        private void XFBackButton_Tapped(object sender, EventArgs e)
        {
            App.NavigationPage.Navigation.PopAsync();
        }

        private void OnClickMyJobPosts(object sender, EventArgs e)
        {
            App.NavigationPage.Navigation.PushAsync(new MyJobPostPage());
        }

        private void OnClickMyNetwork(object sender, EventArgs e)
        {
            App.NavigationPage.Navigation.PushAsync(new MyNetworkPage());
        }

        private void OnClickSavedItem(object sender, EventArgs e)
        {
            App.NavigationPage.Navigation.PushAsync(new SavedItemsUsersPage());
        }

        private void OnClickRateUs(object sender, EventArgs e)
        {
            Device.OpenUri(new Uri("market://details?id=com.vinove.Helpa"));
        }

        private void OnClickLikeOnFacebook(object sender, EventArgs e)
        {
            Device.OpenUri(new Uri("fb://page/108972495813848"));
        }
    }
}