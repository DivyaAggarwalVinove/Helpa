using Helpa.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
            try
            {
                database = new SQLiteAsyncConnection(dbPath);
                database.CreateTableAsync<RegisterUserModel>().Wait();
                database.CreateTableAsync<ServiceModel>().Wait();
                database.CreateTableAsync<ScopeModel>().Wait();
            }
            catch(Exception e)
            {
                Console.Write(e.StackTrace);
            }
        }
        #endregion

        #region Operations on Register user Table
        /// <summary>
        /// GetUsersAsync: Read all user.
        /// </summary>
        /// <returns></returns>
        public List<RegisterUserModel> GetUsersAsync()
        {
            List<RegisterUserModel> user = database.Table<RegisterUserModel>().Where(i => i.IsCompleted == true && i.IsRegistered==true).ToListAsync().Result;
            if(user.Count==0)
            {
                user = database.Table<RegisterUserModel>().Where(i => i.IsVerified == true && i.IsRegistered == true).ToListAsync().Result;
                if(user.Count==0) 
                {
                    user = database.Table<RegisterUserModel>().Where(i => i.IsRegistered == true).ToListAsync().Result;
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

        /// <summary>
        /// GetServicesAsync: To get the list of services from the database.
        /// </summary>
        /// <returns></returns>
        public List<ServiceModel> GetServicesAsync()
        {
            List<ServiceModel> services = database.Table<ServiceModel>().Where(i => i.isSelected == true).ToListAsync().Result;
            
            return services;
        }
        #endregion

        #region Operations on Scope Table
        /// <summary>
        /// SaveScopeAsync: Save scopes into database.
        /// </summary>
        /// <param name="scopes"></param>
        /// <returns></returns>
        public Task<int> SaveScopeAsync(IEnumerable<ScopeModel> scopes)
        {
            Task<int> result = null;
            foreach (ScopeModel scope in scopes)
            {
                ScopeModel s = database.Table<ScopeModel>().Where(i => i.ScopeId == scope.ScopeId).FirstOrDefaultAsync().Result;
                if (s != null)
                {
                    result = database.UpdateAsync(scope);
                }
                else
                {
                    result = database.InsertAsync(scope);
                }
            }

            return result;
        }

        /// <summary>
        /// SaveScopeAsync: Save scope into database.
        /// </summary>
        /// <param name="service"></param>
        /// <returns></returns>
        public Task<int> SaveScopeAsync(ScopeModel scope)
        {
            ScopeModel s = database.Table<ScopeModel>().Where(i => i.ScopeId == scope.ScopeId).FirstOrDefaultAsync().Result;
            if (s != null)
            {
                return database.UpdateAsync(scope);
            }
            else
            {
                return database.InsertAsync(scope);
            }
        }

        /// <summary>
        /// GetScopesAsync: To get the list of scopes from the database.
        /// </summary>
        /// <returns></returns>
        public List<ScopeModel> GetScopesAsync()
        {
            List<ScopeModel> scopes = database.Table<ScopeModel>().Where(i => i.isSelected == true).ToListAsync().Result;

            return scopes;
        }

        #endregion
    }
}
