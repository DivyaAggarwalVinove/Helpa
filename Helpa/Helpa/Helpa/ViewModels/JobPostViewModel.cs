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
using Xamarin.Forms.Maps;

namespace Helpa.ViewModels
{
    public class JobPostViewModel : INotifyPropertyChanged
    {
        //public IHelpersServices<HelperHomeModel> DataStore => DependencyService.Get<IHelpersServices<HelperHomeModel>>();
        public ObservableCollection<JobsHomeModel> jobPostList { get; set; }
        public ObservableCollection<JobsHome> JobFullList { get; set; }

        ObservableCollection<JobsHome> jobPostHalfList = new ObservableCollection<JobsHome>();
        public ObservableCollection<JobsHome> JobPostHalfList
        {
            get
            {
                return jobPostHalfList;
            }
            set { SetProperty(ref jobPostHalfList, value); }
        }

        public Command LoadItemsCommand { get; set; }

        public CustomMap mapJob;
        public ActivityIndicator activityIndicator;

        public JobPostViewModel(JobPosts jobPosts)
        {
            activityIndicator = JobPosts.Instance.Content.FindByName<ActivityIndicator>("aiJobPost");

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
                JobFullList = new ObservableCollection<JobsHome>();
                JobPostHalfList = new ObservableCollection<JobsHome>();

                JobServices jobServices = new JobServices();
                var j  = await jobServices.GetJobsList(0);
                if (j != null && (j.Count() > 0))
                    jobPostList = new ObservableCollection<JobsHomeModel>(j.ToList());

                //for (int i = 0; i < jobPostList.Count; i++)
                //{
                //    jobPostList.ElementAt(i).LocationType = ' ';
                //}

                SetLocationOnMap();

                #region Check Location Permission
                //var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);
                //if (status == PermissionStatus.Granted)
                //{
                //    ProgressBar pb = new ProgressBar();

                //    activityIndicator.IsRunning = true;

                //    //var position = await CrossGeolocator.Current.GetPositionAsync();
                //    var position = jobPostList.ElementAt(0);
                //    mapJob.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(position.Latitude, position.Longitude), Distance.FromKilometers(1)));
                //    mapJob.IsShowingUser = true;
                //    mapJob.MapType = MapType.Street;

                //    activityIndicator.IsRunning = false;

                //    #region Call api to get data from server
                //    // var helpers = await DataStore.GetHelpersList(10000, 28.4514279, 77.0704678);
                //    // var helpers = await (new HelpersServices()).GetHelpersList(2000000, position.Latitude, position.Longitude);
                //    // HelperList = new ObservableCollection<HelperHome>(helpers.First().HelpersInLocalties);
                //    #endregion
                //}
                #endregion
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

        public void SetLocationOnMap()
        {
            try
            {
                int i = 0;
                var t = "0";
                foreach (JobsHomeModel h in jobPostList)
                {
                    mapJob.selectedJob = jobPostList.ElementAt(i);
                    var pin = new Pin()
                    {
                        Type = PinType.Place,
                        Position = new Position(h.Latitude, h.Longitude),
                        Label = h.LocationName + " (" + h.Count + ")",
                        Address = i.ToString(),
                        Id = i.ToString()
                    };
                    mapJob.Pins.Add(pin);

                    t = (int.Parse(t) + h.Count).ToString();
                    i++;
                }

                var position = jobPostList.ElementAt(0);
                mapJob.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(position.Latitude, position.Longitude), Distance.FromKilometers(1)));
                mapJob.IsShowingUser = true;
                mapJob.MapType = MapType.Street;

                TotallJobs = t;
            }
            catch (Exception e)
            {
                Debug.Write(e.Message);
            }
        }

        string totalJobs = "?";
        public string TotallJobs
        {
            get
            {
                return totalJobs;
            }
            set
            {
                SetProperty(ref totalJobs, value);
            }
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
