using Helpa.Models;
using Helpa.Services;
using Helpa.Utility;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
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
                //userModel.Gender = detailsInJson.GetValue("gender").ToString();
                userModel.Token = token;
                userModel.UserName = detailsInJson.GetValue("id").ToString();
                userModel.LoginProvider = "Facebook";
                userModel.Role = "Helper".ToUpper();

                await (new RegisterServices()).RegisterExternal(userModel);
                //await GetFacebookProfileAsync(token);
            };

            entryHelperRegEmail.Completed += (s, e) => entryHelperRegUsername.Focus();
            entryHelperRegUsername.Completed += (s, e) => entryHelperRegPwd.Focus();
        }

        public void GotoNext(RegisterUserModel userModel)
        {
            App.Database.SaveUserAsync(userModel);
            Navigation.PushAsync(new HelperCompleteRegister(userModel));
        }

        public void ShowError(string error)
        {
            DisplayAlert("Error", error, "Ok");
        }
        
        void OnSignUpEmailPhnClicked(object sender, EventArgs args)
        {
            btnHelperSignUpEmailPhn.IsVisible = false;
            slHelperSignUpEmailPhn.IsVisible = true;
            btnHelperSignUp.IsVisible = true;
        }

        async void OnFaceBookSignUp(object sender, EventArgs args)
        {
            var externalDetails = await (new RegisterServices()).GetExternalDetails();
            await (new RegisterServices()).FacebookSignUp(externalDetails.Where(x=>x.Name.ToUpper() == "FACEBOOK").FirstOrDefault());
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

        public void OnBackPress(object sender, TappedEventArgs eventArgs)
        {
            if (slHelperSignUpEmailPhn.IsVisible)
            {
                btnHelperSignUpEmailPhn.IsVisible = true;
                slHelperSignUpEmailPhn.IsVisible = false;
                btnHelperSignUp.IsVisible = false;
            }
            else
            {
                Navigation.PopAsync();
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
                    userModel.MobileNumber = entryHelperRegEmail.Text;
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

        void OnClickLogin(object sender, EventArgs args)
        {
            Navigation.PopAsync();
            //Navigation.PushAsync(new LoginPage());
        }

        void ShowOrHidePassword(object sender, EventArgs args)
        {
            entryHelperRegPwd.IsPassword = !entryHelperRegPwd.IsPassword;
        }
    }
}