using Helpa.Models;
using Helpa.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;
namespace Helpa.ViewModels.OtherViewModels
{
    class MyJobPostsViewModel : INotifyPropertyChanged
    {
        public Command LoadItemsCommand { get; set; }
        public MyJobPostsViewModel()
        {
            //activityIndicator = MyJobPostPage.Instance.Content.FindByName<ActivityIndicator>("aiJobPost");
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
                await SetMyJobPostsList();
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

        async Task SetMyJobPostsList()
        {
            try
            {
                var user = App.Database.GetLoggedUser();
                JobServices jobServices = new JobServices();
                var js = await jobServices.GetMyJobs(user.Id);

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
                MyJobsList = new ObservableCollection<JobsHome>(js.Data);
                //lvMyJobPost.ItemsSource = jobsViewModel.JobFullList;
                //lblJobsCount.Text = js.Count() + " Jobs found in " + selectedHelpersInCluster.LocationName;
                //aiJobPost.IsRunning = false;
            }
            catch (Exception e)
            {
                Console.Write(e.StackTrace);
            }
        }
        ObservableCollection<JobsHome> JobsList = new ObservableCollection<JobsHome>();
        public ObservableCollection<JobsHome> MyJobsList
        {
            get
            {
                return JobsList;
            }
            set { SetProperty(ref JobsList, value); }
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