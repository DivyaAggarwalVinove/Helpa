using Helpa.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Helpa.Services
{
    public interface IRegisterServices
    {
        #region Custom Signup
        Task RegisterService(RegisterUserModel helpersModel);
        Task CompleteRegisterService(RegisterUserModel helpersModel);
        #endregion

        #region External Sign in
        Task RegisterExternal(ExternalUserModel helpersModel);
        #endregion

        #region Other Utlity Register Api
        Task SendSmsCode(int userId, string mobileno, string role);
        Task VerifyOtp(int userId, string otp, string role);
        Task<ResponseModel> SendLink(string email);
        Task EmailVerify(string Email);
        #endregion
    }
}