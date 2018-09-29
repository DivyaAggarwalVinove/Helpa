using Helpa;
using Helpa.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using UIKit;
using Foundation;
using CoreGraphics;
using System;

[assembly: ExportRenderer(typeof(ServiceButton), typeof(ServiceButtonRendererIos))]
namespace Helpa.Droid
{
    public class ServiceButtonRendererIos : ButtonRenderer
    {
        ServiceButton serviceButton;

        public ServiceButtonRendererIos() : base()
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
                    Control.Layer.CornerRadius = 2.0f;
                    Control.BackgroundColor = UIColor.White;
                    Control.Layer.BorderWidth = 1.0f;
                    Control.Layer.BorderColor = UIColor.FromRGB(254, 120, 144).CGColor;
                    Control.SetTitleColor(UIColor.FromRGB(254, 120, 144), UIControlState.Normal);
                }
                else
                {
                    Control.TitleEdgeInsets = new UIEdgeInsets(0, 5, 0, 5);

                    Control.Layer.CornerRadius = 2.0f;
                    Control.BackgroundColor = UIColor.White;
                    Control.Layer.BorderWidth = 1.0f;
                    Control.Layer.BorderColor = UIColor.FromRGB(188, 193, 196).CGColor;
                    Control.SetTitleColor(UIColor.FromRGB(188, 193, 196), UIControlState.Normal);
                }

                //CGRect frame = Control.Frame;
                //frame.Width = frame.re + 50;
                Control.SizeToFit();


                Control.Bounds = new CGRect(Control.Bounds.X, Control.Bounds.Y, Control.Bounds.Width, Control.Bounds.Height);
                Control.VerticalAlignment = UIControlContentVerticalAlignment.Center;
                Control.HorizontalAlignment = UIControlContentHorizontalAlignment.Center;

                Control.Selected = false;

                Control.TouchUpInside += TouchUpInsideEvent;
            }
        }

        public override CGSize SizeThatFits(CGSize size)
        {
            size.Width = size.Width + 20;
            return base.SizeThatFits(size);
        }

        void TouchUpInsideEvent(object sender, EventArgs eventArgs)
        {
            if (serviceButton.isSelected)
            {
                Control.TitleEdgeInsets = new UIEdgeInsets(0, 5, 0, 5);

                Control.Layer.CornerRadius = 2.0f;
                Control.BackgroundColor = UIColor.White;
                Control.Layer.BorderWidth = 1.0f;
                Control.Layer.BorderColor = UIColor.FromRGB(188, 193, 196).CGColor;
                Control.SetTitleColor(UIColor.FromRGB(188, 193, 196), UIControlState.Normal);
            }
            else
            {
                Control.Layer.CornerRadius = 2.0f;
                Control.BackgroundColor = UIColor.White;
                Control.Layer.BorderWidth = 1.0f;
                Control.Layer.BorderColor = UIColor.FromRGB(254, 120, 144).CGColor;
                Control.SetTitleColor(UIColor.FromRGB(254, 120, 144), UIControlState.Normal);
            }
            serviceButton.isSelected = !serviceButton.isSelected;
            serviceButton.serviceName = Control.TitleLabel.Text.ToString();
            serviceButton.OnOffServices(serviceButton.isSelected);
        }
    }
}