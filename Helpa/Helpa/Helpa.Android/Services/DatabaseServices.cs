using Helpa.Droid;
using Helpa.Services;
using System;
using System.IO;
using Xamarin.Forms;

[assembly: Dependency(typeof(DatabaseServices))]
namespace Helpa.Droid
{
    public class DatabaseServices: IDatabaseServices
    {
        public string GetLocalFilePath(string filename)
        {
            //var path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            return Path.Combine(path, filename);
        }
    }
}