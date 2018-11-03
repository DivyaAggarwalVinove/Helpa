using Helpa.Models;
using Helpa.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Helpa.ViewModels
{
    class NotificationsViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<NotificationModel> notificationList = new ObservableCollection<NotificationModel>();

        public ObservableCollection<NotificationModel> NotificationList
        {
            get
            {
                return notificationList;
            }
            set
            {
                SetProperty(ref notificationList, value);
            }
        }

        public Command LoadItemsCommand { get; set; }
        public ActivityIndicator aiNotification;

        public NotificationsViewModel(Notifications notifications)
        {
            aiNotification = notifications.FindByName<ActivityIndicator>("aiNotification");

            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            LoadItemsCommand.Execute(null);
        }

        public async Task ExecuteLoadItemsCommand()
        {
            if (this.IsBusy)
                return;
            this.IsBusy = true;
            try
            {
                aiNotification.IsRunning = true;

                NotificationResponseModel notification = await new Utilities().GetNotification(App.Database.GetLoggedUser().Id);
                if (notification.Data != null)
                {
                    foreach (NotificationModel notificationModel in (Collection<NotificationModel>)notification.Data)
                    {
                        DateTime dateTime = Convert.ToDateTime(notificationModel.DateTime, (IFormatProvider)new CultureInfo("en-US"));
                        notificationModel.DateLabel = dateTime.ToString("MMM dd") + " at " + dateTime.ToString("HH:mm");
                    }
                    NotificationList = notification.Data;
                }
                else
                    await App.Current.MainPage.DisplayAlert("", notification.Message, "Ok");

                aiNotification.IsRunning = false;
            }
            catch (Exception ex)
            {
                Console.Write(ex.StackTrace);
                aiNotification.IsRunning = false;
            }
            finally
            {
                this.IsBusy = false;
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
