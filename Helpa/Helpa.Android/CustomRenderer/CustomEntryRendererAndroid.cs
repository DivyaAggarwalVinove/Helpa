using Helpa;
using Helpa.Droid;
using Android.Content;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomEntry), typeof(CustomEntryRendererAndroid))]
namespace Helpa.Droid
{
    public class CustomEntryRendererAndroid : EntryRenderer
    {
        public CustomEntryRendererAndroid(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.SetBackgroundResource(Resource.Drawable.custom_entry_style);
                Control.Gravity = Android.Views.GravityFlags.CenterVertical;
                e.NewElement.Margin = new Thickness(0);
                //Control.TextAlignment = Android.Views.TextAlignment.Center;
                Control.SetPadding(20, 15, 0, 15);
            }
        }
    }
}