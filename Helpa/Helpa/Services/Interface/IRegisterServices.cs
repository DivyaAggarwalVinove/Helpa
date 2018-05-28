using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Helpa.Services
{
    public interface IRegisterServices
    {
        Task RegisterExternal(HelpersModel helpersModel);
        Task RegisterService(HelpersModel helpersModel);
    }
}
