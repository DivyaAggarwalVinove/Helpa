using Helpa.Models;
using Helpa.Services;
using Newtonsoft.Json;
using Plugin.FacebookClient;
using Plugin.GoogleClient;
using Plugin.GoogleClient.Shared;
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
            ExternalUserModel um = new ExternalUserModel();
            await (new RegisterServices()).RegisterExternal(um);
            //var externalDetails = await (new RegisterServices()).GetExternalDetails();
            //await (new RegisterServices()).FacebookSignUp(externalDetails.Where(x => x.Name.ToUpper() == "FACEBOOK").FirstOrDefault());
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
                LoginErrorResponseModel response = await (new LoginServices()).Login(entryLoginEmail.Text, entryLoginPwd.Text);
                if (response == null)
                {
                    await Navigation.PopAsync();
                }
                else
                {
                    await DisplayAlert("Error!", response.error_description, "Ok");
                }
            }
        }

        async void OnFacebookLogin(object sender, EventArgs args)
        {
            var userdata = await CrossFacebookClient.Current.RequestUserDataAsync(new string[] { "email", "first_name", "gender", "last_name", "birthday" }, new string[] { "email", "user_birthday" });
            var data = userdata.Data;

            //var json = JsonConvert.SerializeObject(data);
            var d = JsonConvert.DeserializeObject<FacebookModel>(data);

            ExternalUserModel userinfo = new ExternalUserModel();
            //userinfo.id = data.Id;
            //userinfo.idToken = data.Id;
            //userinfo.name = data.Name;
            //userinfo.email = data.Email;
            userinfo.LoginProvider = "FACEBOOK".ToUpper();
            userinfo.provider = "FACEBOOK".ToUpper();

            bool result = await (new LoginServices()).ExternalLogin(userinfo);

            if (result)
            {
                await Navigation.PopAsync();
            }
            else
            {
                await DisplayAlert("", "Not able to Login!", "Ok");
            }
        }
        async void OnGoogleLogin(object sender, EventArgs args)
        {
            var userdata = await CrossGoogleClient.Current.LoginAsync();
            GoogleUser data = userdata.Data;

            ExternalUserModel userinfo = new ExternalUserModel();
            userinfo.id = data.Id;
            userinfo.idToken = data.Id;
            userinfo.name = data.Name;
            userinfo.email = data.Email;
            userinfo.profileImage = data.Picture.AbsoluteUri;

            userinfo.LoginProvider = "GOOGLE".ToUpper();
            userinfo.provider = "GOOGLE".ToUpper();
            bool result = await (new LoginServices()).ExternalLogin(userinfo);

            if(result)
            {
                await Navigation.PopAsync();
            }
            else
            {
                await DisplayAlert("", "Not able to Login!", "Ok");
            }
        }

        void ShowOrHidePassword(object sender, EventArgs args)
        {
            entryLoginPwd.IsPassword = !entryLoginPwd.IsPassword;
        }

        async void OnParentSignup(object sender, EventArgs args)
        {
            var user = App.Database.GetUsers();
            if (user == null || user.Count == 0)
                await Navigation.PushAsync(new Register());
            else
            {
                await Navigation.PushAsync(new Register1(user[0]));
            }
        }

        async void OnHelperSignup(object sender, EventArgs args)
        {
            var user = App.Database.GetUsers();
            if (user == null || user.Count == 0)
                await Navigation.PushAsync(new HelperRegister());
            else
            {
                await Navigation.PushAsync(new HelperCompleteRegister(user[0]));
            }
        }
    }
}