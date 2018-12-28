using Helpa.Models;
using Helpa.Services;
using Helpa.ViewModels;
using Plugin.Share;
using Plugin.Share.Abstractions;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Helpa.Views.Profile.OtherPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MyReviewsPage : ContentPage
    {
        public MyReviewsPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);

            BindingContext = new MyReviewViewModel();

            lblReviewAboutMe.TextColor = Color.FromHex("#FF748E");
            bvReviewAboutMe.BackgroundColor = Color.FromHex("#FF748E");
        }
        private void OnClickReviewToReview(object sender, EventArgs e)
        { 
            lvReviewToReview.IsVisible = true;
            lvReviewAboutMe.IsVisible = false;
            // lvReviewByMe.IsVisible = false;

            lblReviewToReview.TextColor = Color.FromHex("#FF748E");
            bvReviewToReview.BackgroundColor = Color.FromHex("#FF748E");

            lblReviewAboutMe.TextColor = Color.LightGray;
            bvReviewAboutMe.BackgroundColor = Color.Transparent;

            lblReviewByMe.TextColor = Color.LightGray;
            bvReviewByMe.BackgroundColor = Color.Transparent;
            
        }
        private void OnClickReviewAboutMe(object sender, EventArgs e)
        {
            lvReviewToReview.IsVisible = false;
            lvReviewAboutMe.IsVisible = true;
            // lvReviewByMe.IsVisible = false;

            lblReviewToReview.TextColor = Color.LightGray;
            bvReviewToReview.BackgroundColor = Color.Transparent;

            lblReviewAboutMe.TextColor = Color.FromHex("#FF748E");
            bvReviewAboutMe.BackgroundColor = Color.FromHex("#FF748E");

            lblReviewByMe.TextColor = Color.LightGray;
            bvReviewByMe.BackgroundColor = Color.Transparent;
        }
        private void OnClicReviewAboutByMe(object sender, EventArgs e)
        {
            lvReviewToReview.IsVisible = false;
            lvReviewAboutMe.IsVisible = false;
            // lvReviewByMe.IsVisible = true;

            lblReviewToReview.TextColor = Color.LightGray;
            bvReviewToReview.BackgroundColor = Color.Transparent;

            lblReviewAboutMe.TextColor = Color.LightGray;
            bvReviewAboutMe.BackgroundColor = Color.Transparent;

            lblReviewByMe.TextColor = Color.FromHex("#FF748E");
            bvReviewByMe.BackgroundColor = Color.FromHex("#FF748E");
        }
        private void OnReplyAboutMeReview(object sender, EventArgs e)
        {
            var gridReviewAboutMeReply = ((Grid)((Label)sender).Parent.Parent.Parent.Parent.Parent).Children[2];
            gridReviewAboutMeReply.IsVisible = true;
        }
        private void OnClickCancel(object sender, EventArgs e)
        {
            var gridReviewAboutMeReply = ((Grid)((Button)sender).Parent.Parent.Parent);
            gridReviewAboutMeReply.IsVisible = false;
        }
        private void OnBackPress(object sender, EventArgs e)
        {
            App.NavigationPage.Navigation.PopAsync();
        }
        private async void OnClickBookmark(object sender, TappedEventArgs e)
        {
            //aiFindHelper.IsRunning = true;

            RegisterUserModel loggedUser = await App.Database.GetLoggedUser();
            if (loggedUser == null)
            {
                await Application.Current.MainPage.Navigation.PushAsync(new LoginPage());
            }
            else
            {
                var imgBookmark = ((Image)sender);
                if (imgBookmark.Source.ToString().Contains("save.png"))
                {
                    if (e.Parameter != null)
                    {
                        int helperId = int.Parse(e.Parameter.ToString());

                        bool isSuccess = await (new HelpersServices()).BookMarkHelper(loggedUser.Id, helperId);
                        imgBookmark.Source = "save_filled.png";
                    }
                }
                else
                {
                    int helperId = int.Parse(e.Parameter.ToString());

                    bool isSuccess = await (new HelpersServices()).UnBookMarkHelper(loggedUser.Id, helperId);
                    imgBookmark.Source = "save.png";
                }
            }

            //aiFindHelper.IsRunning = false;
        }
        private void OnClickShare(object sender, EventArgs e)
        {
            //aiFindHelper.IsRunning = true;

            CrossShare.Current.Share(new ShareMessage { Text = "Test", Title = "Test Title", Url = "www.google.com" });

            //aiFindHelper.IsRunning = false;
        }
    }
}