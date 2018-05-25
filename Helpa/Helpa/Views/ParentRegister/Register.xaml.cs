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
            App.PostSuccessFacebookAction = async token =>
            {
                //you can use this token to authenticate to the server here
                //call your FacebookLoginService.LoginToServer(token)
                //I'll just navigate to a screen that displays the token:
                //await Navigation.PushAsync(new Register1());

                await GetFacebookProfileAsync(token);
            };
        }

        public void GotoNext(string userName, string gender, string phoneNumber, string email)
        {
            Navigation.PushAsync(new Register1(userName, gender, phoneNumber, email));
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

        void OnFacebookLogin(object sender, EventArgs args)
        {
            /*var api_request = "
             * https://www.facebook.com/v3.0/dialog/oauth?client_id=172211350107196&display=popup&response_type=token&redirect_uri=https://www.facebook.com/connect/login_success.html
             * ";
             *
            var webview = new WebView
            {
                Source = api_request,
                HeightRequest = 1,
            };
            
            //webview.Navigated += WebViewNavigation;
            webview.Navigating += WebViewNavigating;

            Navigation.PushAsync(new ContentPage() { Content = webview });
            Content = webview;*/
        }

        //private async void WebViewNavigating(object sender, WebNavigatingEventArgs e)
        //{
        //    var url = e.Url;
        //    var cancel = e.Cancel;

        //    var accessToken = ExtractAccessTokenFromUrl(e.Url);
        //    if (accessToken != "")
        //    {
        //        await GetFacebookProfileAsync(accessToken);
        //    }
        //}

        //private void WebViewNavigation(object sender, WebNavigatedEventArgs e)
        //{
        //    var accessToken = ExtractAccessTokenFromUrl(e.Url);
        //    var result = e.Result;

        //    if (accessToken != null)
        //    {
        //        //await GetFacebookProfileAsync(accessToken);
        //    }
        //}

        //private string ExtractAccessTokenFromUrl(string url)
        //{
        //    string access_code = "";
        //    if (url.Contains("access_token") && url.Contains("&expires_in="))
        //    {
        //        access_code = url.Replace("https://www.facebook.com/connect/login_success.html#access_token=", "").ToString();

        //        if(access_code.Contains("reauthorize_required_in"))
        //        {
        //            access_code = access_code.Remove(access_code.IndexOf("&reauthorize_required_in"));
        //        }
        //        else
        //        {
        //            access_code = access_code.Remove(access_code.IndexOf("&expires_in"));
        //        }
        //    }

        //    return access_code;
        //}

        private async Task GetFacebookProfileAsync(string accessToken)
        {
            var requestUrl = "https://graph.facebook.com/v2.7/me/?fields=name,id,email,gender,picture&access_token=" + accessToken;
            var httpClient = new HttpClient();
            var userDetails = await httpClient.GetStringAsync(requestUrl);

            var detailsInJson = JObject.Parse(userDetails);

            HelpersModel helperModel = new HelpersModel();
            helperModel.email = detailsInJson.GetValue("email").ToString();
            //helperModel.profileImage
            helperModel.gender = detailsInJson.GetValue("gender").ToString();
            helperModel.token = accessToken;
            helperModel.userName = detailsInJson.GetValue("id").ToString();
            helperModel.loginProvider = "Facebook";
            helperModel.role = "Helper".ToUpper();

            await (new ParentRegisterServices()).RegisterExternal(helperModel);
        }

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
            else if (!Utils.isValidEmail(entryRegEmail.Text))
            {
                if (!Utils.isValidMobileNo(entryRegEmail.Text))
                    await DisplayAlert("Warning", "Please enter valid email or phone number.", "Ok");
                else
                {
                    HelpersModel helperModel = new HelpersModel();
                    helperModel.phoneNumber = entryRegEmail.Text;
                    helperModel.userName = entryRegUsername.Text;
                    helperModel.password = entryRegPwd.Text;
                    helperModel.role = "Helper".ToUpper();

                    await (new ParentRegisterServices()).ParentRegister(helperModel);
                    //Navigation.PushAsync(new Register1());
                }
            }
            else
            {
                HelpersModel helperModel = new HelpersModel();
                helperModel.email = entryRegEmail.Text;
                helperModel.userName = entryRegUsername.Text;
                helperModel.password = entryRegPwd.Text;
                helperModel.role = "Helper".ToUpper();

                await (new ParentRegisterServices()).ParentRegister(helperModel);
                //Navigation.PushAsync(new Register1());
            }
        }

        void ShowOrHidePassword(object sender, EventArgs args)
        {
            entryRegPwd.IsPassword = !entryRegPwd.IsPassword;
        }
    }
}