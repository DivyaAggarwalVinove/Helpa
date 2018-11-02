using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Helpa.Views.Profile.UserProfile
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ProfileBeforeLoginPage : ContentView
	{
        public static bool isLogin;

		public ProfileBeforeLoginPage ()
		{
			InitializeComponent ();
            NavigationPage.SetHasNavigationBar(this, false);
		}

        private void OnClickProfileLogin(object sender, EventArgs e)
        {
            App.NavigationPage.Navigation.PushAsync(new LoginPage());
        }

        private void OnClickProfileParentSignup(object sender, EventArgs e)
        {
            App.NavigationPage.Navigation.PushAsync(new Register());
        }

        private void OnClickProfileHelperSignup(object sender, EventArgs e)
        {
            App.NavigationPage.Navigation.PushAsync(new HelperRegister());
        }
    }
}