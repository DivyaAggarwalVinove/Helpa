using SQLite;
using System.ComponentModel;

namespace Helpa.Models
{
    public class ScopeModel
    {
        [PrimaryKey]
        [DisplayName("Id")]
        public int ScopeId { get; set; }
        public int ServiceId { get; set; }
        public string ScopeName { get; set; }
    }
}