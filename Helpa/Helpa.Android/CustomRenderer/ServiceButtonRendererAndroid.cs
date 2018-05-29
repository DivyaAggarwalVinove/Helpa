using Helpa;
using Helpa.Droid;
using Android.Content;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(ServiceButton), typeof(ServiceButtonRendererAndroid))]
namespace Helpa.Droid
{
    public class ServiceButtonRendererAndroid : ButtonRenderer
    {
        public ServiceButtonRendererAndroid(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {
                Control.SetBackgroundResource(Resource.Drawable.service_button_style);
                Control.TextAlignment = Android.Views.TextAlignment.Center;
                Control.InputType = Android.Text.InputTypes.TextFlagCapSentences;
            }
        }
    }
}