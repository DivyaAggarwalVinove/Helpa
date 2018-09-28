using Helpa.iOS;
using Helpa.Services;
using System;
using System.IO;
using Xamarin.Forms;

[assembly: Dependency(typeof(DatabaseServices))]
namespace Helpa.iOS
{
    public class DatabaseServices: IDatabaseServices
    {
        public string GetLocalFilePath(string filename)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            return Path.Combine(path, filename);
        }
    }
}