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
	public partial class MyJobPostPage : ContentPage
	{
      //  List<string> obj = new List<string>();
		public MyJobPostPage ()
		{
			InitializeComponent ();
            NavigationPage.SetHasNavigationBar(this, false);
            List<string> obj = new List<string>()
                {
                    "find_helpers" , "job_posts", "messages","notifications", "profile"
                };

            lvFullJobPost.ItemsSource = obj;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

        }
        private void XFBackButton_Tapped(object sender, EventArgs e)
        {
            App.NavigationPage.Navigation.PopAsync();
        }
    }
}