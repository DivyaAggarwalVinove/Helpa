using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Helpa
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Register1 : ContentPage
    {
        //RegisterViewModel1 vm = null;

        public Register1()
        {
            InitializeComponent();

            NavigationPage.SetHasNavigationBar(this, false);

            //vm = new RegisterViewModel1();
            //this.BindingContext = vm;
            //MyRadioGroup.CheckedChanged += MyRadioGroup_CheckedChanged;
            //this.MyRadioGroup.SelectedIndex = 3;
        }

        //void MyRadioGroup_CheckedChanged(object sender, int e)
        //{
        //    var radio = sender as CustomRadioButton;
        //    if (radio == null || radio.Id == -1) return;
        //    //this.txtSelected.Text = radio.Text;
        //    vm.SelectedIndex = this.MyRadioGroup.SelectedIndex;
        //}

        void OnSignUpNext(object sender, EventArgs args)
        {
            lParentSignUp.Text = "Parent Sign Up 2/2";
            gridLocation.BackgroundColor = Color.FromHex("#FF748C");
            svBasicInfo.IsVisible = false;
            svLocationInfo.IsVisible = true;
            //Navigation.PushAsync(new Register1());
        }

        void OnSignUpDone(object sender, EventArgs args)
        {
        }

        protected override bool OnBackButtonPressed()
        {
            if (svLocationInfo.IsVisible)
            {
                lParentSignUp.Text = "Parent Sign Up 1/2";
                gridLocation.BackgroundColor = Color.FromHex("#818A8F");
                svBasicInfo.IsVisible = true;
                svLocationInfo.IsVisible = false;

                return true;
            }
            else
            {
                return base.OnBackButtonPressed();
            }
        }
        void OnFocus(object sender, EventArgs args)
        {
            Navigation.PushAsync(new Register2());
        }
    }
}