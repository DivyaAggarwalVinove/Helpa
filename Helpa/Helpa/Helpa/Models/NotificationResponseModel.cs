using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Helpa.Models
{
    public class NotificationResponseModel
    {
        public string Code { get; set; }

        public string Message { get; set; }

        public ObservableCollection<NotificationModel> Data { get; set; }

        public string NewCount { get; set; }
    }
}