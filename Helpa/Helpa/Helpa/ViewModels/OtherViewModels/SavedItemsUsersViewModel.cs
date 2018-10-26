using Helpa.Models;
using Helpa.Services;
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
    public class SavedItemsUsersViewModel : INotifyPropertyChanged
    {
        ObservableCollection<MyNetworkModel> savedUserList = new ObservableCollection<MyNetworkModel>();
        public ObservableCollection<MyNetworkModel> SavedUserList
        {
            get
            {
                return savedUserList;
            }
            set { SetProperty(ref savedUserList, value); }
        }


        ObservableCollection<JobsHome> savedJobPostsList = new ObservableCollection<JobsHome>();
        public ObservableCollection<JobsHome> SavedJobPostsList
        {
            get
            {
                return savedJobPostsList;
            }
            set { SetProperty(ref savedJobPostsList, value); }
        }

        public Command LoadItemsCommand { get; set; }
        
        public SavedItemsUsersViewModel()
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

                #region get saved user
                HelpersServices helpersServices = new HelpersServices();
                var savedUser = await helpersServices.GetSavedUsers(user.Id);
                SavedUserCount = savedUser.Total;
                var n = savedUser.Data;

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

                    n.Select(x => x.UserName == h.UserName ? h : x);
                }
                SavedUserList = new ObservableCollection<MyNetworkModel>(n);
                #endregion

                #region get saved job posts
                JobServices jobServices = new JobServices();
                var js = await jobServices.GetSavedJobs(user.Id);
                SavedJobsCount = js.Total;

                for (int i = 0; i < js.Data.Count(); i++)
                {
                    JobsHome j = js.Data.ElementAt(i);
                    if (j != null)
                    {
                        if (j.JobType == "N")
                        {
                            j.JobType = "Normal";
                            j.JobBgColor = "#289EF5";
                            j.JobTextColor = "#FFFFFF";
                        }
                        else if (j.JobType == "U")
                        {
                            j.JobType = "Urgent";
                            j.JobBgColor = "#F63D38";
                            j.JobTextColor = "#FFFFFF";
                        }
                        else
                        {
                            j.JobType = "Sponsored";
                            j.JobBgColor = "#FCDC55";
                            j.JobTextColor = "#000000";
                        }
                    }

                    if (j.Location != null && j.Location.Count() > 0)
                        j.JobLocationName = j.Location.ElementAt(0).LocationName;

                    if (j.IsDaily)
                        j.JobPriceLabel = "From $" + j.MinDailyPrice.Remove(j.MinDailyPrice.IndexOf(".")) + "-$" + j.MaxDailyPrice.Remove(j.MaxDailyPrice.IndexOf(".")) + "/Day";
                    else
                    if (j.IsMonthly)
                        j.JobPriceLabel = "From $" + j.MinMonthlyPrice.Remove(j.MinMonthlyPrice.IndexOf(".")) + "-$" + j.MaxMonthlyPrice.Remove(j.MaxMonthlyPrice.IndexOf(".")) + "/Month";
                    else
                    if (j.IsHourly)
                        j.JobPriceLabel = "From $" + j.MinHourlyPrice.Remove(j.MinHourlyPrice.IndexOf(".")) + "-$" + j.MaxHourlyPrice.Remove(j.MaxHourlyPrice.IndexOf(".")) + "/Hour";

                    j.createDate = j.CreateDate.Substring(0, 10);
                    js.Data.Select(x => x.JobId == j.JobId ? j : x);
                }

                SavedJobPostsList = new ObservableCollection<JobsHome>(js.Data);
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
