using Helpa;
using Helpa.iOS;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomButton), typeof(CustomButtonRendererIos))]
namespace Helpa.iOS
{
    public class CustomButtonRendererIos : ButtonRenderer
    {
        public CustomButtonRendererIos()
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
        {
            base.OnElementChanged(e);

            if(Control!=null)
            {
                Control.Layer.CornerRadius = 20.0f;
                Control.BackgroundColor = UIColor.FromRGB(255, 116, 140);
                Control.VerticalAlignment = UIControlContentVerticalAlignment.Center;
                Control.HorizontalAlignment = UIControlContentHorizontalAlignment.Center;
            }
        }
    }
}