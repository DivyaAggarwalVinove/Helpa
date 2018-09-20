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
                //lvHalfHelpa.ScrollTo(helpersViewModel.helperList, ScrollToPosition., false);
                //lvHalfHelpa.SelectedItem

                // if (rlHalfView.IsVisible == false)
                {
                    rlHalfJobView.IsVisible = true;

                    var selectedHelpersInCluster = jobsViewModel.jobPostList.Where(h => h.LocalityName == (selectedCluster)).FirstOrDefault();
                    jobsViewModel.JobPostHalfList = new ObservableCollection<HelperHome>(selectedHelpersInCluster.HelpersInLocalties);
                    lvHalfJobs.ItemsSource = jobsViewModel.JobPostHalfList;
                    lblJobsCount.Text = selectedHelpersInCluster.NumberOfHelpersInLocality + " Jobs found in " + selectedHelpersInCluster.LocalityName;
                }
                // else
                // rlHalfView.IsVisible = false;
            });
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await GetRuntimeLocationPermission(5000);
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
            jobsViewModel.JobFullList.ElementAt(selectedIndex).color = "#FADC54";
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
                lvFullJobPost.IsVisible = true;
                mapJob.IsVisible = false;
                lblTotalJobsCount.IsVisible = false;

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