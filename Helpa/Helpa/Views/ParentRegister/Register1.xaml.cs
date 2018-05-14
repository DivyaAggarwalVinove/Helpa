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

        public Register1 ()
		{
			InitializeComponent ();

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
    }
}