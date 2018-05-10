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
	public partial class Register1 : ContentPage
	{
		public Register1 ()
		{
			InitializeComponent ();

            NavigationPage.SetHasNavigationBar(this, false);
        }
	}
}