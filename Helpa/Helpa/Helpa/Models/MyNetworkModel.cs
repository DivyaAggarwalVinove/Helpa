using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace Helpa.Models
{
    public class NetworkModel
    {
        public IEnumerable<MyNetworkModel> Data { get; set; }
        public int Total { get; set; }
    }

    public class MyNetworkModel
    {
        public int FavourateId { get; set; }
        public int? HelperID { get; set; }
        public bool BookMark { get; set; }
        public string Image { get; set; }
        public int NoOfService { get; set; }
        public int NoOfJobPost { get; set; }
        public LocationModel Location { get; set; }
        public string UserName { get; set; }
        public int Rating { get; set; }
        public int NoOfConnetion { get; set; }
        public int Type { get; set; }
        public bool Status { get; set; }
        string _helperStatus = "Available";
        public string helperStatus
        {
            get { return _helperStatus; }
            set { SetProperty(ref _helperStatus, value); }
        }

        public int NoOfUniqueUserCount { get; set; }
        public string AverageRatingCount { get; set; }

        public int NoOfRatingUserCount { get; set; }
        public List<Helper_Services> Service { get; set; }
        public DateTime CreatedDate { get; set; }

        public string ServiceLabel
        {

            get
            {
                return string.Format("Services ({0}). Job Posts({1}) ", NoOfService, NoOfJobPost);
            }
        }

        string _bgcolor = "#00FFFFFF";
        public string bgcolor
        {
            get { return _bgcolor; }
            set
            {
                SetProperty(ref _bgcolor, value);
            }
        }

        string _textcolor = "#00FFFFFF";
        public string textcolor
        {
            get { return _textcolor; }
            set
            {
                SetProperty(ref _textcolor, value);
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

    public class Helper_Services
    {
        public int ServiceId { get; set; }
        public string ServiceName { get; set; }
        public HSPrice price { get; set; }
        public List<ScopeModel> Scope { get; set; }
        public List<LocationModel> Location { get; set; }
        public string LocationType { get; set; }

    }
}
