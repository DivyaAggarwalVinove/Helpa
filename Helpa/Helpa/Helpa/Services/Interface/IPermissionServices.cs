using System.Threading.Tasks;
using Xamarin.Forms;

namespace Helpa.Services
{
    public interface IPermissionServices
    {
        #region Requesting Runtime Location Permissions
        Task GetPermission(Page page);
        #endregion
    }
}