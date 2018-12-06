using Helpa.Droid;
using Android.Content;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Helpa;

[assembly: ExportRenderer(typeof(CustomReviewLabel), typeof(CustomReviewLabelAndroid))]
namespace Helpa.Droid
{
    public class CustomReviewLabelAndroid : LabelRenderer
    {
        public CustomReviewLabelAndroid(Context context) : base(context)
        {
        }
        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {
                Control.SetMaxLines(3);
                
                //Control.InputType = Android.Text.InputTypes.TextFlagCapSentences;
            }
        }
    }
}