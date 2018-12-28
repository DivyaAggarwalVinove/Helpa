using Helpa.Models;
using Helpa.Services;
using Helpa.Utility;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Plugin.FacebookClient;
using Plugin.GoogleClient;
using Plugin.GoogleClient.Shared;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Helpa
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Register : ContentPage
    {
        private static Register instance;
        public static Register Instance
        {
            get { return instance; }
            set { instance = value; }
        }

        public Register()
        {
            InitializeComponent();

            NavigationPage.SetHasNavigationBar(this, false);

            Instance = this;
            
            entryRegEmail.Completed += (s, e) => entryRegUsername.Focus();
            entryRegUsername.Completed += (s, e) => entryRegPwd.Focus();
        }

        public void GotoNext(RegisterUserModel userModel)
        {
            userModel.IsRegistered = true;
            App.Database.SaveUserAsync(userModel);
            App.NavigationPage.Navigation.PushAsync(new Register1(userModel));
        }
        public void ShowError(string error)
        {
            DisplayAlert("Error", error, "Ok");
        }

        void OnSignUpEmailPhnClicked(object sender, EventArgs args)
        {
            btnSignUpEmailPhn.IsVisible = false;
            slSignUpEmailPhn.IsVisible = true;
            btnSignUp.IsVisible = true;
        }

        protected override bool OnBackButtonPressed()
        {
            if (slSignUpEmailPhn.IsVisible)
            {
                btnSignUpEmailPhn.IsVisible = true;
                slSignUpEmailPhn.IsVisible = false;
                btnSignUp.IsVisible = false;

                return true;
            }
            else
            {
                return base.OnBackButtonPressed();
            }
        }
        public void OnBackPress(object sender, TappedEventArgs eventArgs)
        {
            if (slSignUpEmailPhn.IsVisible)
            {
                btnSignUpEmailPhn.IsVisible = true;
                slSignUpEmailPhn.IsVisible = false;
                btnSignUp.IsVisible = false;
            }
            else
            {
                Navigation.PopAsync();
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
            userinfo.Role = "Parent".ToUpper();

            await (new RegisterServices()).RegisterExternal(userinfo);
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
            userinfo.Role = "Parent".ToUpper();

            await (new RegisterServices()).RegisterExternal(userinfo);   
        }

        //private async Task GetFacebookProfileAsync(string accessToken)
        //{
        //    var requestUrl = "https://graph.facebook.com/v2.7/me/?fields=name,id,email,gender,picture&access_token=" + accessToken;
        //    var httpClient = new HttpClient();
        //    var userDetails = await httpClient.GetStringAsync(requestUrl);

        //    var detailsInJson = JObject.Parse(userDetails);

        //    ExternalModel userModel = new ExternalModel();
        //    userModel.email = detailsInJson.GetValue("email").ToString();
        //    //helperModel.profileImage
        //    //userModel.Gender = detailsInJson.GetValue("gender").ToString();
        //    userModel.idToken = accessToken;
        //    userModel.name = detailsInJson.GetValue("id").ToString();
        //    userModel.LoginProvider = "Facebook";
        //    userModel.Role = "Parent".ToUpper();

        //    await (new RegisterServices()).RegisterExternal(userModel);
        //}

        async void OnSignUp(object sender, EventArgs args)
        {
            if (string.IsNullOrEmpty(entryRegEmail.Text))
            {
                await DisplayAlert("Warning", "Please enter email or phone number.", "Ok");
            }
            else if (string.IsNullOrEmpty(entryRegUsername.Text))
            {
                await DisplayAlert("Warning", "Please enter username.", "Ok");
            }
            else if (string.IsNullOrEmpty(entryRegPwd.Text))
            {
                await DisplayAlert("Warning", "Please enter password.", "Ok");
            }
            else if (!Utils.IsValidEmail(entryRegEmail.Text))
            {
                if (!Utils.IsValidMobileNo(entryRegEmail.Text))
                    await DisplayAlert("Warning", "Please enter valid email or phone number.", "Ok");
                else
                {
                    RegisterUserModel userModel = new RegisterUserModel();
                    userModel.MobileNumber = entryRegEmail.Text;
                    userModel.UserName = entryRegUsername.Text;
                    userModel.Password = entryRegPwd.Text;
                    userModel.Role = "Parent".ToUpper();

                    await (new RegisterServices()).RegisterService(userModel);
                    //Navigation.PushAsync(new Register1());
                }
            }
            else
            {
                RegisterUserModel userModel = new RegisterUserModel();
                userModel.Email = entryRegEmail.Text;
                userModel.UserName = entryRegUsername.Text;
                userModel.Password = entryRegPwd.Text;
                userModel.Role = "Parent".ToUpper();

                await (new RegisterServices()).RegisterService(userModel);
                //Navigation.PushAsync(new Register1());
            }
        }
        void OnClickLogin(object sender, EventArgs args)
        {
            Navigation.PopAsync();
            //Navigation.PushAsync(new LoginPage());
        }
        void ShowOrHidePassword(object sender, EventArgs args)
        {
            entryRegPwd.IsPassword = !entryRegPwd.IsPassword;
        }
    }
}