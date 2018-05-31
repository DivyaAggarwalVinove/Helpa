using SQLite;

namespace Helpa.Models
{
    /// <summary>
    /// RegisterUserModel: Model for User(Helpa/Parent) who wants to register.
    /// </summary>
    public class RegisterUserModel
    {
        [PrimaryKey]
        public int Id { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public string Gender { get; set; }
        public string Token { get; set; }
        public string UserName { get; set; }
        public string LoginProvider { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsLogged { get; set; }
        public bool IsVerified { get; set; }
        public bool IsCompleted { get; set; }
    }
}
