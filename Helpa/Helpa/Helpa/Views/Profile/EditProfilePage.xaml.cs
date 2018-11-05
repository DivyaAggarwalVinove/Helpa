﻿using Helpa.Views.Profile;
using Helpa.Views.Profile.UserProfile;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Helpa
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class EditProfilePage : ContentPage
	{
        public EditProfilePage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        public void OnClickBasicInfo(object sender, EventArgs e)
        {
            App.NavigationPage.Navigation.PushAsync(new EditBasicInfo());
        }

        public void OnClickServices(object sender, EventArgs e)
        {
            App.NavigationPage.Navigation.PushAsync(new Service());
        }

        public void OnClickVerification(object sender, EventArgs e)
        {
            App.NavigationPage.Navigation.PushAsync(new EditVerificationPage());
        }

        private void OnBackPress(object sender, EventArgs e)
        {
            App.NavigationPage.Navigation.PopAsync();
        }

    }
}