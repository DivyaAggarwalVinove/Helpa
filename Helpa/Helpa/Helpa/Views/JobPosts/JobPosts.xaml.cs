using Helpa.Models;
using Helpa.Services;
using Helpa.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace Helpa
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class JobPosts : ContentPage
	{
        JobPostViewModel jobsViewModel;

        #region JobInstance
        static JobPosts instance;
        public static JobPosts Instance
        {
            get
            {
                return instance;
            }
        }
        #endregion

        public JobPosts ()
		{
			InitializeComponent ();

            instance = this;
            NavigationPage.SetHasNavigationBar(this, false);

            jobsViewModel = new JobPostViewModel(this);
            jobsViewModel.mapJob = mapJob;
            BindingContext = jobsViewModel;

            MessagingCenter.Subscribe<CustomMap, string>(this, "Hi", (sender, selectedCluster) =>
            {
                // if (rlHalfView.IsVisible == false)
                try
                {
                    aiJobPost.IsRunning = true;
                    ShowHelperHalfList(selectedCluster);
                    aiJobPost.IsRunning = false;
                    /*rlHalfJobView.IsVisible = true;

                    var selectedHelpersInCluster = jobsViewModel.jobPostList.Where(h => h.LocationName == (selectedCluster)).FirstOrDefault();
                    
                    lvHalfJobs.ItemsSource = jobsViewModel.JobPostHalfList;
                    lblJobsCount.Text = selectedHelpersInCluster.Count + " Helpers found in " + selectedHelpersInCluster.LocationName;
                    */
                }
                catch(Exception e)
                {
                    Console.Write(e.StackTrace);
                }
            });
        }

        //protected override async void OnAppearing()
        //{
        //    base.OnAppearing();

        //    await GetRuntimeLocationPermission(5000);
        //}


        async void ShowHelperHalfList(string selectedCluster)
        {
            try
            {
                rlHalfJobView.IsVisible = true;

                var selectedHelpersInCluster = jobsViewModel.jobPostList.Where(h => h.LocationName == (selectedCluster)).FirstOrDefault();
                mapJob.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(selectedHelpersInCluster.Latitude - 0.0055, selectedHelpersInCluster.Longitude), Distance.FromKilometers(1)));

                JobServices jobServices = new JobServices();
                var js = await jobServices.GetJobsInLocation(selectedHelpersInCluster.Latitude, selectedHelpersInCluster.Longitude, 0);

                for (int i = 0; i < js.Count(); i++)
                {
                    JobsHome j = js.ElementAt(i);
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

                    js.Select(x => x.JobId == j.JobId ? j : x);
                }

                jobsViewModel.JobPostHalfList = new ObservableCollection<JobsHome>(js);
                lvHalfJobs.ItemsSource = jobsViewModel.JobPostHalfList;
                lblJobsCount.Text = js.Count() + " Jobs found in " + selectedHelpersInCluster.LocationName;
            }
            catch (Exception e)
            {
                Console.Write(e.StackTrace);
            }
        }

        async void ShowJobFullList()
        {
            try
            {
                aiJobPost.IsRunning = true;

                JobServices jobServices = new JobServices();
                var js = await jobServices.GetAllJobs(0);

                lblJobFullCount.Text = js.Total + lblJobFullCount.Text;

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

                    js.Data.Select(x => x.JobId == j.JobId ? j : x);
                }

                jobsViewModel.JobFullList = new ObservableCollection<JobsHome>(js.Data);
                lvFullJobPost.ItemsSource = jobsViewModel.JobFullList;
                //lblJobsCount.Text = js.Count() + " Jobs found in " + selectedHelpersInCluster.LocationName;
            
                aiJobPost.IsRunning = false;
            }
            catch (Exception e)
            {
                Console.Write(e.StackTrace);
            }
        }

        public async Task GetRuntimeLocationPermission(int milisec)
        {
            await Task.Delay(milisec);

            await DependencyService.Get<IPermissionServices>().GetPermission(this);

            jobsViewModel.LoadItemsCommand = new Command(async () => await jobsViewModel.ExecuteLoadItemsCommand());
            jobsViewModel.LoadItemsCommand.Execute(null);
        }

        void OnCloseHalfJobList(object sender, EventArgs args)
        {
            rlHalfJobView.IsVisible = false;
        }

        void JobSelectedFullList(object sender, ItemTappedEventArgs args)
        {
            var selectedItem = (HelperHome)args.Item;
            int selectedIndex = jobsViewModel.JobFullList.IndexOf(jobsViewModel.JobFullList.Where(h => h.UserId == selectedItem.UserId).FirstOrDefault());
            //jobsViewModel.JobFullList.ElementAt(selectedIndex).color = "#FADC54";

            //slOuterFull.
            //lvFullHelpa.GetChildAt(args.)
        }

        async void OnClickBecomeHelper(object sender, EventArgs args)
        {
            var user = App.Database.GetLoggedUser();
            if (user == null)
                await Application.Current.MainPage.Navigation.PushAsync ( new LoginPage());
                
                //await Navigation.PushAsync(new LoginPage());
            else
            {
                // Become a helper
            }
            //var user = App.Database.GetUsersAsync();
            //if (user == null || user.Count == 0)
            //{
            //    //NavigationPage np = new NavigationPage(new HelperRegister());
            //    //Application.Current.MainPage = np;
            //    Navigation.PushModalAsync(new HelperRegister());
            //}
            //else
            //{
            //    if (user[0].IsBuildTrusted)
            //    {
            //        //post a job
            //    }
            //    else
            //    {
            //        Navigation.PushAsync(new HelperCompleteRegister(user[0]));
            //    }
            //}
        }

        void OnShowJobsList(object sender, EventArgs args)
        {
            if (mapJob.IsVisible)
            {
                slFullJobPost.IsVisible = true;
                mapJob.IsVisible = false;
                lblTotalJobsCount.IsVisible = false;

                ShowJobFullList();

                imgJobsList.Source = "location_filter.png";
            }
            else
            {
                lvFullJobPost.IsVisible = false;
                mapJob.IsVisible = true;
                lblTotalJobsCount.IsVisible = true;

                imgJobsList.Source = "filter.png";
            }
        }
    }
}