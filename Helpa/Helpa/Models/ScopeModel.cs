using SQLite;

namespace Helpa.Models
{
    public class ScopeModel
    {
        [PrimaryKey]
        public int ScopeId { get; set; }
        public string ScopeName { get; set; }
        public bool isSelected { get; set; }
    }
}