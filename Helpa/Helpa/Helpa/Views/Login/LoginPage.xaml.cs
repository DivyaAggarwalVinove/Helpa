using Helpa.Models;
using Helpa.Services;
using Helpa.Views.Profile.UserProfile;
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
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);

            entryLoginEmail.Completed += (s, e) => entryLoginPwd.Focus();
        }

        async void OnExternalLogin(object sender, EventArgs args)
        {
            var externalDetails = await (new RegisterServices()).GetExternalDetails();
            await (new RegisterServices()).FacebookSignUp(externalDetails.Where(x => x.Name.ToUpper() == "FACEBOOK").FirstOrDefault());
        }

        void OnLoginEmailPhnClicked(object sender, EventArgs args)
        {
            btnLoginEmailPhn.IsVisible = false;
            slLoginEmailPhn.IsVisible = true;
            btnLoginSignUp.IsVisible = true;
        }

        void OnSignUpEmailPhnClicked(object sender, EventArgs args)
        {
            btnLoginEmailPhn.IsVisible = false;
            slLoginEmailPhn.IsVisible = true;
            btnLoginSignUp.IsVisible = true;
        }

        protected override bool OnBackButtonPressed()
        {
            if (slLoginEmailPhn.IsVisible)
            {
                btnLoginEmailPhn.IsVisible = true;
                slLoginEmailPhn.IsVisible = false;
                btnLoginSignUp.IsVisible = false;

                return true;
            }
            else
            {
                return base.OnBackButtonPressed();
            }
        }

        public void OnBackPress(object sender, TappedEventArgs eventArgs)
        {
            if (slLoginEmailPhn.IsVisible)
            {
                btnLoginEmailPhn.IsVisible = true;
                slLoginEmailPhn.IsVisible = false;
                btnLoginSignUp.IsVisible = false;
            }
            else
            {
                Navigation.PopAsync();
            }
        }

        async void OnLogin(object sender, EventArgs args)
        {
            if (string.IsNullOrEmpty(entryLoginEmail.Text))
            {
                await DisplayAlert("Warning", "Please enter email or phone number.", "Ok");
            }
            else if (string.IsNullOrEmpty(entryLoginPwd.Text))
            {
                await DisplayAlert("Warning", "Please enter password.", "Ok");
            }
            else
            {
                LoginErrorResponseModel response = await(new LoginServices()).Login(entryLoginEmail.Text, entryLoginPwd.Text);
                if (response == null)
                {
                    //if (App.selectedPage == 4)
                    //{
                    //    try
                    //    {
                    //        await Navigation.PopAsync();

                    //        var loggedUser = App.Database.GetLoggedUser();

                    //        ProfileAfterLoginPage profileAfterLoginPage = new ProfileAfterLoginPage();
                    //        profileAfterLoginPage.currentUser = loggedUser;

                    //        var cp = App.app.contentPresenter;
                    //        cp.Content = profileAfterLoginPage.Content;
                    //    }
                    //    catch (Exception e)
                    //    {
                    //        Console.Write(e.StackTrace);
                    //    }
                    //}
                    //else
                    //{
                        await Navigation.PopAsync();
                    //}
                }
                else
                {
                    await DisplayAlert("Error!", response.error_description, "Ok");
                }
            }
        }

        void ShowOrHidePassword(object sender, EventArgs args)
        {
            entryLoginPwd.IsPassword = !entryLoginPwd.IsPassword;
        }

        async void OnParentSignup(object sender, EventArgs args)
        {
            var user = App.Database.GetUsersAsync();
            if (user == null || user.Count == 0)
                await Navigation.PushAsync(new Register());
            else
            {
                await Navigation.PushAsync(new Register1(user[0]));
            }
        }

        async void OnHelperSignup(object sender, EventArgs args)
        {
            var user = App.Database.GetUsersAsync();
            if (user == null || user.Count == 0)
                await Navigation.PushAsync(new HelperRegister());
            else
            {
                await Navigation.PushAsync(new HelperCompleteRegister(user[0]));
            }
        }
    }
}