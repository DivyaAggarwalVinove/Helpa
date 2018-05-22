using Android.App;
using Android.Content;
using Android.OS;
using Java.Lang;
using Xamarin.Facebook;


[assembly: MetaData("com.facebook.sdk.ApplicationId", Value = "@string/app_id")]
namespace Helpa.Droid
{
    [Activity(Label = "")]
    public class FacebookActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            
            // Create your application here
        }
    }
}