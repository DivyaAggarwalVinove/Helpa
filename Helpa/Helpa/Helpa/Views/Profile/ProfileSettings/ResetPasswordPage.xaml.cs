using Helpa.Models;
using Helpa.Services;
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
	public partial class ResetPasswordPage : ContentPage
	{
		public ResetPasswordPage ()
		{
			InitializeComponent ();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        private void OnClickForgotPassword(object sender, EventArgs e)
        {
            App.NavigationPage.Navigation.PushAsync(new ForgotPasswordPage());
        }

        private void OnBackPress(object sender, EventArgs e)
        {
            App.NavigationPage.PopAsync();
        }

        private async void OnUpdatePassword(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(entryCurrentpassword.Text))
            {
                await DisplayAlert("Warning", "Please enter current password.", "Ok");
            }
            else if (string.IsNullOrEmpty(entryNewpassword.Text))
            {
                await DisplayAlert("Warning", "Please enter new password.", "Ok");
            }
            else if(string.IsNullOrEmpty(entryConfirmpassword.Text))
            {
                await DisplayAlert("Warning", "Please enter confirm password.", "Ok");
            }
            else if(!entryNewpassword.Text.Equals(entryConfirmpassword.Text))
            {
                await DisplayAlert("Warning", "New password and Confirm password should be same.", "Ok");
            }
            else if (entryCurrentpassword.Text.Equals(entryNewpassword.Text))
            {
                await DisplayAlert("Warning", "Current password and New password should not be same.", "Ok");
            }
            else
            {
                var user = App.Database.GetLoggedUser();
                ResponseModel response = await(new RegisterServices()).ResetPassword(user.Id, entryCurrentpassword.Text, entryNewpassword.Text);
                await DisplayAlert("", response.Message, "OK");
            }
        }
    }
}