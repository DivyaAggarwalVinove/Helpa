using Helpa.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Helpa.Services
{
    public interface ILoginServices
    {
        Task<LoginErrorResponseModel> Login(string email, string pwd);
        Task<bool> ExternalLogin(ExternalUserModel userModel);

        #region User Basic Info Services 
        Task<UserModel> GetUserBasicInfo(int UserId);
        Task<bool> SaveUserBasicInfo(UserModel userInfo);
        #endregion

        #region User Verification Service
        Task<VerificationInfoModel> GetVerificationInfo(int UserId);
        Task<bool> UploadCertificate(int hid, string cert);
        Task<bool> UploadID(int hid, string cert);
        #endregion
    }
}
