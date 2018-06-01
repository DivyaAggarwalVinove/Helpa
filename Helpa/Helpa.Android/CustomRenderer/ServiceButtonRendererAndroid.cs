using Helpa;
using Helpa.Droid;
using Android.Content;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.Views;
using Android.Graphics;
using Android.Graphics.Drawables;

[assembly: ExportRenderer(typeof(ServiceButton), typeof(ServiceButtonRendererAndroid))]
namespace Helpa.Droid
{
    public class ServiceButtonRendererAndroid : ButtonRenderer, Android.Views.View.IOnClickListener
    {
        private int resid;

        public ServiceButtonRendererAndroid(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {
                Control.SetBackgroundResource(Resource.Drawable.service_unselected_button_style);
                Control.TextAlignment = Android.Views.TextAlignment.Center;
                Control.InputType = Android.Text.InputTypes.TextFlagCapSentences;
                Control.SetOnClickListener(this);
            }
        }

        public void OnClick(Android.Views.View v)
        {
            Control.Activated = !Control.Activated;
            if (Control.Activated)
            {
                Control.SetBackgroundResource(Resource.Drawable.service_unselected_button_style);
                Control.SetTextColor(Android.Graphics.Color.ParseColor("#BCC1C4"));
            }
            else
            {
                Control.SetBackgroundResource(Resource.Drawable.service_selected_button_style);
                Control.SetTextColor(Android.Graphics.Color.ParseColor("#FE7890"));
            }
            
        }

        
    }
}