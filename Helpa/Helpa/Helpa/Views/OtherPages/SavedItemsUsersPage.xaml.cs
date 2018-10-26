using Helpa.ViewModels;
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

            //List<string> obj = new List<string>()
            //    {
            //        "find_helpers" , "job_posts", "messages","notifications", "profile"
            //    };

            //lvFullUsers.ItemsSource = obj;

            BindingContext = new SavedItemsUsersViewModel();

            lblUsers.TextColor = Color.FromHex("#FF748E");
            bvUsers.BackgroundColor = Color.FromHex("#FF748E");
        }
        
        private void OnClickSavedUser(object sender, EventArgs e)
        {
            //BackgroundColor = "#FF748E"
            lvFullUsers.IsVisible = true;

            lvFullJobPost.IsVisible = false;

            //lvFullJobPost.ItemsSource = null;

            lblUsers.TextColor = Color.FromHex("#FF748E");
            bvUsers.BackgroundColor= Color.FromHex("#FF748E");

            lblJobPosts.TextColor= Color.LightGray;
            bvJobPost.BackgroundColor = Color.Transparent;

            //List<string> obj = new List<string>()
            //    {
            //        "find_helpers" , "job_posts", "messages","notifications", "profile"
            //    };

            //lvFullUsers.ItemsSource = obj;
        }

        private void OnClickSavedJobPosts(object sender, EventArgs e)
        {
            lvFullUsers.IsVisible = false;
            //lvFullUsers.ItemsSource = null;
            lvFullJobPost.IsVisible = true;

            lblUsers.TextColor = Color.LightGray;
            bvUsers.BackgroundColor = Color.Transparent;

            lblJobPosts.TextColor = Color.FromHex("#FF748E");
            bvJobPost.BackgroundColor = Color.FromHex("#FF748E");

            //List<string> obj = new List<string>()
            //    {
            //        "find_helpers" , "job_posts", "messages","notifications", "profile"
            //    };

            //lvFullJobPost.ItemsSource = obj;
        }

        private void OnBackPress(object sender, EventArgs e)
        {
            App.NavigationPage.Navigation.PopAsync();
        }
    }
}