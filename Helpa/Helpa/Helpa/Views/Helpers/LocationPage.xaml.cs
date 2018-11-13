using DurianCode.PlacesSearchBar;
using Helpa.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Helpa.Views.Helpers
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LocationPage : ContentPage
	{
        private Page page;
        public string selectedLoc;
        private List<AutoCompletePrediction> selectedPrediction;
        private List<string> description { get; set; }
        
        public LocationPage (Page page)
		{
			InitializeComponent ();

            this.page = page;
            pbPJLocation.ApiKey = Constants.googlePlaceApiKey;
            pbPJLocation.Text = selectedLoc;

            NavigationPage.SetHasNavigationBar(this, false);
        }

        private void OnPlacesRetrieved(object sender, AutoCompleteResult result)
        {
            if (!(result.Status == "OK"))
                return;
            description = new List<string>();
            selectedPrediction = result.AutoCompletePlaces;
            foreach (AutoCompletePrediction autoCompletePlace in result.AutoCompletePlaces)
            {
                //SfAutoCompleteItem autoCompleteItem = new SfAutoCompleteItem(autoCompletePlace.Description, "location.png");
                description.Add(autoCompletePlace.Description);
            }
            if (description.Count == 0)
                return;
            listView.ItemsSource = description;
        }

        private void OnLocationSelected(object sender, SelectedItemChangedEventArgs e)
        {
            string selectedLoc = ((ListView)sender).SelectedItem.ToString();
            pbPJLocation.Text = selectedLoc;

            PostJobPage page = (PostJobPage)this.page;
            page.selectedLocation = selectedLoc;
            page.selectedPrediction = selectedPrediction.Where(x => x.Description == selectedLoc).FirstOrDefault();

            listView.SelectedItem = null;

            Navigation.PopAsync();
        }
    }
}