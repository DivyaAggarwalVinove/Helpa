using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Helpa
{
    class HelpersViewModel : INotifyPropertyChanged
    {
        public IHelpersServices<HelpersModel> DataStore => DependencyService.Get<IHelpersServices<HelpersModel>>();
        public ObservableCollection<HelpersModel> helperList { get; set; }
        public Command LoadItemsCommand { get; set; }
        public Helpers context;

        public HelpersViewModel()
        {
            helperList = new ObservableCollection<HelpersModel>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
        }

        async Task ExecuteLoadItemsCommand()
        //private Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                helperList.Clear();

                // Call api to get data from server.
                var helpers = await DataStore.GetHelpersList(true);
                /*foreach (var item in helpers)
                {
                    //Parse list
                    //item.ImageUri = new UriImageSource { Uri = new Uri(item.ImageIcon) };
                    //item.color = "#FFFFFF";
                    //Items.Add(item);
                }*/

                context.CreateMap();
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
