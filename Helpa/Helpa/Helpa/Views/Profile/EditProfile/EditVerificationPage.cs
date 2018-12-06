using Helpa.Models;
using Helpa.Services;
using Helpa.Utility;
using Plugin.FilePicker;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
namespace Helpa.Views.Profile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditVerificationPage : ContentPage
    {
        public RegisterUserModel LoggedinUser { get; set; }
        public static EditVerificationPage Instance { get; set; }

        public EditVerificationPage()
        {
            InitializeComponent();

            Instance = this;
            NavigationPage.SetHasNavigationBar(this, false);   
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            VerificationInfoModel verificationInfo = await (new LoginServices()).GetVerificationInfo(LoggedinUser.Id);
            if (verificationInfo.certificate == null)
            {
                gridVerifiedCertificate.IsVisible = false;
                gridNotVerifiedIdCertificate.IsVisible = true;
            }
            else
            {
                gridVerifiedCertificate.IsVisible = true;
                gridNotVerifiedIdCertificate.IsVisible = false;
            }

            if (verificationInfo.idcard == null)
            {
                gridVerifiedIdCard.IsVisible = false;
                gridNotVerifiedIdIdCard.IsVisible = true;
            }
            else
            {
                gridVerifiedIdCard.IsVisible = true;
                gridNotVerifiedIdIdCard.IsVisible = false;
            }

            if (!verificationInfo.Google)
            {
                gridVerifiedGoogle.IsVisible = false;
                gridNotVerifiedIdGoogle.IsVisible = true;
            }
            else
            {
                gridVerifiedGoogle.IsVisible = true;
                gridNotVerifiedIdGoogle.IsVisible = false;
            }

            if (!verificationInfo.Facebook)
            {
                gridVerifiedFacebook.IsVisible = false;
                gridNotVerifiedIdFacebook.IsVisible = true;
            }
            else
            {
                gridVerifiedFacebook.IsVisible = true;
                gridNotVerifiedIdFacebook.IsVisible = false;
            }

            if (!verificationInfo.emailconfirmed)
            {
                gridNotVerifiedIdEmail.IsVisible = true;
                entryEditVerifyEmail.Text = verificationInfo.email;
            }
            else
            {
                gridNotVerifiedIdEmail.IsVisible = false;
                //entryEditVerifyPhone.Text = verificationInfo.;
            }

            if (!verificationInfo.phonenumberconfirmed)
            {
                gridNotVerifiedIdMobile.IsVisible = true;
                entryEditVerifyPhone.Text = verificationInfo.PhoneNumber;
            }
            else
            {
                gridNotVerifiedIdMobile.IsVisible = false;
            }
        }
        private void OnBackPress(object sender, EventArgs e)
        {
            App.NavigationPage.Navigation.PopAsync();
        }
        async void OnClickSendSms(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(entryEditVerifyPhone.Text))
            {
                await DisplayAlert("Warning", "Please enter phone number", "Ok");
            }
            else if (!Utils.IsValidMobileNo(entryEditVerifyPhone.Text))
            {
                await DisplayAlert("Warning", "Please enter valid mobile number", "Ok");
            }
            else
            {
                if (LoggedinUser != null)
                    await (new RegisterServices()).SendSmsCode(LoggedinUser.Id, entryEditVerifyPhone.Text, "VERIFICATION");
            }
        }

        async void OnVerifyCode(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(entryEditVerifySmsCode.Text))
            {
                await DisplayAlert("Warning", "Please enter verification code", "Ok");
            }
            else
            {
                if (LoggedinUser != null)
                    await (new RegisterServices()).VerifyOtp(LoggedinUser.Id, entryEditVerifySmsCode.Text, "VERIFICATION");
            }
        }

        async void OnVerifyEmail(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(entryEditVerifyEmail.Text))
            {
                await DisplayAlert("", "Required email id to verify", "Ok");
            }
            else
            {
                if (LoggedinUser != null)
                {
                    await (new RegisterServices()).EmailVerify(entryEditVerifyEmail.Text);
                    //Call api to verify email id.
                    //await (new RegisterServices()).VerifyOtp(LoggedinUser.Id, entryEditVerifySmsCode.Text, "VERIFICATION");
                }
            }
        }

        async void OnCertificateUpload(object sender, EventArgs e)
        {
            if (LoggedinUser != null)
            {
                var file = await CrossFilePicker.Current.PickFile();

                if (file != null)
                {
                    var stream = Utils.ConvertByteToBase64(file.DataArray);

                    //await (new LoginServices()).UploadCertificate(LoggedinUser.HelperId, stream);
                    await (new LoginServices()).UploadCertificate(133, stream);
                }
            }
        }

        public void ShowSmsMessage(ResponseModel message, bool isSuccess)
        {
            if (isSuccess)
            {
                entryEditVerifyPhone.IsEnabled = false;
            }

            DisplayAlert("", message.Message, "Ok");
        }

        public void ShowVerificationMessage(ResponseModel message, bool isSuccess)
        {
            if (isSuccess)
            {
                entryEditVerifySmsCode.IsEnabled = false;
                gridNotVerifiedIdMobile.IsVisible = true;
            }

            DisplayAlert("", message.Message, "Ok");
        }

        public void ShowEmailVerifyMessage(ResponseModel message, bool isSuccess)
        {
            if (isSuccess)
            {
                entryEditVerifyEmail.IsEnabled = false;
                gridNotVerifiedIdEmail.IsVisible = false;
            }
            DisplayAlert("", message.Message, "Ok");
        }
    }
}