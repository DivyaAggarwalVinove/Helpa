using Helpa.ViewModels;
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
    public partial class MyNetworkPage : ContentPage
    {
        public MyNetworkPage()
        {
            InitializeComponent();

            NavigationPage.SetHasNavigationBar(this, false);
            BindingContext = new MyNetworkViewModel();
        }
        private void OnBackPress(object sender, EventArgs e)
        {
            App.NavigationPage.Navigation.PopAsync();
        }
    }
}