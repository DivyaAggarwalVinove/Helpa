using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Helpa
{
    interface IHelpersServices<T>
    {
        Task<IEnumerable<T>> GetHelpersList(bool forceRefresh = false);
    }
}
