using AsNum.XFControls;
using DurianCode.PlacesSearchBar;
using Helpa.Models;
using System.Linq;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections;
using System;

namespace Helpa.Views.Helpers
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PostJobPage : ContentPage
    {
        private IList<ScopeModel> scopes;
        private IList<ServiceModel> services;
        public RegisterUserModel loggedUser;
        private int selectedService;
        public string selectedLocation;
        public AutoCompletePrediction selectedPrediction;

        public PostJobPage()
        {
            InitializeComponent();

            NavigationPage.SetHasNavigationBar(this, false);

            SetRadioList(new List<string>()
            {
                "Normal",
                "Urgent"
            }, rgPJJobStatus);

            SetRadioList(new List<String>()
            {
                "Helper's Home",
                "Mobile Helper"
            }, rgPJHelperType);

            SetServices();
        }

        private void SetRadioList(IEnumerable<string> genderList, RadioGroup radioGroup)
        {
            radioGroup.ItemsSource = (IEnumerable)genderList;
            radioGroup.SelectedItem = (object)genderList.ElementAt<string>(0);
            StackLayout content = (StackLayout)radioGroup.Content;
            for (int index = 0; index < content.Children.Count; ++index)
            {
                Radio child = (Radio)content.Children[index];
                child.Margin = new Thickness(0.0, 0.0, 25.0, 0.0);
                child.VerticalOptions = LayoutOptions.Center;
            }
        }

        private async void SetServices()
        {
            //PostJob postJob = this;
            IList<ServiceModel> servicesAsync = await new Helpa.Services.Utilities().GetServicesAsync();
            services = servicesAsync;
            int num1 = services.Count<ServiceModel>();

            for (int index = 0; index < (num1 - 1) / 3 + 1; ++index)
                gridPJServices.RowDefinitions.Add(new RowDefinition()
                {
                    Height = GridLength.Auto
                });

            for (int index = 0; index < 3; ++index)
                gridPJServices.ColumnDefinitions.Add(new ColumnDefinition()
                {
                    Width = GridLength.Star
                });
            gridPJServices.Padding = new Thickness(0.0, 0.0, 10.0, 0.0);

            for (int index = 0; index < num1; ++index)
            {
                Button button = new Button();
                button.BackgroundColor = Color.FromHex("F1F5F6");
                button.CornerRadius = 3;
                button.BorderColor = Color.FromHex("BCC1C4");
                button.BorderWidth = 1.0;
                button.HorizontalOptions = LayoutOptions.FillAndExpand;
                button.Margin = new Thickness(5.0, 0.0);
                button.TextColor = Color.FromHex("BCC1C4");
                button.Text = services.ElementAt<ServiceModel>(index).ServiceName;

                // ISSUE: reference to a compiler-generated method
                //button.Clicked += new EventHandler(postJob.\u003CSetServices\u003Eb__5_0);
                button.Clicked += (sender, e) =>
                 {
                     Button btn = (Button)sender;

                     if (btn.BorderColor == Color.FromHex("BCC1C4"))
                     {
                         Grid grid = (Grid)btn.Parent;
                         Button btnSelectedService = (Button)grid.Children.ElementAt(selectedService * 2);
                         btnSelectedService.BorderColor = Color.FromHex("BCC1C4");
                         btnSelectedService.TextColor = Color.FromHex("BCC1C4");

                         Image imgSelected = (Image)grid.Children.ElementAt(selectedService * 2 + 1);
                         imgSelected.IsVisible = false;

                         btn.BorderColor = Color.FromHex("FE7890");
                         btn.TextColor = Color.FromHex("FE7890");

                         selectedService = services.IndexOf(services.Where(x => x.ServiceName == btn.Text).FirstOrDefault());
                         Image img = (Image)grid.Children.ElementAt(selectedService * 2 + 1);
                         img.IsVisible = true;

                         
                     }
                 };

                Image image = new Image();
                image.Source = "selected.png";
                image.Aspect = Aspect.AspectFit;
                image.Margin = new Thickness(15.0, 0.0, -5.0, 0.0);
                image.HorizontalOptions = LayoutOptions.End;
                image.IsVisible = false;
                gridPJServices.Children.Add((View)button, index % 3, index / 3);
                gridPJServices.Children.Add((View)image, index % 3, index / 3);
            }

            //SetScopes(postJob.services.ElementAt<ServiceModel>(postJob.selectedService).Id);
            ((Button)gridPJServices.Children[0]).BorderColor = Color.FromHex("FE7890");
            ((Button)gridPJServices.Children[0]).TextColor = Color.FromHex("FE7890");
            gridPJServices.Children[1].IsVisible = true;
            App.Database.DeleteServiceAsync();
            int num2 = await App.Database.SaveServicesAsync((IEnumerable<ServiceModel>)services);
        }

        private void OnFocus(object sender, FocusEventArgs e)
        {
           /* Navigation.PushAsync((Page)new LocationPage((Page)this)
            {
                selectedLoc = this.entryPJLocation.Text
            });*/
        }

        private void OnBackPress(object sender, EventArgs e)
        {
            App.NavigationPage.Navigation.PopAsync();
        }
    }
}