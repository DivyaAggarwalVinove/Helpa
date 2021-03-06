﻿using Helpa.Models;
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
    public partial class ProfilePage : ContentPage
    {
        public static ContentView pcv;
        public ProfilePage()
        {
            this.InitializeComponent();
            NavigationPage.SetHasNavigationBar((BindableObject)this, false);
            pcv = ProfileContentView;

            GoToProfilePage();
        }

        void GoToProfilePage()
        {
            try
            {
                RegisterUserModel loggedUser = App.Database.GetLoggedUser();
                if (loggedUser == null)
                    ProfileContentView.Content = (new ProfileBeforeLoginPage()).Content;
                else
                    ProfileContentView.Content = (new ProfileAfterLoginPage(loggedUser)).Content;
            }
            catch(Exception e)
            {
                Console.Write(e.Message);
            }
        }
    }
}