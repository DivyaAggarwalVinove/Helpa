using Helpa.Droid;
using Android.Content;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Helpa;

[assembly: ExportRenderer(typeof(CustomHelperStatus), typeof(CustomHelperStatusAndroid))]
namespace Helpa.Droid
{
    public class CustomHelperStatusAndroid : LabelRenderer
    {
        public CustomHelperStatusAndroid(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {
                Control.SetBackgroundResource(Resource.Drawable.custom_helper_status_style);
                Control.SetPadding(10, 0, 10, 0);
                Control.Gravity = Android.Views.GravityFlags.Center;
                
                //Control.InputType = Android.Text.InputTypes.TextFlagCapSentences;
            }
        }
    }
}