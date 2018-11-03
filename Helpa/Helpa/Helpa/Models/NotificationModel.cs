using System;
using System.Collections.Generic;
using System.Text;

namespace Helpa.Models
{
    public class NotificationModel
    {
        public string SenderName { get; set; }

        public string DateTime { get; set; }

        public string DateLabel { get; set; }

        public string ReceiverName { get; set; }

        public string Title { get; set; }

        public string Message { get; set; }

        public string SenderImage { get; set; }

        public string NotificationStatus { get; set; }

        public bool IsNew { get; set; }

        public int SenderId { get; set; }

        public int NotificationId { get; set; }

        public int ReceiverId { get; set; }
    }
}