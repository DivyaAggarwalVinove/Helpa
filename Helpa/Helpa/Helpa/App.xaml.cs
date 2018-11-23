using System;
using System.Diagnostics;
using Xamarin.Forms;
using System.Linq;
using System.Collections.Generic;
using Helpa.Services;
using Helpa.Views.Profile;

namespace Helpa
{
    public partial class App : Application
    {
        /// <summary>
        /// Create database
        /// </summary>

        #region Database
        static Database database;
        public static Database Database
        {
            get
            {
                if (database == null)
                {
                    database = new Database(DependencyService.Get<IDatabaseServices>().GetLocalFilePath("helpa.db3"));
                }
                return database;
            }
        }
        #endregion

        static App _app;
        public static App app
        {
            get
            {
                
                return app;
            }
        }

        public static int selectedPage = 0;
        List<KeyValuePair<string, string>> listTabs;
        List<string> imagesName;

       public static NavigationPage NavigationPage { get; set; }

        public App()
        {
            try
            {
                //Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MTQwMTVAMzEzNjJlMzIyZTMwYzJHZDNOMUtsV1pPbVdKYU5taEp4T3BXMVZjc0lKTVBWV214dWMrT2VyMD0=");

                InitializeComponent();

                DependencyService.Register<IPermissionServices>();

                listTabs = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("imgHelpers", "lblHelpers"),
                    new KeyValuePair<string, string>("imgPosts", "lblPosts"),
                    new KeyValuePair<string, string>("imgMessages", "lblMessages"),
                    new KeyValuePair<string, string>("imgNotifications", "lblNotifications"),
                    new KeyValuePair<string, string>("imgProfile", "lblProfile"),
                };

                imagesName = new List<string>()
                {
                    "find_helpers" , "job_posts", "messages","notifications", "profile"
                };

                _app = this;

                NavigationPage = new NavigationPage(new Helpers());
                MainPage = NavigationPage;
            }
            catch (Exception e)
            {
                Debug.Write(e.Message);
            }
        }

        public ContentPresenter contentPresenter;
        void OnFindHelperPressed(object sender, EventArgs args)
        {
            if (selectedPage != 0)
            {
                Grid grid = ((Grid)sender);
                SelectTab(grid, selectedPage, 0);

                contentPresenter = grid.FindByName<ContentPresenter>("content");
                // contentPresenter.Content = (new Helpers()).Content;
                // Helpers.Instance.helpersViewModel.TotalHelpers = Helpers.Instance.helpersViewModel.TotalHelpers;
                var lbl = Helpers.Instance.FindByName<Label>("lblCount");
                lbl.Text = Helpers.Instance.helpersViewModel.TotalHelpers;
                contentPresenter.Content = Helpers.Instance.Content;
            }
        }
        
        void OnJobPostPressed(object sender, EventArgs args)
        {
            if (selectedPage != 1)
            {
                Grid grid = ((Grid)sender);
                SelectTab(grid, selectedPage, 1);

                contentPresenter = grid.FindByName<ContentPresenter>("content");
                //contentPresenter.Content = (new JobPosts()).Content;

                if (JobPosts.Instance != null)
                {
                    var lbl = JobPosts.Instance.FindByName<Label>("lblTotalJobsCount");
                    lbl.Text = JobPosts.Instance.jobsViewModel.TotallJobs;

                    contentPresenter.Content = JobPosts.Instance.Content;
                }
                else
                    contentPresenter.Content = (new JobPosts()).Content;
            }
        }

        void OnMessagesPressed(object sender, EventArgs args)
        {
            if (selectedPage != 2)
            {
                Grid grid = ((Grid)sender);
                SelectTab(grid, selectedPage, 2);

                contentPresenter = grid.FindByName<ContentPresenter>("content");
                contentPresenter.Content = (new Messages()).Content;
            }
        }

        void OnNotificationsPressed(object sender, EventArgs args)
        {
            if (selectedPage != 3)
            {
                Grid grid = ((Grid)sender);
                SelectTab(grid, selectedPage, 3);

                contentPresenter = grid.FindByName<ContentPresenter>("content");
                contentPresenter.Content = (new Notifications()).Content;
            }
        }

        //public void OnProfilePressed(object sender, EventArgs args)
        //{
        //    if (selectedPage != 4)
        //    {
        //        Grid grid = ((Grid)sender);
        //        SelectTab(grid, selectedPage, 4);

        //        contentPresenter = grid.FindByName<ContentPresenter>("content");

        //        var loggedUser = Database.GetLoggedUser();
        //        if (loggedUser == null)
        //            contentPresenter.Content = (new ProfileBeforeLoginPage()).Content;
        //        else
        //        {
        //            ProfileAfterLoginPage profileAfterLogin = new ProfileAfterLoginPage();
        //            profileAfterLogin.currentUser = loggedUser;
        //            contentPresenter.Content = (profileAfterLogin).Content;
        //        }
        //    }
        //}

        public void OnProfilePressed(object sender, EventArgs args)
        {
            if (App.selectedPage == 4)
                return;

            Grid grid = (Grid)sender;
            SelectTab(grid, App.selectedPage, 4);

            contentPresenter = grid.FindByName<ContentPresenter>("content");
           // contentPresenter = NameScopeExtensions.FindByName<ContentPresenter>(grid, "content");
            contentPresenter.Content = new ProfilePage().Content;
        }

        void SelectTab(Grid grid, int currentIndex, int newIndex)
        {
            // Select new tab
            var imgPosts = grid.FindByName<Image>(listTabs.ElementAt(newIndex).Key);
            Label lblPosts = grid.FindByName<Label>(listTabs.ElementAt(newIndex).Value);

            imgPosts.Source = ImageSource.FromFile(imagesName.ElementAt(newIndex) + "_pink.png");
            lblPosts.TextColor = Color.FromHex("#FF748C");

            // Unselect seleceted tab
            var selectedImage = grid.Parent.FindByName<Image>(listTabs.ElementAt(currentIndex).Key);
            var selectedLabel = grid.Parent.FindByName<Label>(listTabs.ElementAt(currentIndex).Value);

            selectedImage.Source = ImageSource.FromFile(imagesName.ElementAt(currentIndex) + ".png");
            selectedLabel.TextColor = Color.FromHex("#767b7e");

            selectedPage = newIndex;
        }

        public static Action<string> PostSuccessFacebookAction { get; set; }

        protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
