using System;

using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Util;

namespace Helpa.Droid
{
    [Activity(Label = "Helpa", Icon = "@drawable/icon", Theme = "@style/MainTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            try
            {
                TabLayoutResource = Resource.Layout.Tabbar;
                ToolbarResource = Resource.Layout.Toolbar;

                base.OnCreate(bundle);

                global::Xamarin.Forms.Forms.Init(this, bundle);
                Xamarin.FormsMaps.Init(this, bundle);

                LoadApplication(new App());
            }
            catch(Exception e)
            {
                Log.Debug("Error", e.Message);
            }
        }
    }
}

