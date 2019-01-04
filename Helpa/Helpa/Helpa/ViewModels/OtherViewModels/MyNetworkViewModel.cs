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
    public class MyNetworkViewModel : INotifyPropertyChanged
    {
        ObservableCollection<MyNetworkModel> myNetworkList = new ObservableCollection<MyNetworkModel>();
        public ObservableCollection<MyNetworkModel> MyNetworkList
        {
            get
            {
                return myNetworkList;
            }
            set { SetProperty(ref myNetworkList, value); }
        }
        //ObservableCollection<HelperHome> myNetworkList = new ObservableCollection<HelperHome>();
        //public ObservableCollection<HelperHome> MyNetworkList
        //{
        //    get
        //    {
        //        return myNetworkList;
        //    }
        //    set { SetProperty(ref myNetworkList, value); }
        //}
        public Command LoadItemsCommand { get; set; }

        public MyNetworkViewModel()
        {
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
                RegisterUserModel user = App.Database.GetLoggedUser();
                HelpersServices helpersServices = new HelpersServices();
                var myNetwork = await helpersServices.GetMyNetworks(user.Id);
                var n = myNetwork.Data;
                for (int i = 0; i < n.Count(); i++)
                {
                    MyNetworkModel h = n.ElementAt(i);
                    // if (h.Service != null && h.Service.Count() != 0)
                    {
                        //HService hserv = h.Service.Where(x => x.ServiceName == "ChildCare").FirstOrDefault();
                        //if (hserv == null)
                        //{
                        //    hserv = h.Service.ElementAt(h.Service.Count() - 1);
                        //    h.ServiceName = hserv.ServiceName;
                        //    if (hserv.price.Daily)
                        //        h.ServicePriceLabel = "from $" + hserv.price.Min.Remove(hserv.price.Min.IndexOf(".")) + "-$" + hserv.price.Max.Remove(hserv.price.Max.IndexOf(".")) + "/Day";
                        //    else if (hserv.price.Monthly)
                        //        h.ServicePriceLabel = "from $" + hserv.price.Min.Remove(hserv.price.Min.IndexOf(".")) + "-$" + hserv.price.Max.Remove(hserv.price.Max.IndexOf(".")) + "/Month";
                        //    else if (hserv.price.Hours)
                        //        h.ServicePriceLabel = "from $" + hserv.price.Min.Remove(hserv.price.Min.IndexOf(".")) + "-$" + hserv.price.Max.Remove(hserv.price.Max.IndexOf(".")) + "/Hour";
                        //    if (hserv.Location != null && hserv.Location.Count() > 0)
                        //        h.ServiceLocationName = hserv.Location.ElementAt(0).LocationName;
                        //if (h.NoOfRatingUserCount == null)
                        //{
                        //    h.AverageRatingCount = "(0)";
                        //}
                        //else
                        //{
                        h.AverageRatingCount = h.Rating.ToString() + " (" + h.NoOfRatingUserCount.ToString() + ")";
                        //}
                        if (h.Status)
                        {
                            h.bgcolor = "#32BDA0";
                            h.textcolor = "#FFFFFF";
                            h.helperStatus = "Available";
                        }
                        else
                        {
                            h.bgcolor = "#EAE9E9";
                            h.textcolor = "#000000";
                            h.helperStatus = "Not Available";
                        }
                        n.Select(x => x.UserName == h.UserName ? h : x);
                    }
                }
                MyNetworkList = new ObservableCollection<MyNetworkModel>(n);
                //aiFindHelper.IsRunning = false;
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