using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace Helpa.Models
{
    public class HHome
    {
        public IEnumerable<HelperHomeModel> Data { get; set; }
        public int Total { get; set; }
    }

    public class HelperHomeModel
    {
        public string LocationName { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int Count { get; set; }
        public char LocationType { get; set; }
        public List<HelperHome> HelpersInLocalties { get; set; }
        //public string price { get; set; }
        //public string location { get; set; }
        //public string status { get; set; }
        //public string profileImage { get; set; }
        //public float rating { get; set; }
        //public string rating_count { get; set; }
        //public int count1 { get; set; }
        //public int count2 { get; set; }
    }

    public class HHomeModel
    {
        public IEnumerable<HelperHome> Data { get; set; }
        public int Total { get; set; }
    }

    public class HelperHome : INotifyPropertyChanged
    {
        public int UserId { get; set; }
        public int HelperId { get; set; }
        public string Name { get; set; }
        public bool BookMark { get; set; }
        public string ProfilePicture { get; set; }

        public string AverageRating { get; set; }

        public string RatingCount { get; set; }

        [DisplayName("RatingCount")]
        public string AverageRatingCount { get; set; }

        public string Price { get; set; }

        public string ServiceName { get; set; }
        public string ServicePriceLabel { get; set; }
        public string ServiceLocationName { get; set; }

        public IEnumerable<HService> Service { get; set; }

        [DisplayName("no_of_connections")]
        public int ConnectionCount { get; set; }

        [DisplayName("EnquireCount")]
        public int EnquiredUserCount { get; set; }

        public bool Status { get; set; }
        public string Description { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string LocationName { get; set; }
        public string JobType { get; set; }
        public string JobBgColor { get; set; }

        string _helperStatus="Available";
        public string helperStatus
        {
            get { return _helperStatus; }
            set { SetProperty(ref _helperStatus, value); }
        }

        string _bgcolor = "#00FFFFFF";
        public string bgcolor
        {
            get { return _bgcolor; }
            set {
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

    public class HService
    {
        public int ServiceId { get; set;}
        public string ServiceName { get; set; }
        public HSPrice price { get; set; }
        public IEnumerable<ScopeModel> Scope { get; set; }
        public IEnumerable<LocationModel> Location { get; set; }
        public string LocationType { get; set; }
    }

    public class HSPrice
    {
        public bool Hours { get; set; }
        public bool Monthly { get; set; }
        public bool Daily { get; set; }

        public string Min { get; set; }
        public string Max { get; set; }
    }
}