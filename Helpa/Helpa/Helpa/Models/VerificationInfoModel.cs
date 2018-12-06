using SQLite;
using System.ComponentModel;

namespace Helpa.Models
{
    public class Certificate
    {
        public string certificate { get; set; }
        public string id { get; set; }
    }

    public class Idcard
    {
        public string idcardurl { get; set; }
        public string id { get; set; }
    }

    public class VerificationInfoModel
    {
        public bool phonenumberconfirmed { get; set; }
        public bool emailconfirmed { get; set; }
        public string email { get; set; }
        public string PhoneNumber { get; set; }
        public bool Google { get; set; }
        public bool Facebook { get; set; }
        public Certificate certificate { get; set; }
        public Idcard idcard { get; set; }
    }
}