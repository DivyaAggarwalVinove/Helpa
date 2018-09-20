using Helpa;
using Helpa.Droid;
using Android.Content;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomLabel), typeof(CustomLabelRendererAndroid))]
namespace Helpa.Droid
{
    public class CustomLabelRendererAndroid : LabelRenderer
    {
        public CustomLabelRendererAndroid(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {
                Control.SetBackgroundResource(Resource.Drawable.custom_label_style);
                //Control.TextAlignment = Android.Views.TextAlignment.Center;
                Control.Gravity = Android.Views.GravityFlags.Center;
                //Control.InputType = Android.Text.InputTypes.TextFlagCapSentences;
            }
        }
    }
}