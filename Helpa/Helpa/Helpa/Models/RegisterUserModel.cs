using SQLite;

namespace Helpa.Models
{
    /// <summary>
    /// RegisterUserModel: Model for User(Helper/Parent) who wants to register.
    /// </summary>
    public class RegisterUserModel
    {
        [PrimaryKey]
        public int Id { get; set; }

        public int HelperId { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        
        public string Token { get; set; }
        
        public string LoginProvider { get; set; }

        public string UserName { get; set; }
        public string name { get; set; }

        public int Gender { get; set; }

        public string MobileNumber { get; set; }
        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public string profileImage { get; set; }

        public string locationName { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }

        public string description { get; set; }

        public bool IsRegistered { get; set; }
        public bool IsVerified { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsServiced { get; set; }
        public bool IsBuildTrusted { get; set; }
        public bool isLoggedIn { get; set; }
    }

    public class ExternalUserModel
    {
        public string id { get; set; }
        public string email { get; set; }
        public string idToken { get; set; }
        public string provider { get; set; }
        public string name { get; set; }
        public string profileImage { get; set; }
        public string PhoneNumber { get; set; }
        public string LoginProvider { get; set; }
        public string Role { get; set; }
    }

    public class ExternalLoginResponseModel
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public int expires_in { get; set; }
        public string userName { get; set; }
        public string id { get; set; }
        public string roles { get; set; }
        public string email { get; set; }
        public string PhoneVerification { get; set; }
        public string HelperId { get; set; }
        public string profileImage { get; set; }
    }

    enum Gender
    {
        Male = 1, Female = 2, Other = 3
    }
}
