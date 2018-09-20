using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Helpa.ViewModels
{
    class RegisterViewModel1 : INotifyPropertyChanged
    {
        public RegisterViewModel1()
        {
            myList = new Dictionary<int, string>();
            selectedIndex = -1;
            LoadData();
        }

        private void LoadData()
        {
            for (int i = 0; i < 5; i++)
            {
                MyList.Add(i, "Item " + i);
            }
        }

        private Dictionary<int, string> myList;
        public Dictionary<int, string> MyList
        {
            get
            {
                var v = MyList.Values;
                return myList;
            }
            set
            {
                myList = value;
                OnPropertyChanged("MyList");
            }
        }

        private int selectedIndex;
        public int SelectedIndex
        {
            get { return selectedIndex; }
            set
            {
                if (value == selectedIndex) return;
                selectedIndex = value;
                OnPropertyChanged("SelectedIndex");
            }
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
