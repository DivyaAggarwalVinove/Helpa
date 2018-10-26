using Helpa;
using Helpa.Droid;
using Android.Content;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomStackLayout), typeof(CustomStackLayoutRendererAndroid))]
namespace Helpa.Droid
{
    public class CustomStackLayoutRendererAndroid : VisualElementRenderer<StackLayout>
    {
        public CustomStackLayoutRendererAndroid(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<StackLayout> e)
        {
            base.OnElementChanged(e);
            StackLayout sl = e.NewElement;
            sl.VerticalOptions = LayoutOptions.Center;
            //if (Control != null)
            {
                this.SetBackgroundResource(Resource.Drawable.custom_stacklayout_style);
                //Control.TextAlignment = Android.Views.TextAlignment.Center;
            }
        }
    }
}