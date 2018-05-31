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
    }
}
