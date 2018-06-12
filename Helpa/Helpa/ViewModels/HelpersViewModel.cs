using Helpa.Models;
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
    public class HelpersViewModel : INotifyPropertyChanged
    {
        public IHelpersServices<HelpersModel> DataStore => DependencyService.Get<IHelpersServices<HelpersModel>>();
        public ObservableCollection<Helper> HelperList { get; set; }
        public Command LoadItemsCommand { get; set; }
        public Helpers context;

        public HelpersViewModel(Helpers helpers)
        {
            HelperList = new ObservableCollection<Helper>();
            context = helpers;
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            //ExecuteLoadItemsCommand();
            LoadItemsCommand.Execute(null);
        }

        private async Task ExecuteLoadItemsCommand()
        //private void ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                HelperList.Clear();

                // Call api to get data from server.
                //var helpers = await DataStore.GetHelpersList(10000, 28.4514279, 77.0704678);
                var helpers = await (new HelpersServices()).GetHelpersList(10000, 28.4514279, 77.0704678);
                HelperList = new ObservableCollection<Helper>(helpers.First().HelpersInLocalties);

                //HelpersModel helper = new HelpersModel();
                //helper.userName = "Army Rose";
                //helper.service = "Childcare";
                //helper.HelpersInLocalties[0].price = "From $100 per hour";
                //helper.LocalityName = "Carble Garden";
                //helper.HelpersInLocalties[0].Status = "Available";
                //helper.HelpersInLocalties[0].ProfilePicture = "picture2.png";
                //helper.HelpersInLocalties[0].AverageRating = 4.3f;
                //helper.HelpersInLocalties[0].AverageRatingCount = "4.3(" + "32)";
                //helper.HelpersInLocalties[0].ConnectionCount = 37;
                //helper.HelpersInLocalties[0].EnquiredUserCount = 44;
                //helperList.Add(helper);

                //helper = new HelpersModel();
                //helper.userName = "HSBC";
                //helper.service = "Bank";
                //helper.price = "That is reliable";
                //helper.location = "Carble Garden";
                //helper.status = "Sponsored";
                //helper.profileImage = "message_picture3.png";
                //helper.rating = 0;
                //helper.rating_count = "0.0(" + "0)";
                //helper.count1 = 0;
                //helper.count2 = 0;
                //helperList.Add(helper);

                //helper = new HelpersModel();
                //helper.userName = "Emily Clarkson";
                //helper.service = "Childcare";
                //helper.price = "From $100 per hour";
                //helper.location = "Carble Garden";
                //helper.status = "Not Available";
                //helper.profileImage = "picture.png";
                //helper.rating = 4.4f;
                //helper.rating_count = "4.4(" +"19)";
                //helper.count1 = 23;
                //helper.count2 = 97;
                //helperList.Add(helper);

                //helper = new HelpersModel();
                //helper.userName = "Army Rose";
                //helper.service = "Childcare";
                //helper.price = "From $100 per hour";
                //helper.location = "Carble Garden";
                //helper.status = "Available";
                //helper.profileImage = "picture2.png";
                //helper.rating = 4.3f;
                //helper.rating_count = "4.3(" + "32)";
                //helper.count1 = 37;
                //helper.count2 = 44;
                //helperList.Add(helper);

                //helper = new HelpersModel();
                //helper.userName = "HSBC";
                //helper.service = "Bank";
                //helper.price = "That is reliable";
                //helper.location = "Carble Garden";
                //helper.status = "Sponsored";
                //helper.profileImage = "message_picture3.png";
                //helper.rating = 0;
                //helper.rating_count = "0.0(" + "0)";
                //helper.count1 = 0;
                //helper.count2 = 0;
                //helperList.Add(helper);

                //helper = new HelpersModel();
                //helper.userName = "Emily Clarkson";
                //helper.service = "Childcare";
                //helper.price = "From $100 per hour";
                //helper.location = "Carble Garden";
                //helper.status = "Not Available";
                //helper.profileImage = "picture.png";
                //helper.rating = 4.4f;
                //helper.rating_count = "4.4(" + "19)";
                //helper.count1 = 23;
                //helper.count2 = 97;
                //helperList.Add(helper);
                /*foreach (var item in helpers)
                {
                    //Parse list
                    //item.ImageUri = new UriImageSource { Uri = new Uri(item.ImageIcon) };
                    //item.color = "#FFFFFF";
                    //Items.Add(item);
                }*/

                //return null;
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
