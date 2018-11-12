using AsNum.XFControls;
using System;
using System.Collections.Generic;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.RangeSlider.Forms;

namespace Helpa.Views.Helpers
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ChildView : ContentView
	{
        public ChildView()
        {
            InitializeComponent();

            SetRadioList(new List<string>()
              {
                "Male",
                "Female",
                "N/A"
              }, rgPJChildGender);
        }

        private void SetRadioList(IEnumerable<string> genderList, RadioGroup radioGroup)
        {
            radioGroup.ItemsSource = genderList;

            radioGroup.SelectedItem = genderList.ElementAt<string>(0);
            StackLayout content = (StackLayout)radioGroup.Content;
            for (int index = 0; index < content.Children.Count; ++index)
            {
                Radio child = (Radio)content.Children[index];
                child.Margin = new Thickness(0.0, 0.0, 10.0, 0.0);
                child.VerticalOptions = LayoutOptions.Center;
            }
        }

        private void SetChildAge(object sender, EventArgs e)
        {
            var rs = (RangeSlider)sender;
            btnPJChildAge.Text = (rs.UpperValue / 12) + "." + (rs.UpperValue % 12);
        }
    }
}