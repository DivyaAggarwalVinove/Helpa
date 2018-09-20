using Helpa.Droid;
using Android.Content;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Helpa;

[assembly: ExportRenderer(typeof(CustomButton), typeof(CustomButtonRendererAndroid))]
namespace Helpa.Droid
{
    public class CustomButtonRendererAndroid : ButtonRenderer
    {
        public CustomButtonRendererAndroid(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {
                Control.SetBackgroundResource(Resource.Drawable.custom_button_style);
                Control.TextAlignment = Android.Views.TextAlignment.Center;
                Control.InputType = Android.Text.InputTypes.TextFlagCapSentences;
            }
        }
    }
}