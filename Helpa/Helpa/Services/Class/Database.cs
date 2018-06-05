using Helpa.Models;
using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Helpa.Services
{
    /// <summary>
    /// DatabaseServices: Services are used to perform operations on local database. 
    /// </summary>
    public class Database
    {
        #region Local Variables
        SQLiteAsyncConnection database;
        #endregion

        #region Constructor
        /// <summary>
        /// DatabaseServices: Constructor to create database at dbPath.
        /// </summary>
        /// <param name="dbPath"></param>
        public Database(string dbPath)
        {
            database = new SQLiteAsyncConnection(dbPath);
            database.CreateTableAsync<RegisterUserModel>().Wait();
            database.CreateTableAsync<ServiceModel>().Wait();
        }
        #endregion

        #region Operations on Register user Table

        /// <summary>
        /// GetUsersAsync: Read all user.
        /// </summary>
        /// <returns></returns>
        public List<RegisterUserModel> GetUsersAsync()
        {
            List<RegisterUserModel> user = database.Table<RegisterUserModel>().Where(i => i.IsCompleted == true && i.IsLogged==true).ToListAsync().Result;
            if(user.Count==0)
            {
                user = database.Table<RegisterUserModel>().Where(i => i.IsVerified == true && i.IsLogged == true).ToListAsync().Result;
                if(user.Count==0) 
                {
                    user = database.Table<RegisterUserModel>().Where(i => i.IsLogged == true).ToListAsync().Result;
                }
            }

            return user;
        }

        /// <summary>
        /// GetRegisterUsersAsync: Read  user filter by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<RegisterUserModel> GetUsersAsync(int id)
        {
            return database.Table<RegisterUserModel>().Where(i => i.Id == id).FirstOrDefaultAsync();
        }

        /// <summary>
        /// SaveUserAsync: Save user into database.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<int> SaveUserAsync(RegisterUserModel user)
        {
            RegisterUserModel registerUser = database.Table<RegisterUserModel>().Where(i => i.Id == user.Id).FirstOrDefaultAsync().Result;
            if (registerUser != null)
            {
                return database.UpdateAsync(user);
            }
            else
            {
                return database.InsertAsync(user);
            }
        }

        /// <summary>
        /// DeleteUserAsync: Delete user from database.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<int> DeleteUserAsync(RegisterUserModel user)
        {
            return database.DeleteAsync(user);
        }

        #endregion

        #region Operations on Service Table
        /// <summary>
        /// SaveServicesAsync: Save services into database.
        /// </summary>
        /// <param name="service"></param>
        /// <returns></returns>
        public Task<int> SaveServicesAsync(IEnumerable<ServiceModel> services)
        {
            Task<int> result = null;
            foreach (ServiceModel service in services)
            {
                ServiceModel s = database.Table<ServiceModel>().Where(i => i.Id == service.Id).FirstOrDefaultAsync().Result;
                if (s != null)
                {
                    result = database.UpdateAsync(service);
                }
                else
                {
                    result = database.InsertAsync(service);
                }
            }

            return result;
        }

        #region Operations on Service Table
        /// <summary>
        /// SaveServiceAsync: Save service into database.
        /// </summary>
        /// <param name="service"></param>
        /// <returns></returns>
        public Task<int> SaveServiceAsync(ServiceModel service)
        {
            ServiceModel s = database.Table<ServiceModel>().Where(i => i.Id == service.Id).FirstOrDefaultAsync().Result;
            if (s != null)
            {
                return database.UpdateAsync(service);
            }
            else
            {
                return database.InsertAsync(service);
            }
        }
        #endregion

        public List<ServiceModel> GetServicesAsync()
        {
            List<ServiceModel> services = database.Table<ServiceModel>().Where(i => i.isSelected == true).ToListAsync().Result;
            
            return services;
        }

        #endregion
    }
}
