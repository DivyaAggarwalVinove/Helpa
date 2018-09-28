using Helpa;
using Helpa.iOS;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomEntry), typeof(CustomEntryRendererIos))]
namespace Helpa.iOS
{
    public class CustomEntryRendererIos : EntryRenderer
    {
        public CustomEntryRendererIos()
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if(Control!=null)
            {
                Control.BorderStyle = UITextBorderStyle.None;
                Control.LayoutMargins = new UIEdgeInsets(0,5,0,5);

                Control.Layer.CornerRadius = 10.0f;
                Control.BackgroundColor = UIColor.White;
                Control.VerticalAlignment = UIControlContentVerticalAlignment.Center;
            }
        }
    }
}
