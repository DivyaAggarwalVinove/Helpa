using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Helpa.Models;

namespace Helpa.ViewModels
{
    public class ProfileAfterLoginViewModel : INotifyPropertyChanged
    {
        //public RegisterUserModel currentUser { get; set; }

        RegisterUserModel currentUser = new RegisterUserModel();
        public RegisterUserModel CurrentUser
        {
            get
            {
                return currentUser;
            }
            set { SetProperty(ref currentUser, value); }
        }

        public ProfileAfterLoginViewModel(RegisterUserModel user)
        {
            CurrentUser = user;
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
