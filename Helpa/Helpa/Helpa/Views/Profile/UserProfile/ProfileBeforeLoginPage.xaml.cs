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
	public partial class ProfileBeforeLoginPage : ContentPage
	{
		public ProfileBeforeLoginPage ()
		{
			InitializeComponent ();
            NavigationPage.SetHasNavigationBar(this, false);
		}

        private void XFBtnLogin_Clicked(object sender, EventArgs e)
        {
            App.NavigationPage.Navigation.PushAsync(new ProfileAfterLoginPage());
          //  App.NavigationPage.Navigation.PushAsync(new LoginPage());
        }
    }
}