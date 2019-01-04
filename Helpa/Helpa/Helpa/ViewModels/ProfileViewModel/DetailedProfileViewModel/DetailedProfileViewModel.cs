using Helpa.Models;
using Helpa.Services;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace Helpa.ViewModels.ProfileViewModel.DetailedProfileViewModel
{
    class DetailedProfileViewModel : INotifyPropertyChanged
    {
        public DetailedProfileViewModel(RegisterUserModel LoggedinUser)
        {
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
                SelectedCarousel = ((UserInfo.Carousel != null) ? "1/" + UserInfo.Carousel.Count.ToString() : "0/0");

                verificationInfo = await (new LoginServices()).GetVerificationInfo(userid);
                HelperServices = await (new HelpersServices()).GetHelperServices(userid);
                HelperServices.ServiceCount = HelperServices.Service.Count;
            }
        }

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

        HelperServiceModel helperServices = new HelperServiceModel();
        public HelperServiceModel HelperServices
        {
            get
            {
                return helperServices;
            }

            set
            {
                SetProperty(ref helperServices, value);
            }
        }

        VerificationInfoModel verificationInfo = new VerificationInfoModel();
        public VerificationInfoModel VerificationInfo
        {
            get
            {
                return verificationInfo;
            }

            set
            {
                SetProperty(ref verificationInfo, value);
            }
        }

        string selectedCarousel = "0/0";
        public string SelectedCarousel
        {
            get { return selectedCarousel; }
            set { SetProperty(ref selectedCarousel, value); }
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
