using System;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace Helpa
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            NavigationPage.SetHasNavigationBar(this, false);

            var map = new Map(
            MapSpan.FromCenterAndRadius(
                    new Position(37, -122), Distance.FromMiles(0.3)))
            {
                IsShowingUser = true,
                HeightRequest = 100,
                WidthRequest = 960,
                VerticalOptions = LayoutOptions.FillAndExpand
            };
            map.MapType = MapType.Street;
            
            //var stack = new StackLayout { Spacing = 0 };
            //stack.Children.Add(map);
            //Content = stack;
            grid_map.Children.Add(map);
        }
    }
}
