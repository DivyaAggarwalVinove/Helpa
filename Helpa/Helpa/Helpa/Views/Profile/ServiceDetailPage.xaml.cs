using AsNum.XFControls;
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
	public partial class ServiceDetailPage : ContentPage
	{
		public ServiceDetailPage ()
		{
			InitializeComponent ();
            NavigationPage.SetHasNavigationBar(this, false);
            //IEnumerable<string> genders = new List<string>() { "Helper's Home", "Mobile Helper" };
            //SetRadioList(genders, rgGender);
            SetPriceType();
            MessagingCenter.Subscribe<ServiceButton, bool>(this, "Select Or Unselect Service", (sender, isSelected) =>
            {
                try
                {
                    string serviceName = ((ServiceButton)sender).serviceName;

                    if (string.Equals(serviceName, "Hourly"))
                    {
                        if (((ServiceButton)sender).isSelected)
                            gridPriceHour.IsVisible = true;
                        else
                            gridPriceHour.IsVisible = false;
                    }
                    else if (string.Equals(serviceName, "Daily"))
                    {
                        if (((ServiceButton)sender).isSelected)
                            gridPriceDay.IsVisible = true;
                        else
                            gridPriceDay.IsVisible = false;
                    }
                    else if (string.Equals(serviceName, "Monthly"))
                    {
                        if (((ServiceButton)sender).isSelected)
                            gridPriceMonth.IsVisible = true;
                        else
                            gridPriceMonth.IsVisible = false;
                    }
                    //else
                    //{
                    //    List<ServiceModel> services = App.Database.GetServicesAsync();
                    //    ServiceModel service = services.Where(a => a.ServiceName == serviceName).FirstOrDefault();
                    //    //var selectedService = helperServices.Service.Where(a => a.ServiceId == service.Id).FirstOrDefault();
                    //    int serviceIndex = services.IndexOf(service);
                    //    var gService = gridServices.Children.Where(c => Grid.GetRow(c) == (serviceIndex) / 3 && Grid.GetColumn(c) == (serviceIndex) % 3);
                    //    gService.ElementAt(1).IsVisible = !gService.ElementAt(1).IsVisible;

                    //    if (isSelected)
                    //    {
                    //        AddGrid(gridHelperEditServices);

                    //        HelperService helperService = new HelperService();
                    //        helperService.ServiceId = service.Id;
                    //        helperService.ServiceName = service.ServiceName;
                    //        helperServices.Service.Add(helperService);

                    //        //selectedService.isSelected = true;
                    //        //App.Database.SaveServiceAsync(selectedService);
                    //    }
                    //    else
                    //    {
                    //        int count = gridHelperEditServices.ColumnDefinitions.Count;

                    //        for (int i = count - 1; i >= 0; i--)
                    //        {
                    //            gridHelperEditServices.ColumnDefinitions.RemoveAt(i);
                    //            gridHelperEditServices.Children.RemoveAt(i);
                    //        }

                    //        for (int i = 0; i < count - 1; i++)
                    //            AddGrid(gridHelperEditServices);

                    //        HelperService helperService = helperServices.Service.Where(a => a.ServiceId == service.Id).FirstOrDefault();
                    //        helperServices.Service.Remove(helperService);

                    //        //selectedService.isSelected = false;
                    //        //App.Database.SaveServiceAsync(selectedService);
                    //    }

                    //    var g = gridHelperEditServices.Children.Where(c => Grid.GetRow(c) == 0 && Grid.GetColumn(c) == 0).First();
                    //    g.BackgroundColor = Color.FromHex("#FF748C");
                    //}
                }
                catch (Exception e)
                {
                    Console.Write(e.StackTrace);
                }
            });
        }
        void SetRadioList(IEnumerable<string> genderList, RadioGroup radioGroup)
        {
            radioGroup.ItemsSource = genderList;

            radioGroup.SelectedItem = genderList.ElementAt(0);

            StackLayout content = (StackLayout)radioGroup.Content;
            Radio rg = null;
            for (int i = 0; i < content.Children.Count; i++)
            {
                rg = (Radio)(content.Children[i]);
                rg.Margin = new Thickness(0, 0, 10, 0);
                rg.VerticalOptions = LayoutOptions.Center;
            }
        }

        private void OnFocus(object sender, FocusEventArgs e)
        {

        }
        void OnHomeLocationSelected(object sender, EventArgs args)
        {
            //rgHelperHomeLocation.Checked = !rgHelperHomeLocation.Checked;
            if (rgHelperHomeLocation.Checked)
            {
                rgHelperMobileLocation.Checked = !rgHelperMobileLocation.Checked;

                btnHelperRegDistrict.IsVisible = false;
                entryHelperRegLocation.IsVisible = true;
            }
            else
                rgHelperHomeLocation.Checked = !rgHelperHomeLocation.Checked;
        }

        void OnMobileLocationSelected(object sender, EventArgs args)
        {
            if (rgHelperMobileLocation.Checked)
            {
                rgHelperHomeLocation.Checked = !rgHelperHomeLocation.Checked;

                btnHelperRegDistrict.IsVisible = true;
                entryHelperRegLocation.IsVisible = false;
            }
            else
                rgHelperMobileLocation.Checked = !rgHelperMobileLocation.Checked;
            //rgHelperMobileLocation.Checked = !rgHelperMobileLocation.Checked;
        }
        void SetPriceType()
        {
            List<string> price = new List<string> { "Hourly", "Daily", "Monthly", "TBD" };

            foreach (View gPrice in gridPriceType.Children.ToList())
            {
                gridPriceType.Children.Remove(gPrice);
            }

            gridPriceType.RowDefinitions = new RowDefinitionCollection();
            gridPriceType.ColumnDefinitions = new ColumnDefinitionCollection();

            gridPriceType.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            foreach (string p in price)
                gridPriceType.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            ServiceButton priceButton;
            for (int i = 0; i < price.Count; i++)
            {
                priceButton = new ServiceButton();
                priceButton.Text = price.ElementAt(i);
                priceButton.Margin = new Thickness(5, 5, 5, 5);

                //Image image = new Image();
                //image.Source = "selected.png";
                //image.Aspect = Aspect.AspectFit;
                //image.Margin = new Thickness(20, 15, 0, 15);
                //image.HorizontalOptions = LayoutOptions.End;
                //image.IsVisible = false;

                gridPriceType.Children.Add(priceButton, i, 0);
                //gridPriceType.Children.Add(image, i, 0);
            }
        }
        void SetMinHour(object sender, EventArgs args) =>
                   btnHelperRegPriceHrMin.Text = "$ " + ((RangeSlider)sender).LowerValue.ToString();

        void SetMaxHour(object sender, EventArgs args) =>
            btnHelperRegPriceHrMax.Text = "$ " + ((RangeSlider)sender).UpperValue.ToString();

        void SetMinDay(object sender, EventArgs args) =>
            btnHelperRegPriceDayMin.Text = "$ " + ((RangeSlider)sender).LowerValue.ToString();

        void SetMaxDay(object sender, EventArgs args) =>
            btnHelperRegPriceDayMax.Text = "$ " + ((RangeSlider)sender).UpperValue.ToString();

        void SetMinMonth(object sender, EventArgs args) =>
            btnHelperRegPriceMonthMin.Text = "$ " + ((RangeSlider)sender).LowerValue.ToString();

        void SetMaxMonth(object sender, EventArgs args) =>
            btnHelperRegPriceMonthMax.Text = "$ " + ((RangeSlider)sender).UpperValue.ToString();

        //void SetMinAge(object sender, EventArgs args) =>
        //    btnHelperRegAgeMin.Text = ((RangeSlider)sender).LowerValue.ToString();

        //void SetMaxAge(object sender, EventArgs args) =>
        //    btnHelperRegAgeMax.Text = ((RangeSlider)sender).UpperValue.ToString();
    }
}