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
	public partial class Register2 : ContentPage
	{
		public Register2 ()
		{
			InitializeComponent ();

            NavigationPage.SetHasNavigationBar(this, false);
        }

        void OnSetLocationOnMap(Object sender, EventArgs eventArgs)
        {
            Navigation.PushAsync(new Register3());
        }
    }
}