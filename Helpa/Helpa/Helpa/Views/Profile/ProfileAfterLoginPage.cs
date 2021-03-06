﻿using Helpa.Models;
using Helpa.ViewModels;
using Helpa.Views.Profile.DetailedProfile;
using Helpa.Views.Profile.OtherPages;
using Helpa.Views.Profile.ProfileSettings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Helpa.Views.Profile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfileAfterLoginPage : ContentView
    {
        public RegisterUserModel currentUser { get; set; }
        public ProfileAfterLoginPage(RegisterUserModel currentUser)
        {
            InitializeComponent();

            try
            {
                NavigationPage.SetHasNavigationBar(this, false);
            }
            catch (Exception e)
            {
                Console.Write(e.StackTrace);
            }

            this.currentUser = currentUser;
            lblUserProfileName.Text = currentUser.UserName;
            imgUserProfilePicture.Source = currentUser.profileImage;
            //BindingContext = new ProfileAfterLoginViewModel(currentUser);
        }

        private void OnClickViewProfile(object sender, EventArgs e)
        {
            //App.NavigationPage.Navigation.PushAsync(new EditProfilePage() { LoggedinUser = currentUser });
            App.NavigationPage.Navigation.PushAsync(new DetailedProfilePage(currentUser));
        }

        private void OnClickSetting(object sender, EventArgs e)
        {
            App.NavigationPage.Navigation.PushAsync(new UserSettingsPage());
        }

        private void XFBackButton_Tapped(object sender, EventArgs e)
        {
            App.NavigationPage.Navigation.PopAsync();
        }

        private void OnClickMyJobPosts(object sender, EventArgs e)
        {
            App.NavigationPage.Navigation.PushAsync(new MyJobPostPage());
        }

        private void OnClickMyReview(object sender, EventArgs e)
        {
            App.NavigationPage.Navigation.PushAsync(new MyReviewsPage());
        }

        private void OnClickMyNetwork(object sender, EventArgs e)
        {
            App.NavigationPage.Navigation.PushAsync(new MyNetworkPage());
        }

        private void OnClickSavedItem(object sender, EventArgs e)
        {
            App.NavigationPage.Navigation.PushAsync(new SavedItemsUsersPage());
        }

        private void OnClickRateUs(object sender, EventArgs e)
        {
            //Device.OpenUri(new Uri("market://details?id=com.vinove.Helpa"));
        }

        private void OnClickLikeOnFacebook(object sender, EventArgs e)
        {
            //Device.OpenUri(new Uri("fb://page/108972495813848"));
        }

        private void OnClickLogout(object sender, EventArgs e)
        {
            RegisterUserModel loggedUser = App.Database.GetLoggedUser();
            if (loggedUser == null)
                return;
            loggedUser.isLoggedIn = false;
            App.Database.SaveUser(loggedUser);
            ProfilePage.pcv.Content = (new ProfileBeforeLoginPage()).Content;
        }
    }
}