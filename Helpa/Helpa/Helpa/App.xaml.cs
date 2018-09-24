using System;
using System.Diagnostics;
using Xamarin.Forms;
using System.Linq;
using System.Collections.Generic;
using Helpa.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Helpa.Views.Profile.UserProfile;
using Helpa.Views.Profile.ProfileSettings;

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
        
        int selectedPage = 0;
        List<KeyValuePair<string, string>> listTabs;
        List<string> imagesName;
       public static NavigationPage NavigationPage { get; set; }
        public App()
        {
            try
            {
                Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MTQwMTVAMzEzNjJlMzIyZTMwYzJHZDNOMUtsV1pPbVdKYU5taEp4T3BXMVZjc0lKTVBWV214dWMrT2VyMD0=");

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
               // NavigationPage = new NavigationPage(new ProfileAfterLoginPage());              
                NavigationPage= new NavigationPage(new Helpers());
                MainPage = NavigationPage;
                JobPosts jobPosts = new JobPosts();
            }
            catch (Exception e)
            {
                Debug.Write(e.Message);
            }
        }

        async void OnFindHelperPressed(object sender, EventArgs args)
        {
            if (selectedPage != 0)
            {
                Grid grid = ((Grid)sender);
                SelectTab(grid, selectedPage, 0);

                //MainPage = new NavigationPage(Helpers.Instance);
                //MainPage = Helpers.Instance;
                ContentPresenter contentPresenter = grid.FindByName<ContentPresenter>("content");
                //Helpers helper = new Helpers();
                contentPresenter.Content = (new Helpers()).Content;

                await Helpers.Instance.GetRuntimeLocationPermission(5000);
            }
        }
        
        async void OnJobPostPressed(object sender, EventArgs args)
        {
            if (selectedPage != 1)
            {
                //MainPage = new NavigationPage(JobPosts.Instance);

                Grid grid = ((Grid)sender);
                SelectTab(grid, selectedPage, 1);

                ContentPresenter contentPresenter = grid.FindByName<ContentPresenter>("content");
                contentPresenter.Content = (new JobPosts()).Content;

                await JobPosts.Instance.GetRuntimeLocationPermission(5000);
            }
        }

        void OnMessagesPressed(object sender, EventArgs args)
        {
            if (selectedPage != 2)
            {
                Grid grid = ((Grid)sender);
                SelectTab(grid, selectedPage, 2);

                ContentPresenter contentPresenter = grid.FindByName<ContentPresenter>("content");
                contentPresenter.Content = (new Messages()).Content;
            }
        }

        void OnNotificationsPressed(object sender, EventArgs args)
        {
            if (selectedPage != 3)
            {
                Grid grid = ((Grid)sender);
                SelectTab(grid, selectedPage, 3);

                ContentPresenter contentPresenter = grid.FindByName<ContentPresenter>("content");
                contentPresenter.Content = (new Notifications()).Content;
            }
        }

        void OnProfilePressed(object sender, EventArgs args)
        {
            if (selectedPage != 4)
            {
                Grid grid = ((Grid)sender);
                SelectTab(grid, selectedPage, 4);

                ContentPresenter contentPresenter = grid.FindByName<ContentPresenter>("content");
                contentPresenter.Content = (new UserSettingsPage()).Content;
            }
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
