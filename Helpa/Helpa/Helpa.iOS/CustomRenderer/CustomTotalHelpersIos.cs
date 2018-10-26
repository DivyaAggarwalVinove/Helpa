using CoreGraphics;
using Helpa;
using Helpa.Droid;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomTotalHelpers), typeof(CustomTotalHelpersIos))]
namespace Helpa.Droid
{
    public class CustomTotalHelpersIos : LabelRenderer
    {
        public CustomTotalHelpersIos()
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {
                Control.TextAlignment = UITextAlignment.Center;

                CGRect frame = Control.Layer.Frame;
                frame.Size = new CGSize(60.0f, 60.0f);
                Control.Layer.Frame = new CGRect() { Height=60.0f, Width=60.0f};

                Control.Layer.CornerRadius = 30.0f;
                Control.Layer.BackgroundColor = UIColor.Red.CGColor;
                Control.Layer.BorderColor = UIColor.Blue.CGColor;
                Control.Layer.BorderWidth = 2;


               /* 
                CALayer bottomBorder = new CALayer();
                bottomBorder.BorderColor = UIColor.LightGray.CGColor;
                bottomBorder.BorderWidth = 1;
                new CGRect()
                bottomBorder.Frame = new RectangleF(-1, Control.Frame.Size.Height - 1, lblTitle.Frame.Size.Width, 1);
                */

            }
        }
    }
}