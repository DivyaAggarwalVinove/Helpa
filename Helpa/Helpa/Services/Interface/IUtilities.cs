using Helpa.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Helpa.Services
{
    interface IUtilities
    {
        Task<IEnumerable<ScopeModel>> GetScpoesAsync();
        Task<IEnumerable<ServiceModel>> GetServicesAsync();
    }
}
