using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Helpa.Services
{
    public interface ITrust
    {
         Task<bool> UploadTrust(object trust);
    }
}
