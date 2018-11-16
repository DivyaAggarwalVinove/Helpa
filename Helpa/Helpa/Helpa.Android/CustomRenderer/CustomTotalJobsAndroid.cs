using Helpa.Droid;
using Android.Content;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Helpa;

[assembly: ExportRenderer(typeof(CustomTotalJobs), typeof(CustomTotalJobsAndroid))]
namespace Helpa.Droid
{
    public class CustomTotalJobsAndroid : LabelRenderer
    {
        public CustomTotalJobsAndroid(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {
                Control.SetBackgroundResource(Resource.Drawable.custom_total_job_style);
                //Control.TextAlignment = Android.Views.TextAlignment.Center;
                Control.Gravity = Android.Views.GravityFlags.Center;
                //Control.InputType = Android.Text.InputTypes.TextFlagCapSentences;
            }
        }
    }
}