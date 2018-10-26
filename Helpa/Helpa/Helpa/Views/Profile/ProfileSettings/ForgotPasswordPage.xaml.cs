using Helpa.Models;
using Helpa.Services;
using Helpa.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Helpa.Views.Profile.ProfileSettings
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ForgotPasswordPage : ContentPage
	{
		public ForgotPasswordPage ()
		{
			InitializeComponent ();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        private void OnBackPress(object sender, EventArgs e)
        {
            App.NavigationPage.PopAsync();
        }

        private async void OnSendLink(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(entryForgotEmail.Text))
            {
                await DisplayAlert("Warning", "Please enter email.", "Ok");
            }
            else if (!Utils.IsValidEmail(entryForgotEmail.Text))
            {
                await DisplayAlert("Warning", "Please enter valid email.", "Ok");
            }
            else
            {
                ResponseModel response = await (new RegisterServices()).SendLink(entryForgotEmail.Text);
                await DisplayAlert("", response.Message, "OK");
            }
        }
    }
}