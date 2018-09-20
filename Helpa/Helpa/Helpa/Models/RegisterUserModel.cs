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
        public string Password { get; set; }
        public string Role { get; set; }
        public int Gender { get; set; }
        public string Token { get; set; }
        public string UserName { get; set; }
        public string LoginProvider { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public bool IsRegistered { get; set; }
        public bool IsVerified { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsServiced { get; set; }
        public bool IsBuildTrusted { get; set; }
        public bool isLoggedIn { get; set; }
    }

    enum Gender
    {
        Male = 1, Female = 2, Other = 3
    }
}
