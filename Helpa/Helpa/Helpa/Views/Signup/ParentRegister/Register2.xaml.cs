using System;
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

        void OnSetLocationOnMap(object sender, EventArgs eventArgs)
        {
            //Navigation.PushAsync(new Register3());
        }

        void OnClickYourLocation(object sender, EventArgs eventArgs)
        {
            MessagingCenter.Send<Register2>(this, "Current Address");
            Navigation.PopAsync();
        }
    }
}