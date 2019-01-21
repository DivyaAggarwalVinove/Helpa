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
        SQLiteConnection database;
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
                database = new SQLiteConnection(dbPath);
                database.CreateTable<RegisterUserModel>();
                database.CreateTable<ServiceModel>();
                database.CreateTable<ScopeModel>();
            }
            catch(Exception e)
            {
                Console.Write(e.StackTrace);
            }
        }
        #endregion

        #region Operations on Register user Table
        /// <summary>
        /// GetUsers: Read all user.
        /// </summary>
        /// <returns></returns>
        public List<RegisterUserModel> GetUsers()
        {
            List<RegisterUserModel> user = database.Table<RegisterUserModel>().Where(i => i.IsCompleted == true && i.IsRegistered==true).ToList();
            if(user.Count==0)
            {
                user = database.Table<RegisterUserModel>().Where(i => i.IsVerified == true && i.IsRegistered == true).ToList();
                if(user.Count==0) 
                {
                    user = database.Table<RegisterUserModel>().Where(i => i.IsRegistered == true).ToList();
                }
            }

            return user;
        }

        public RegisterUserModel GetRegisteredUser()
        {
            return database.Table<RegisterUserModel>().Where(i => i.IsCompleted == true).FirstOrDefault();
        }

        public RegisterUserModel GetLoggedUser()
        {
            var user = database.Table<RegisterUserModel>().Where(i => i.isLoggedIn == true);
            // await user;

            return user.FirstOrDefault();
        }

        /// <summary>
        /// GetRegisterUsers: Read  user filter by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public RegisterUserModel GetUsers(int id)
        {
            return database.Table<RegisterUserModel>().Where(i => i.Id == id).FirstOrDefault();
        }

        /// <summary>
        /// SaveUser: Save user into database.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public int SaveUser(RegisterUserModel user)
        {
            RegisterUserModel registerUser = database.Table<RegisterUserModel>().Where(i => i.Id == user.Id).FirstOrDefault();
            if (registerUser != null)
            {
                return database.Update(user);
            }
            else
            {
                return database.Insert(user);
            }
        }

        /// <summary>
        /// DeleteUser: Delete user from database.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public int DeleteUser(RegisterUserModel user)
        {
            return database.Delete(user);
        }
        #endregion

        #region Operations on Service Table
        /// <summary>
        /// SaveServices: Save services into database.
        /// </summary>
        /// <param name="service"></param>
        /// <returns></returns>
        public int SaveServices(IEnumerable<ServiceModel> services)
        {
            int result = 0;
            foreach (ServiceModel service in services)
            {
                ServiceModel s = database.Table<ServiceModel>().Where(i => i.Id == service.Id).FirstOrDefault();
                if (s != null)
                {
                    result = database.Update(service);
                }
                else
                {
                    result = database.Insert(service);
                }
            }

            return result;
        }

        /// <summary>
        /// SaveService: Save service into database.
        /// </summary>
        /// <param name="service"></param>
        /// <returns></returns>
        public int SaveService(ServiceModel service)
        {
            ServiceModel s = database.Table<ServiceModel>().Where(i => i.Id == service.Id).FirstOrDefault();
            if (s != null)
            {
                return database.Update(service);
            }
            else
            {
                return database.Insert(service);
            }
        }

        /// <summary>
        /// GetServices: To get the list of services from the database.
        /// </summary>
        /// <returns></returns>
        public List<ServiceModel> GetServices()
        {
            List<ServiceModel> services = database.Table<ServiceModel>().ToList();
            
            return services;
        }

        /// <summary>
        /// DeleteService: Delete all services in the table.
        /// </summary>
        public void DeleteService()
        {
            database.DropTable<ServiceModel>();
            database.CreateTable<ServiceModel>();
        }
        #endregion

        #region Operations on Scope Table
        /// <summary>
        /// SaveScope: Save scopes into database.
        /// </summary>
        /// <param name="scopes"></param>
        /// <returns></returns>
        public int SaveScope(List<ScopeModel> scopes)
        {
            int result = 0;
            foreach (ScopeModel scope in scopes)
            {
                ScopeModel s = database.Table<ScopeModel>().Where(i => i.Id == scope.Id).FirstOrDefault();
                if (s != null)
                {
                    result = database.Update(scope);
                }
                else
                {
                    result = database.Insert(scope);
                }
            }

            return result;
        }

        /// <summary>
        /// SaveScope: Save scope into database.
        /// </summary>
        /// <param name="service"></param>
        /// <returns></returns>
        public int SaveScope(ScopeModel scope)
        {
            ScopeModel s = database.Table<ScopeModel>().Where(i => i.Id == scope.Id).FirstOrDefault();
            if (s != null)
            {
                return database.Update(scope);
            }
            else
            {
                return database.Insert(scope);
            }
        }

        /// <summary>
        /// GetScopes: To get the list of scopes from the database.
        /// </summary>
        /// <returns></returns>
        public List<ScopeModel> GetScopes()
        {
            List<ScopeModel> scopes = database.Table<ScopeModel>().ToList();

            return scopes;
        }

        /// <summary>
        /// DeleteScope: Delete all scope from table.
        /// </summary>
        public void DeleteScope()
        {
            database.DropTable<ScopeModel>();
            database.CreateTable<ScopeModel>();
        }
        #endregion
    }
}
