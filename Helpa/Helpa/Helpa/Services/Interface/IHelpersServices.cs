using Helpa.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Helpa.Services
{
    public interface IHelpersServices
    {
        Task<IEnumerable<HelperHomeModel>> GetHelpersList(int UserId, int ServiceId=0);
        Task<IEnumerable<HelperHome>> GetHelpersInLocation(double Latitude, double Longitude, int UserId);
        Task<HHomeModel> GetAllHelpers(int UserId);
        Task<NetworkModel> GetMyNetworks(int UserId);
        Task<NetworkModel> GetSavedUsers(int UserId);
        Task<HelperServiceModel> SaveHelperServices(HelperServiceModel helperService);
        Task<bool> BookMarkHelper(int userId, int helperId);
        Task<bool> UnBookMarkHelper(int userId, int helperId);
    }
}
