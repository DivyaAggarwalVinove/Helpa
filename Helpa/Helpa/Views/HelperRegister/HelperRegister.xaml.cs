using Helpa.Models;
using Helpa.Services;
using Helpa.Utility;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Helpa
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HelperRegister : ContentPage
    {
        private static HelperRegister instance;
        public static HelperRegister Instance
        {
            get { return instance; }
            set { instance = value; }
        }

        public HelperRegister()
        {
            InitializeComponent();

            NavigationPage.SetHasNavigationBar(this, false);

            Instance = this;
            App.PostSuccessFacebookAction = async token =>
            {
                JObject detailsInJson = await (new FacebookServices().GetFacebookProfileAsync(token));

                RegisterUserModel userModel = new RegisterUserModel();
                userModel.Email = detailsInJson.GetValue("email").ToString();
                //userModel.profileImage
                userModel.Gender = detailsInJson.GetValue("gender").ToString();
                userModel.Token = token;
                userModel.UserName = detailsInJson.GetValue("id").ToString();
                userModel.LoginProvider = "Facebook";
                userModel.Role = "Helper".ToUpper();

                await (new RegisterServices()).RegisterExternal(userModel);
                //await GetFacebookProfileAsync(token);
            };
        }

        public void GotoNext(RegisterUserModel userModel)
        {
            App.Database.SaveUserAsync(userModel);
            Navigation.PushAsync(new HelperRegister1(userModel));
        }

        void OnSignUpEmailPhnClicked(object sender, EventArgs args)
        {
            btnHelperSignUpEmailPhn.IsVisible = false;
            slHelperSignUpEmailPhn.IsVisible = true;
            btnHelperSignUp.IsVisible = true;
        }

        protected override bool OnBackButtonPressed()
        {
            if (slHelperSignUpEmailPhn.IsVisible)
            {
                btnHelperSignUpEmailPhn.IsVisible = true;
                slHelperSignUpEmailPhn.IsVisible = false;
                btnHelperSignUp.IsVisible = false;

                return true;
            }
            else
            {
                return base.OnBackButtonPressed();
            }
        }
        
        async void OnSignUp(object sender, EventArgs args)
        {
            if (string.IsNullOrEmpty(entryHelperRegEmail.Text))
            {
                await DisplayAlert("Warning", "Please enter email or phone number.", "Ok");
            }
            else if (string.IsNullOrEmpty(entryHelperRegUsername.Text))
            {
                await DisplayAlert("Warning", "Please enter username.", "Ok");
            }
            else if (string.IsNullOrEmpty(entryHelperRegPwd.Text))
            {
                await DisplayAlert("Warning", "Please enter password.", "Ok");
            }
            else if (!Utils.IsValidEmail(entryHelperRegEmail.Text))
            {
                if (!Utils.IsValidMobileNo(entryHelperRegEmail.Text))
                    await DisplayAlert("Warning", "Please enter valid email or phone number.", "Ok");
                else
                {
                    RegisterUserModel userModel = new RegisterUserModel();
                    userModel.PhoneNumber = entryHelperRegEmail.Text;
                    //helperModel.profileImage                    
                    userModel.UserName = entryHelperRegUsername.Text;
                    userModel.Password = entryHelperRegPwd.Text;
                    userModel.Role = "Helper".ToUpper();

                    await (new RegisterServices()).RegisterService(userModel);                    
                }
            }
            else
            {
                RegisterUserModel userModel = new RegisterUserModel();
                userModel.Email = entryHelperRegEmail.Text;
                //helperModel.profileImage                    
                userModel.UserName = entryHelperRegUsername.Text;
                userModel.Password = entryHelperRegPwd.Text;
                userModel.Role = "Helper".ToUpper();

                await (new RegisterServices()).RegisterService(userModel);
            }
        }

        void ShowOrHidePassword(object sender, EventArgs args)
        {
            entryHelperRegPwd.IsPassword = !entryHelperRegPwd.IsPassword;
        }
    }
}