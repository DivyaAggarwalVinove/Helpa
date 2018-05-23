using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Helpa.Services
{
    public interface IParentRegisterServices
    {
        Task RegisterExternal(HelpersModel helpersModel);
    }
}
