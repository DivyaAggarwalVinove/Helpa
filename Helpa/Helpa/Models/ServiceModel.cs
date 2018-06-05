using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Helpa.Models
{
    public class ServiceModel
    {
        [PrimaryKey]
        public string Id { get; set; }
        public string ServiceName { get; set; }
        public bool isSelected { get; set; }
    }
}
