using Helpa.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Helpa.Services
{
    public interface IHelpersServices<T>
    {
        Task<IEnumerable<T>> GetHelpersList(int radius, double latitude, double longitude);
        Task<HelperServiceModel> SaveHelperServices(HelperServiceModel helperService);
    }
}
