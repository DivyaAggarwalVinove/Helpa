using System;
using System.Diagnostics;
using Xamarin.Forms;
using System.Linq;
using System.Collections.Generic;
using Helpa.Services;

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
        public App()
        {
            try
            {
                InitializeComponent();
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

                MainPage = new NavigationPage(new Helpers());
            }
            catch (Exception e)
            {
                Debug.Write(e.Message);
            }
        }

        void OnFindHelperPressed(object sender, EventArgs args)
        {
            if (selectedPage != 0)
            {
                Grid grid = ((Grid)sender);
                SelectTab(grid, selectedPage, 0);

                ContentPresenter contentPresenter = grid.FindByName<ContentPresenter>("content");
                contentPresenter.Content = (new Helpers()).Content;
            }
        }

        void OnJobPostPressed(object sender, EventArgs args)
        {
            if (selectedPage != 1)
            {
                //Current.MainPage = new JobPosts();
                Grid grid = ((Grid)sender);
                SelectTab(grid, selectedPage, 1);
                
                ContentPresenter contentPresenter = grid.FindByName<ContentPresenter>("content");
                contentPresenter.Content = (new JobPosts()).Content;
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
                contentPresenter.Content = (new Profile()).Content;
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
