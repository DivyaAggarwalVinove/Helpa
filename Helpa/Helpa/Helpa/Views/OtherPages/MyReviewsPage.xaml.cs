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
	public partial class MyReviewsPage : ContentPage
	{
		public MyReviewsPage ()
		{
			InitializeComponent ();
            NavigationPage.SetHasNavigationBar(this, false);
		}

        private void XFBoxviewAboutme_Tapped(object sender, EventArgs e)
        {

        }

        private void XFBoxviewByMe_Tapped(object sender, EventArgs e)
        {

        }

        private void XFBoxviewToReview_Tapped(object sender, EventArgs e)
        {

        }

        private void XFBackButton_Tapped(object sender, EventArgs e)
        {
            App.NavigationPage.Navigation.PopAsync();
        }
    }
}