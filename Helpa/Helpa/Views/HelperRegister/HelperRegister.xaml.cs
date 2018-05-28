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

                HelpersModel helperModel = new HelpersModel();
                helperModel.email = detailsInJson.GetValue("email").ToString();
                //helperModel.profileImage
                helperModel.gender = detailsInJson.GetValue("gender").ToString();
                helperModel.token = token;
                helperModel.userName = detailsInJson.GetValue("id").ToString();
                helperModel.loginProvider = "Facebook";
                helperModel.role = "Helper".ToUpper();

                await (new RegisterServices()).RegisterExternal(helperModel);
                //await GetFacebookProfileAsync(token);
            };
        }

        public void GotoNext(string userName, string gender, string phoneNumber, string email)
        {
            Navigation.PushAsync(new HelperRegister1(userName, gender, phoneNumber, email));
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
            else if (!Utils.isValidEmail(entryHelperRegEmail.Text))
            {
                if (!Utils.isValidMobileNo(entryHelperRegEmail.Text))
                    await DisplayAlert("Warning", "Please enter valid email or phone number.", "Ok");
                else
                {
                    HelpersModel helperModel = new HelpersModel();
                    helperModel.phoneNumber = entryHelperRegEmail.Text;
                    helperModel.userName = entryHelperRegUsername.Text;
                    helperModel.password = entryHelperRegPwd.Text;
                    helperModel.role = "Helper".ToUpper();

                    await (new RegisterServices()).RegisterService(helperModel);                    
                }
            }
            else
            {
                HelpersModel helperModel = new HelpersModel();
                helperModel.email = entryHelperRegEmail.Text;
                helperModel.userName = entryHelperRegUsername.Text;
                helperModel.password = entryHelperRegPwd.Text;
                helperModel.role = "Helper".ToUpper();

                await (new RegisterServices()).RegisterService(helperModel);
            }
        }

        void ShowOrHidePassword(object sender, EventArgs args)
        {
            entryHelperRegPwd.IsPassword = !entryHelperRegPwd.IsPassword;
        }
    }
}