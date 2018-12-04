using Helpa.Models;
using Helpa.Services;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace Helpa.ViewModels.ProfileViewModel.EditProfileViewModel
{
    public class EditBasicInfoViewModel : INotifyPropertyChanged
    {
        UserModel userInfo = new UserModel();
        public UserModel UserInfo
        {
            get
            {
                return userInfo;
            }
            set
            {
                SetProperty(ref userInfo, value);
            }
        }

        string selectedCarousel = "0/0";
        public string SelectedCarousel {
            get {return selectedCarousel; }
            set { SetProperty(ref selectedCarousel, value); } }

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

        public EditBasicInfoViewModel()
        {
            var LoggedinUser = App.Database.GetLoggedUser();
            if (LoggedinUser != null)
                GetUserBasicInfo(LoggedinUser.Id);
        }


        async void GetUserBasicInfo(int userid)
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
            }
            else
            {
                UserInfo = await (new LoginServices()).GetUserBasicInfo(userid);

                UserInfo.UserId = userid;

                SelectedCarousel = "0/" + ((UserInfo.Carousel != null) ? UserInfo.Carousel.Count.ToString() : "0");
                UserInfo.GenderName = (UserInfo.Gender == 1) ? "Male" : (UserInfo.Gender == 2) ? "Female" : "Rather no to say";
            }
        }
    }
}
