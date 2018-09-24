using Helpa.Views.Profile;
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
	public partial class ProfilePage : ContentPage
	{
		public ProfilePage()
		{
			InitializeComponent ();
            NavigationPage.SetHasNavigationBar(this, false);
        }
        public void ClickTap1(object sender, EventArgs e)
        {
            //var page = new EditBasicInfo();
            //MainView.Content = page.Content;
            App.NavigationPage.Navigation.PushAsync(new EditBasicInfo());
        }
        public void ClickTap2(object sender, EventArgs e)
        {
            var page = new Service();

            //MainView.Content = page.Content;
            App.NavigationPage.Navigation.PushAsync(page);
        }
        public void ClickTap3(object sender, EventArgs e)
        {
            var page = new EditVerificationPage();
           // Navigation.PushAsync(page);
            // MainView.Content = page.Content;
            App.NavigationPage.Navigation.PushAsync(page);
        }

        private void XFBackButton_Tapped(object sender, EventArgs e)
        {
            App.NavigationPage.Navigation.PopAsync();
        }
    }
}