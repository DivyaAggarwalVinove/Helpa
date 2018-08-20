using Helpa.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Helpa.Services
{
    public interface IRegisterServices
    {
        Task RegisterExternal(RegisterUserModel helpersModel);
        Task RegisterService(RegisterUserModel helpersModel);
        Task CompleteRegisterService(RegisterUserModel helpersModel);

        Task FacebookSignUp(ExternalLoginViewModel externalDetail);
        Task<IEnumerable<ExternalLoginViewModel>> GetExternalDetails();
    }
}