using Helpa.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Helpa.Services
{
    interface IServices
    {
        Task<IEnumerable<ServiceModel>> GetServicesAsync();
    }
}
