using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Helpa
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class HelperRegister2 : ContentPage
	{
		public HelperRegister2()
		{
			InitializeComponent ();

            NavigationPage.SetHasNavigationBar(this, false);
        }

        void OnSetLocationOnMap(object sender, EventArgs eventArgs)
        {
            Navigation.PushAsync(new HelperRegister3());
        }

        void OnClickYourLocation(object sender, EventArgs eventArgs)
        {
            MessagingCenter.Send<HelperRegister2>(this, "Current Address");
            Navigation.PopAsync();
        }
    }
}