using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace Helpa.Models
{
    public class HelperHomeModel
    {
        public string LocalityName { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int NumberOfHelpersInLocality { get; set; }
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

    public class HelperHome : INotifyPropertyChanged
    {
        public string Price { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public string ProfilePicture { get; set; }
        public string AverageRating { get; set; }
        public string RatingCount { get; set; }
        public string AverageRatingCount { get; set; }
        public string Service { get; set; }
        public int ConnectionCount { get; set; }
        public int EnquiredUserCount { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string LocationName { get; set; }
        public string JobType { get; set; }
        public string JobBgColor { get; set; }

        string _color = "#00FFFFFF";
        public string color
        {
            get { return _color; }
            set {
                SetProperty(ref _color, value);
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