using AsNum.XFControls;
using Helpa.Models;
using Helpa.Services;
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
	public partial class Service : ContentPage
	{
		public Service ()
		{
			InitializeComponent ();
            NavigationPage.SetHasNavigationBar(this, false);
            IEnumerable<string> statusList = new List<string>() { "Available", "Not available" };
            SetRadioList(statusList, rgStatusService);
            SetServices();
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

        async void SetServices()
        {
            #region set services
            IList<ServiceModel> services = await (new Utilities()).GetServicesAsync();

            int servicesCount = services.Count();
            for (int i = 0; i < (servicesCount - 1) / 3 + 1; i++)
                gridServices.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            for (int i = 0; i < 3; i++)
                gridServices.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

            ServiceButton serviceButton;
            for (int i = 0; i < servicesCount; i++)
            {
                serviceButton = new ServiceButton();
                serviceButton.Text = services.ElementAt(i).ServiceName;
                serviceButton.Margin = new Thickness(5, 5, 5, 5);

                Image image = new Image();
                image.Source = "selected.png";
                image.Aspect = Aspect.AspectFit;
                image.Margin = new Thickness(20, 15, -5, 15);
                image.HorizontalOptions = LayoutOptions.End;
                image.IsVisible = false;

                gridServices.Children.Add(serviceButton, i % 3, i / 3);
                gridServices.Children.Add(image, i % 3, i / 3);
                #endregion
            }
            App.Database.DeleteServiceAsync();
            await App.Database.SaveServicesAsync(services);
        }

        private void XFBTNServiceDetail_Tapped(object sender, EventArgs e)
        {
            App.NavigationPage.Navigation.PushAsync(new ServiceDetailPage());
        }

        private void XFBackButton_Tapped(object sender, EventArgs e)
        {
            App.NavigationPage.Navigation.PopAsync();
        }
    }
}