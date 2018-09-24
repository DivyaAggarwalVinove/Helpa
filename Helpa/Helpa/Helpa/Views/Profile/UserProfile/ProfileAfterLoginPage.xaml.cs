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
		public ProfileAfterLoginPage ()
		{
			InitializeComponent ();
            NavigationPage.SetHasNavigationBar(this, false);
		}

        private void XFLBLViewProfile_Tapped(object sender, EventArgs e)
        {
            App.NavigationPage.Navigation.PushAsync(new ProfilePage());
        }

        private void XFLBLSettings_Tapped(object sender, EventArgs e)
        {
            App.NavigationPage.Navigation.PushAsync(new UserSettingsPage());
        }
    }
}