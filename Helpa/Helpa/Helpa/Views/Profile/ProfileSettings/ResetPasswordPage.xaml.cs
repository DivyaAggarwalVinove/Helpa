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
	public partial class ResetPasswordPage : ContentPage
	{
		public ResetPasswordPage ()
		{
			InitializeComponent ();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        private void XFLBLForgotPassword_Tapped(object sender, EventArgs e)
        {
            App.NavigationPage.Navigation.PushAsync(new ForgotPasswordPage());
        }

        private void XFBackButton_Tapped(object sender, EventArgs e)
        {
            App.NavigationPage.PopAsync();
        }
    }
}