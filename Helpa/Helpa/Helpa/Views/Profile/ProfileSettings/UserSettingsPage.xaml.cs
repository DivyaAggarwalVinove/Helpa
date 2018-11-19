using Helpa.Views.JobPosts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Helpa.Views.Profile.ProfileSettings
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class UserSettingsPage : ContentPage
	{
		public UserSettingsPage ()
		{
			InitializeComponent ();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        private void XFLBLNotification_Tapped(object sender, EventArgs e)
        {           
            App.NavigationPage.Navigation.PushAsync(new NotificationSettingsPage());
        }

        private void XFLBLMessages_Tapped(object sender, EventArgs e)
        {
            App.NavigationPage.Navigation.PushAsync(new MessagesSettingsPage());
        }

        private void XFLBLBlockUser_Tapped(object sender, EventArgs e)
        {
            App.NavigationPage.Navigation.PushAsync(new BlockUserPage());
        }

        private void OnClickResetPassword(object sender, EventArgs e)
        {
            App.NavigationPage.Navigation.PushAsync(new ResetPasswordPage());
        }

        private void OnBackPress(object sender, EventArgs e)
        {
            App.NavigationPage.Navigation.PopAsync();
        }
    }
}