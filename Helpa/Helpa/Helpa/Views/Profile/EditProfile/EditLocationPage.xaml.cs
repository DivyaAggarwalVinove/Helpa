using DurianCode.PlacesSearchBar;
using Helpa.Utility;
using Helpa.Views.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Helpa.Views.Profile.EditProfile
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class EditLocationPage : ContentPage, INotifyPropertyChanged
    {
        private Page page;

        string selectedLoc = "";
        public string SelectedLoc
        {
            get
            {
                return selectedLoc;
            }

            set
            {
                SetProperty(ref selectedLoc, value);
            }
        }

        private List<AutoCompletePrediction> selectedPrediction;
        private List<string> description { get; set; }
        
        public EditLocationPage(Page page)
		{
			InitializeComponent ();

            this.page = page;
            pbEPLocation.ApiKey = Constants.googlePlaceApiKey;
            
            NavigationPage.SetHasNavigationBar(this, false);

            BindingContext = this;
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
            pbEPLocation.Text = selectedLoc;

            EditBasicInfo page = (EditBasicInfo)this.page;
            page.selectedLocation = selectedLoc;
            page.selectedPrediction = selectedPrediction.Where(x => x.Description == selectedLoc).FirstOrDefault();

            listView.SelectedItem = null;

            Navigation.PopAsync();
        }

        protected bool SetProperty<T>(ref T backingStore, T value,
           [CallerMemberName]string propertyName = "",
           Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}