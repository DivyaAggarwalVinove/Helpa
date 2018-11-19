using Helpa.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.RangeSlider.Forms;
namespace Helpa.Views.Profile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditVerificationPage : ContentPage
    {
        public EditVerificationPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            //SetPriceType();           
        }
        private void XFBackButton_Tapped(object sender, EventArgs e)
        {
            App.NavigationPage.Navigation.PopAsync();
        }
        //void SetPriceType()
        //{
        //    List<string> price = new List<string> { "Hourly", "Daily", "Monthly", "TBD" };
        //    foreach (View gPrice in gridPriceType.Children.ToList())
        //    {
        //        gridPriceType.Children.Remove(gPrice);
        //    }
        //    gridPriceType.RowDefinitions = new RowDefinitionCollection();
        //    gridPriceType.ColumnDefinitions = new ColumnDefinitionCollection();
        //    gridPriceType.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
        //    foreach (string p in price)
        //        gridPriceType.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
        //    ServiceButton priceButton;
        //    for (int i = 0; i < price.Count; i++)
        //    {
        //        priceButton = new ServiceButton();
        //        priceButton.Text = price.ElementAt(i);
        //        priceButton.Margin = new Thickness(5, 5, 5, 5);
        //        //Image image = new Image();
        //        //image.Source = "selected.png";
        //        //image.Aspect = Aspect.AspectFit;
        //        //image.Margin = new Thickness(20, 15, 0, 15);
        //        //image.HorizontalOptions = LayoutOptions.End;
        //        //image.IsVisible = false;
        //        gridPriceType.Children.Add(priceButton, i, 0);
        //        //gridPriceType.Children.Add(image, i, 0);
        //    }
        //}
        //void OnHomeLocationSelected(object sender, EventArgs args)
        //{
        //    //rgHelperHomeLocation.Checked = !rgHelperHomeLocation.Checked;
        //    if (rgHelperHomeLocation.Checked)
        //    {
        //        rgHelperMobileLocation.Checked = !rgHelperMobileLocation.Checked;
        //        btnHelperRegDistrict.IsVisible = false;
        //        entryHelperRegLocation.IsVisible = true;
        //    }
        //    else
        //        rgHelperHomeLocation.Checked = !rgHelperHomeLocation.Checked;
        //}
        //void OnMobileLocationSelected(object sender, EventArgs args)
        //{
        //    if (rgHelperMobileLocation.Checked)
        //    {
        //        rgHelperHomeLocation.Checked = !rgHelperHomeLocation.Checked;
        //        btnHelperRegDistrict.IsVisible = true;
        //        entryHelperRegLocation.IsVisible = false;
        //    }
        //    else
        //        rgHelperMobileLocation.Checked = !rgHelperMobileLocation.Checked;
        //    //rgHelperMobileLocation.Checked = !rgHelperMobileLocation.Checked;
        //}
        //void OnFocus(object sender, EventArgs args)
        //{
        //    Navigation.PushAsync(new HelperRegister2());
        //}
        //void SetMinHour(object sender, EventArgs args) =>
        //   btnHelperRegPriceHrMin.Text = "$ " + ((RangeSlider)sender).LowerValue.ToString();
        //void SetMaxHour(object sender, EventArgs args) =>
        //    btnHelperRegPriceHrMax.Text = "$ " + ((RangeSlider)sender).UpperValue.ToString();
        //void SetMinDay(object sender, EventArgs args) =>
        //    btnHelperRegPriceDayMin.Text = "$ " + ((RangeSlider)sender).LowerValue.ToString();
        //void SetMaxDay(object sender, EventArgs args) =>
        //    btnHelperRegPriceDayMax.Text = "$ " + ((RangeSlider)sender).UpperValue.ToString();
        //void SetMinMonth(object sender, EventArgs args) =>
        //    btnHelperRegPriceMonthMin.Text = "$ " + ((RangeSlider)sender).LowerValue.ToString();
        //void SetMaxMonth(object sender, EventArgs args) =>
        //    btnHelperRegPriceMonthMax.Text = "$ " + ((RangeSlider)sender).UpperValue.ToString();       
    }
}