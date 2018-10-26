using Helpa.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Helpa.Services
{
    public interface IJobServices
    {
        Task<IEnumerable<JobsHomeModel>> GetJobsList(int UserId);
        Task<IEnumerable<JobsHome>> GetJobsInLocation(double Latitude, double Longitude, int UserId);
        Task<JHomeModel> GetAllJobs(int UserId);
        Task<JHomeModel> GetMyJobs(int UserId);
        Task<JHomeModel> GetSavedJobs(int UserId);
        //Task<HelperServiceModel> SaveHelperServices(HelperServiceModel helperService);
    }
}
