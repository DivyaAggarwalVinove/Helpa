using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Helpa.Services.ServiceEntity
{
    public class NotifyPropertyChanged : INotifyPropertyChanged
    {
        /// <summary>
        /// Properties changed event handler 
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// Calling this method on propertyChanged from source to target or target ot source
        /// </summary>
        /// <param name="propertiesName">Propertiesname.</param>
        public void OnPropertyChanged([CallerMemberName] string propertiesName = "")
        {
            var handler = PropertyChanged;
            if (handler == null)
                return;
            handler(this, new PropertyChangedEventArgs(propertiesName));
        }
    }
}
