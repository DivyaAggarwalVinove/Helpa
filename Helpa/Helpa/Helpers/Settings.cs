using System;
using System.Collections.Generic;
using System.Text;
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace Helpa
{
    /// <summary>
    /// This is the Settings static class that can be used in your Core solution or in any
    /// of your client applications. All settings are laid out the same exact way with getters
    /// and setters. 
    /// </summary>
    public static class Settings
    {
        private static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }

        #region Setting Constants

        private const string SettingsKey = "settings_key";
        private static readonly string SettingsDefault = string.Empty;

        #endregion


        public static string GeneralSettings
        {
            get
            {
                return AppSettings.GetValueOrDefault(SettingsKey, SettingsDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(SettingsKey, value);
            }
        }

        #region Setting TokenCode

        private const string tokenCode = "tokenCode_key";
        private static readonly string tokenCodeDefault = string.Empty;




        public static string TokenCode
        {
            get
            {
                return AppSettings.GetValueOrDefault(tokenCode, tokenCodeDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(tokenCode, value);
            }
        }
        #endregion
        #region Setting UserID

        private const string userid = "userid_key";
        private static readonly int useridDefault = 0;




        public static int UserID
        {
            get
            {
                return AppSettings.GetValueOrDefault(userid, useridDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(userid, value);
            }
        }
        #endregion
    }
}
