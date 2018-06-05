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
        ServiceButton serviceButton;
        public ServiceButtonRendererAndroid(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                serviceButton = (ServiceButton)e.NewElement;
                if (serviceButton.isSelected)
                {
                    Control.SetBackgroundResource(Resource.Drawable.service_selected_button_style);
                    Control.SetTextColor(Android.Graphics.Color.ParseColor("#FE7890"));
                }
                else
                {
                    Control.SetBackgroundResource(Resource.Drawable.service_unselected_button_style);
                    Control.SetTextColor(Android.Graphics.Color.ParseColor("#BCC1C4"));
                }

                Control.TextAlignment = Android.Views.TextAlignment.Center;                
                Control.InputType = Android.Text.InputTypes.TextFlagCapSentences;
                Control.Activated = false;
                Control.SetOnClickListener(this);
            }
        }

        public void OnClick(Android.Views.View v)
        {
            if (Control.Activated)
            {
                Control.SetBackgroundResource(Android.Graphics.Color.Transparent);
                Control.SetBackgroundResource(Resource.Drawable.service_unselected_button_style);
                Control.SetTextColor(Android.Graphics.Color.ParseColor("#BCC1C4"));
            }
            else
            {
                Control.SetBackgroundResource(Android.Graphics.Color.Transparent);
                Control.SetBackgroundResource(Resource.Drawable.service_selected_button_style);
                Control.SetTextColor(Android.Graphics.Color.ParseColor("#FE7890"));
            }
            Control.Activated = !Control.Activated;
            serviceButton.serviceName = Control.Text;
            serviceButton.OnOffServices(Control.Activated);
        }        
    }
}