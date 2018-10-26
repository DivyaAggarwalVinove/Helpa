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

                #region Constant Data
                //#region Cluster 1
                //HelperHome helper = new HelperHome();
                //helper.Name = "Army Rose";
                //helper.Service = "Childcare";
                //helper.Price = "From $100 per hour";
                //helper.Status = "Available";
                //helper.ProfilePicture = "picture2.png";
                //helper.AverageRating = "4.3";
                //helper.AverageRatingCount = "4.3(" + "32)";
                //helper.ConnectionCount = 37;
                //helper.EnquiredUserCount = 44;
                //helper.Latitude = 28.4512804;
                //helper.Longitude = 77.0703797;
                //helper.LocationName = "Estel Technologies";
                //helper.UserId = 1;
                //helper.JobType = "Normal";
                //helper.JobBgColor = "#299DF4";
                //helper.color = "#00FFFFFF";
                //JobFullList.Add(helper);
                //JobPostHalfList.Add(helper);

                //helper = new HelperHome();
                //helper.Name = "HSBC";
                //helper.Service = "Bank";
                //helper.Price = "That is reliable";
                //helper.Status = "Sponsored";
                //helper.ProfilePicture = "message_picture3.png";
                //helper.AverageRating = "0.0";
                //helper.AverageRatingCount = "0.0(" + "0)";
                //helper.ConnectionCount = 0;
                //helper.EnquiredUserCount = 0;
                //helper.Latitude = 28.4508911;
                //helper.Longitude = 77.0707457;
                //helper.LocationName = "FIITJEE";
                //helper.UserId = 2;
                //helper.color = "#00FFFFFF";
                //helper.JobType = "Sponsored";
                //helper.JobBgColor = "#FCDB54";
                //JobFullList.Add(helper);
                //JobPostHalfList.Add(helper);

                //helper = new HelperHome();
                //helper.Name = "Emily Clarkson";
                //helper.Service = "Childcare";
                //helper.Price = "From $100 per hour";
                //helper.Status = "Not Available";
                //helper.ProfilePicture = "picture.png";
                //helper.AverageRating = "4.4";
                //helper.AverageRatingCount = "4.4(" + "19)";
                //helper.ConnectionCount = 23;
                //helper.EnquiredUserCount = 97;
                //helper.Latitude = 28.4512804;
                //helper.Longitude = 77.0703797;
                //helper.LocationName = "Estel Technologies";
                //helper.UserId = 3;
                //helper.JobType = "Urgent";
                //helper.JobBgColor = "#F73C37";
                //helper.color = "#00FFFFFF";
                //JobFullList.Add(helper);
                //JobPostHalfList.Add(helper);

                //HelperHomeModel helpers = new HelperHomeModel();
                //helpers.LocationName = "Policy Bazaar";
                //helpers.Latitude = 28.4510871;
                //helpers.Longitude = 77.070111;                
                //helpers.Count = 3;
                //helpers.HelpersInLocalties = JobPostHalfList.ToList();
                //jobPostList = new ObservableCollection<HelperHomeModel>();
                //jobPostList.Add(helpers);
                //#endregion

                //#region Cluster 2
                //JobPostHalfList = new ObservableCollection<HelperHome>();
                //helper = new HelperHome();
                //helper.Name = "Army Rose";
                //helper.Service = "Childcare";
                //helper.Price = "From $100 per hour";
                //helper.Status = "Available";
                //helper.ProfilePicture = "picture2.png";
                //helper.AverageRating = "4.3";
                //helper.AverageRatingCount = "4.3(" + "32)";
                //helper.ConnectionCount = 37;
                //helper.EnquiredUserCount = 44;
                //helper.Latitude = 28.4608904;
                //helper.Longitude = 77.0700297;
                //helper.LocationName = "Oyster's Water";
                //helper.UserId = 4;
                //helper.JobType = "Urgent";
                //helper.JobBgColor = "#F73C37";
                //helper.color = "#00FFFFFF";
                //JobFullList.Add(helper);
                //JobPostHalfList.Add(helper);

                //helper = new HelperHome();
                //helper.Name = "HSBC";
                //helper.Service = "Bank";
                //helper.Price = "That is reliable";
                //helper.Status = "Sponsored";
                //helper.ProfilePicture = "message_picture3.png";
                //helper.AverageRating = "0.0";
                //helper.AverageRatingCount = "0.0(" + "0)";
                //helper.ConnectionCount = 0;
                //helper.EnquiredUserCount = 0;
                //helper.Latitude = 28.4615457;
                //helper.Longitude = 77.0690201;
                //helper.LocationName = "Appu Ghar";
                //helper.UserId = 5;
                //helper.JobType = "Sponsored";
                //helper.JobBgColor = "#FCDB54";
                //helper.color = "#00FFFFFF";
                //JobFullList.Add(helper);
                //JobPostHalfList.Add(helper);

                //helper = new HelperHome();
                //helper.Name = "Emily Clarkson";
                //helper.Service = "Childcare";
                //helper.Price = "From $100 per hour";
                //helper.Status = "Not Available";
                //helper.ProfilePicture = "picture.png";
                //helper.AverageRating = "4.4";
                //helper.AverageRatingCount = "4.4(" + "19)";
                //helper.ConnectionCount = 23;
                //helper.EnquiredUserCount = 97;
                //helper.Latitude = 28.4614138;
                //helper.Longitude = 77.0724419;
                //helper.LocationName = "Max Hospital";
                //helper.UserId = 6;
                //helper.JobType = "Urgent";
                //helper.JobBgColor = "#F73C37";
                //helper.color = "#00FFFFFF";
                //JobFullList.Add(helper);
                //JobPostHalfList.Add(helper);

                //helper = new HelperHome();
                //helper.Name = "Divya Clarkson";
                //helper.Service = "Banking";
                //helper.Price = "From $100 per hour";
                //helper.Status = "Not Available";
                //helper.ProfilePicture = "picture.png";
                //helper.AverageRating = "4.4";
                //helper.AverageRatingCount = "4.4(" + "19)";
                //helper.ConnectionCount = 23;
                //helper.EnquiredUserCount = 97;
                //helper.Latitude = 28.4566543;
                //helper.Longitude = 77.0705663;
                //helper.LocationName = "Fortis Hospital";
                //helper.UserId = 7;
                //helper.JobType = "Normal";
                //helper.JobBgColor = "#299DF4";
                //helper.color = "#00FFFFFF";
                //JobFullList.Add(helper);
                //JobPostHalfList.Add(helper);

                //helpers = new HelperHomeModel();
                //helpers.LocationName = "Huda City Center";
                //helpers.Latitude = 28.4591518;
                //helpers.Longitude = 77.070344;
                //helpers.Count = 4;
                //helpers.HelpersInLocalties = JobPostHalfList.ToList();

                //jobPostList.Add(helpers);
                //#endregion
                #endregion

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
