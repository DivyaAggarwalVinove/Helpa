using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Helpa.Views.OtherPages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SavedItemsUsersPage : ContentPage
	{
		public SavedItemsUsersPage ()
		{
			InitializeComponent ();

            NavigationPage.SetHasNavigationBar(this, false);

            List<string> obj = new List<string>()
                {
                    "find_helpers" , "job_posts", "messages","notifications", "profile"
                };

            lvFullUsers.ItemsSource = obj;
            XFLblUsers.TextColor = Color.FromHex("#FF748E");
            XFBoxviewUsers.BackgroundColor = Color.FromHex("#FF748E");
        }

        private void XFBackButton_Tapped(object sender, EventArgs e)
        {
            App.NavigationPage.Navigation.PopAsync();
        }

        private void XFGRIDUsers_Tapped(object sender, EventArgs e)
        {
            //BackgroundColor = "#FF748E"
            lvFullUsers.IsVisible = true;
            lvFullJobPost.IsVisible = false;
            lvFullJobPost.ItemsSource = null;
            XFLblUsers.TextColor = Color.FromHex("#FF748E");
            XFBoxviewUsers.BackgroundColor= Color.FromHex("#FF748E");
            XFLblJobPosts.TextColor= Color.LightGray;
            XFBoxviewJobPost.BackgroundColor = Color.Transparent;

            List<string> obj = new List<string>()
                {
                    "find_helpers" , "job_posts", "messages","notifications", "profile"
                };

            lvFullUsers.ItemsSource = obj;
        }

        private void XFGRIDJobPosts_Tapped(object sender, EventArgs e)
        {
            lvFullUsers.IsVisible = false;
            lvFullUsers.ItemsSource = null;
            lvFullJobPost.IsVisible = true;
            XFLblUsers.TextColor = Color.LightGray;
            XFBoxviewUsers.BackgroundColor = Color.Transparent;
            XFLblJobPosts.TextColor = Color.FromHex("#FF748E");
            XFBoxviewJobPost.BackgroundColor = Color.FromHex("#FF748E");

            List<string> obj = new List<string>()
                {
                    "find_helpers" , "job_posts", "messages","notifications", "profile"
                };

            lvFullJobPost.ItemsSource = obj;
        }
    }
}