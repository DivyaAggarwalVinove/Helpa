using Plugin.Geolocator;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Helpa.Models
{
    public class LocationModel : INotifyPropertyChanged
    {
        public string LocationName { get; set; }
        public int DisticeId { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        
        //public ObservableCollection<LocationModel> locations { get; set; } 

        string address = "";
        public string Address
        {
            get { return address; }
            set
            {
                SetProperty(ref address, value);
            }
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
