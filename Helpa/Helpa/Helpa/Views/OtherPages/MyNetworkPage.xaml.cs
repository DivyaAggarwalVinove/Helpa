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
	public partial class MyNetworkPage : ContentPage
	{
		public MyNetworkPage ()
		{
			InitializeComponent ();
		}

        private void XFBackButton_Tapped(object sender, EventArgs e)
        {
            App.NavigationPage.Navigation.PopAsync();
        }
    }
}