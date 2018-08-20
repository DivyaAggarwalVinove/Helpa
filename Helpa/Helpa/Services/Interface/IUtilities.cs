using Helpa.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Helpa.Services
{
    public interface IUtilities
    {
        Task<List<ScopeModel>> GetScpoesAsync(int serviceId);
        Task<IEnumerable<ServiceModel>> GetServicesAsync();
    }
}
