using AsNum.XFControls;
using DurianCode.PlacesSearchBar;
using Helpa.Models;
using Helpa.Services;
using Helpa.Utility;
using Helpa.ViewModels.ProfileViewModel.EditProfileViewModel;
using Plugin.Connectivity;
using Plugin.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
namespace Helpa.Views.Profile.EditProfile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditBasicInfo : ContentPage
    {
        #region Variable Declaration
        public RegisterUserModel LoggedinUser { get; set; }
        public static EditBasicInfo Instance { get; set; }
        #endregion
        public EditBasicInfo()
        {
            InitializeComponent();

            Instance = this;

            IEnumerable<string> genders = new List<string>() { "Male", "Female", "Rather no to say" };
            SetRadioList(genders, rgEditBasicInfoGender);
            NavigationPage.SetHasNavigationBar(this, false);

            BindingContext = new EditBasicInfoViewModel();
            //_objProfileInfoResponse = new ProfileInfoResponse();
            //_apiService = new RestApi();
            //_objHeaderModel = new HeaderModel();
            //_objHeaderModel.TokenCode = Settings.TokenCode;
            //_baseUrl = Domain.Url + Domain.GetProfileInfoApiConstant;
        }

        void SetRadioList(IEnumerable<string> genderList, RadioGroup radioGroup)
        {
            radioGroup.ItemsSource = genderList;
            radioGroup.SelectedItem = genderList.ElementAt(0);
            StackLayout content = (StackLayout)radioGroup.Content;
            Radio rg = null;
            for (int i = 0; i < content.Children.Count; i++)
            {
                rg = (Radio)(content.Children[i]);
                rg.Margin = new Thickness(0, 0, 10, 0);
                rg.VerticalOptions = LayoutOptions.Center;
            }
        }

        private void OnBackPress(object sender, EventArgs e)
        {
            App.NavigationPage.Navigation.PopAsync();
        }

        async void OnClickSendSms(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(entryEditBasicInfoPhone.Text))
            {
                await DisplayAlert("Warning", "Please enter phone number", "Ok");
            }
            else if (!Utils.IsValidMobileNo(entryEditBasicInfoPhone.Text))
            {
                await DisplayAlert("Warning", "Please enter valid mobile number", "Ok");
            }
            else
            {
                RegisterUserModel currentUser = App.Database.GetLoggedUser();
                if (currentUser != null)
                    await (new RegisterServices()).SendSmsCode(currentUser.Id, entryEditBasicInfoPhone.Text, "EDITINFO");
            }
        }

        async void OnVerifyCode(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(entryEditBasicInfoSmsCode.Text))
            {
                await DisplayAlert("Warning", "Please enter verification code", "Ok");
            }
            else
            {
                RegisterUserModel currentUser = App.Database.GetLoggedUser();
                if (currentUser != null)
                    await (new RegisterServices()).VerifyOtp(currentUser.Id, entryEditBasicInfoSmsCode.Text, "EDITINFO");
            }
        }

        public void ShowSmsMessage(ResponseModel message, bool isSuccess)
        {
            if (isSuccess)
            {
                entryEditBasicInfoPhone.IsEnabled = false;
            }

            DisplayAlert("", message.Message, "Ok");
        }

        public void ShowVerificationMessage(ResponseModel message, bool isSuccess)
        {
            if (isSuccess)
            {
                entryEditBasicInfoSmsCode.IsEnabled = false;
            }
            DisplayAlert("", message.Message, "Ok");
        }

        public string selectedLocation;
        public AutoCompletePrediction selectedPrediction;
        private void OnNewLocationSelection(object sender, EventArgs e)
        {
            Navigation.PushAsync(new EditLocationPage(this)
            {
                SelectedLoc = entryEditBasicInfoLocation.Text
            });
        }

        private async void OnClickCamera(object sender, EventArgs e)
        {
            Image profile = (Image)sender;
            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("No Camera", ":( No camera avaialble.", "OK");
                return;
            }

            var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                PhotoSize = Plugin.Media.Abstractions.PhotoSize.Small,
                Directory = "Sample",
                MaxWidthHeight = 100, 
                CustomPhotoSize=50,
                CompressionQuality=50,
                Name = "test.jpg"
            });

            if (file == null)
                return;

            // await DisplayAlert("File Location", file.Path, "OK");

            imgEditBasicInfoProfile.Source = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                
                ((EditBasicInfoViewModel)BindingContext).UserInfo.ProfileImage = Utils.ConvertToBase64(file.GetStream());

                file.Dispose();
                return stream;
            });
        }

        private async void OnSaveBasicInfo(object sender, EventArgs e)
        {
            aiEditBasicInfo.IsRunning = true;

            if (string.IsNullOrEmpty(entryEditBasicInfoPhone.Text))
            {
                await DisplayAlert("Warning", "Please enter phone number", "Ok");
            }
            else if (string.IsNullOrEmpty(entryEditBasicInfoEmail.Text))
            {
                await DisplayAlert("Warning", "Please enter email", "Ok");
            }
            else if (!Utils.IsValidMobileNo(entryEditBasicInfoPhone.Text))
            {
                await DisplayAlert("Warning", "Please enter valid mobile number", "Ok");
            }
            else if (!Utils.IsValidEmail(entryEditBasicInfoEmail.Text))
            {
                await DisplayAlert("Warning", "Please enter valid email", "Ok");
            }
            else
            {
                var ui = ((EditBasicInfoViewModel)BindingContext).UserInfo;

                try
                {
                    Gender g = (Gender)Enum.Parse(typeof(Gender), rgEditBasicInfoGender.SelectedItem.ToString());
                    ui.Gender = (int)g;
                }
                catch
                {
                    ui.Gender = 3;
                }

                ui.MobileNumber = entryEditBasicInfoPhone.Text;
                ui.Email = entryEditBasicInfoEmail.Text;


                if (!string.IsNullOrEmpty(entryEditBasicInfoLocation.Text))
                {
                    if (ui.LocationName.Equals(entryEditBasicInfoLocation.Text))
                    {
                        ui.LocationName = entryEditBasicInfoLocation.Text;
                        try
                        {
                            Place place = await Places.GetPlace(selectedPrediction.Place_ID, Constants.googlePlaceApiKey);
                            ui.Latitude = place.Latitude.ToString();
                            ui.Longitude = place.Longitude.ToString();

                        }
                        catch (Exception ex)
                        {
                            Console.Write(ex.StackTrace);
                        }
                    }
                }


                if (!string.IsNullOrEmpty(entryEditBasicInfoIntroduction.Text))
                    ui.selfintroduction = entryEditBasicInfoIntroduction.Text;

                bool result = await (new LoginServices()).SaveUserBasicInfo(ui);

                if (result)

                    await Navigation.PopAsync();
                else
                    await DisplayAlert("", "Please try again.", "Ok");
            }

            aiEditBasicInfo.IsRunning = false;
        }

        private void OnBackCarousel(object sender, EventArgs e)
        {
            var vm = (EditBasicInfoViewModel)BindingContext;

            var img = imgCarousel.Source.GetValue(UriImageSource.UriProperty).ToString();
            var i = vm.UserInfo.Carousel.IndexOf(img);

            if (i == 0)
                return;

            EditBasicInfoViewModel editBasicInfoViewModel = ((EditBasicInfoViewModel)BindingContext);
            editBasicInfoViewModel.SelectedCarousel = i + "/" +((editBasicInfoViewModel.UserInfo.Carousel != null) ? editBasicInfoViewModel.UserInfo.Carousel.Count.ToString() : "0"); ;
            imgCarousel.Source = vm.UserInfo.Carousel[i - 1];
        }

        private void OnNextCarousel(object sender, EventArgs e)
        {
            var vm = (EditBasicInfoViewModel)BindingContext;

            var img = imgCarousel.Source.GetValue(UriImageSource.UriProperty).ToString();
            var i = vm.UserInfo.Carousel.IndexOf(img);

            if (i + 1 >= vm.UserInfo.Carousel.Count)
                return;

            EditBasicInfoViewModel editBasicInfoViewModel = ((EditBasicInfoViewModel)BindingContext);
            editBasicInfoViewModel.SelectedCarousel = (i+2) + "/" + ((editBasicInfoViewModel.UserInfo.Carousel != null) ? editBasicInfoViewModel.UserInfo.Carousel.Count.ToString() : "0"); ;
            imgCarousel.Source = vm.UserInfo.Carousel[i + 1];
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (selectedLocation == null)
                return;

            entryEditBasicInfoLocation.Text = selectedLocation;
        }
    }
}