﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Helpa.Views.Profile.ProfileSettings
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class BlockUserPage : ContentPage
	{
		public BlockUserPage ()
		{
			InitializeComponent ();
            NavigationPage.SetHasNavigationBar(this, false);
		}

        private void XFBackButton_Tapped(object sender, EventArgs e)
        {
            App.NavigationPage.PopAsync();
        }
    }
}