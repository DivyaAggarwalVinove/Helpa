using Helpa.ViewModels.OtherViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Helpa.Views.OtherPages
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
    }
}