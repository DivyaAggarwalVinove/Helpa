using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace Helpa.Models
{
    public class JHome
    {
        public IEnumerable<JobsHomeModel> Data { get; set; }
        public int Total { get; set; }
    }

    public class JobsHomeModel
    {
        public string LocationName { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int Count { get; set; }
    }

    public class JHomeModel
    {
        public IEnumerable<JobsHome> Data { get; set; }
        public int Total { get; set; }
    }

    /*
    "JobScope": [
       {
           "ScopeId": "70",
           "ScopeName": "Maths"
       }],
    "Receivers": [
       {
           "ReceiverGender": 1,
           "ReceiverAge": 9
       }],
    "Location": [
       {
           "LocationName": "Hongo Dori, Tōkyō-to, Japan",
           "DisticeId": 0,
           "Latitude": "35.7221122",
           "Longitude": "139.7548003"
       }
   ]
   "MinPrice": null,
   "MaxPrice": null,
   "JobServices": null, 
   */

    public class JobsHome : INotifyPropertyChanged
    {
        public int UserId { get; set; }

        public int JobId { get; set; }
        public string JobTitle { get; set; }
        public string JobDescription { get; set; }
        public string JobServicesName { get; set; }
        public string JobPriceLabel { get; set; }
        public int JobServicesId { get; set; }
        public bool Status { get; set; }
        
        public string HelperType { get; set; }
        public IEnumerable<LocationModel> Location { get; set; }
        public string JobLocationName { get; set; }
        public string LocationType { get; set; }

        public bool BookMark { get; set; }
        public bool IsDraft { get; set; }

        public string CreateDate { get; set; }

        string _createdDate = "";
        public string createDate
        {
            get { return _createdDate; }
            set
            {
                SetProperty(ref _createdDate, value);
            }
        }

        public int RemainingDays { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }

        public bool IsSunday { get; set; }
        public bool IsMonday { get; set; }
        public bool IsTuesday { get; set; }
        public bool IsWednesday { get; set; }
        public bool IsThursday { get; set; }
        public bool IsFriday { get; set; }
        public bool IsSaturday { get; set; }

        public bool IsHourly { get; set; }
        public bool IsDaily { get; set; }
        public bool IsMonthly { get; set; }

        public string MinHourlyPrice { get; set; }
        public string MaxHourlyPrice { get; set; }
        public string MinDailyPrice { get; set; }
        public string MaxDailyPrice { get; set; }
        public string MinMonthlyPrice { get; set; }
        public string MaxMonthlyPrice { get; set; }

        string jobType = "Urgent";
        public string JobType
        {
            get { return jobType; }
            set
            {
                SetProperty(ref jobType, value);
            }
        }

        string jobBgColor = "#00FFFFFF";
        public string JobBgColor
        {
            get { return jobBgColor; }
            set {
                SetProperty(ref jobBgColor, value);
            }
        }

        string jobTextColor = "#00FFFFFF";
        public string JobTextColor
        {
            get { return jobTextColor; }
            set
            {
                SetProperty(ref jobTextColor, value);
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
 