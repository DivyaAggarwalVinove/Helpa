using Helpa.Models;
using Helpa.Services;
using Helpa.Services.Class;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Helpa.ViewModels
{
    public class MyReviewViewModel : INotifyPropertyChanged
    {
        ObservableCollection<MyNetworkModel> toReviewList = new ObservableCollection<MyNetworkModel>();
        public ObservableCollection<MyNetworkModel> ToReviewList
        {
            get
            {
                return toReviewList;
            }
            set { SetProperty(ref toReviewList, value); }
        }

        ObservableCollection<ReviewModel> aboutMeList = new ObservableCollection<ReviewModel>();
        public ObservableCollection<ReviewModel> AboutMeList
        {
            get
            {
                return aboutMeList;
            }
            set { SetProperty(ref aboutMeList, value); }
        }

        ObservableCollection<ReviewModel> byMeList = new ObservableCollection<ReviewModel>();
        public ObservableCollection<ReviewModel> ByMeList
        {
            get
            {
                return byMeList;
            }
            set { SetProperty(ref byMeList, value); }
        }
        public Command LoadItemsCommand { get; set; }

        public MyReviewViewModel()
        {
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            LoadItemsCommand.Execute(null);
        }
        public async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;
            IsBusy = true;
            try
            {
                var user = App.Database.GetLoggedUser();
                #region Get ToReview List
                var toReview = await (new MyReviewServices()).GetReviewToReview(user.Id);
                SavedUserCount = toReview.Total;
                var n = toReview.Data;
                for (int i = 0; i < n.Count(); i++)
                {
                    MyNetworkModel h = n.ElementAt(i);
                    h.AverageRatingCount = h.Rating.ToString() + " (" + h.NoOfRatingUserCount.ToString() + ")";
                    if (h.Status)
                    {
                        h.bgcolor = "#32BDA0";
                        h.textcolor = "#FFFFFF";
                        h.helperStatus = "Available";
                    }
                    else
                    {
                        h.bgcolor = "#EAE9E9";
                        h.textcolor = "#000000";
                        h.helperStatus = "Not Available";
                    }

                    if (h.BookMark)
                        h.BookmarkImage = "save_filled.png";
                    else
                        h.BookmarkImage = "save.png";
                    n.Select(x => x.UserName == h.UserName ? h : x);
                }
                ToReviewList = new ObservableCollection<MyNetworkModel>(n);
                #endregion

                #region Get Review List by me
                // byMeList = await (new MyReviewServices()).GetReviewByMe(user.Id);
                #endregion

                #region Get Review List about me
                var ReviewAboutMe = await (new MyReviewServices()).GetReviewAboutMe(user.Id);
                if(ReviewAboutMe.Total!=0)
                {
                    AboutMeList = ReviewAboutMe.Data;
                }
                #endregion
                //aiFindHelper.IsRunning = false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
        int savedUserCount = 0;
        public int SavedUserCount
        {
            get { return savedUserCount; }
            set { SetProperty(ref savedUserCount, value); }
        }
        int savedJobsCount = 0;
        public int SavedJobsCount
        {
            get { return savedJobsCount; }
            set { SetProperty(ref savedJobsCount, value); }
        }
        bool isBusy = false;
        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
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