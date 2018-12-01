using Helpa.Models;
using Helpa.Services;
using Helpa.ViewModels.OtherViewModels;
using Plugin.Share;
using Plugin.Share.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Helpa.Views.Profile.OtherPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MyJobPostPage : ContentPage
    {
        public MyJobPostPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            BindingContext = new MyJobPostsViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        private void OnBackPress(object sender, EventArgs e)
        {
            App.NavigationPage.Navigation.PopAsync();
        }

        private void OnClickShare(object sender, EventArgs e)
        {
            aiMyJobPost.IsRunning = true;

            CrossShare.Current.Share(new ShareMessage { Text = "Test", Title = "Test Title", Url = "www.google.com" });

            aiMyJobPost.IsRunning = false;
        }
        private async void OnClickBookmark(object sender, TappedEventArgs e)
        {
            aiMyJobPost.IsRunning = true;

            RegisterUserModel loggedUser = App.Database.GetLoggedUser();
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

                        bool isSuccess = await(new HelpersServices()).BookMarkHelper(loggedUser.Id, helperId);
                        imgBookmark.Source = "save_filled.png";
                    }
                }
                else
                {
                    int helperId = int.Parse(e.Parameter.ToString());

                    bool isSuccess = await(new HelpersServices()).UnBookMarkHelper(loggedUser.Id, helperId);
                    imgBookmark.Source = "save.png";
                }
            }

            aiMyJobPost.IsRunning = false;
        }
    }
}