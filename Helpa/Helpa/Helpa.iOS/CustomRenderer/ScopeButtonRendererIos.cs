using Helpa;
using Helpa.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using UIKit;
using Foundation;
using CoreGraphics;
using System;

[assembly: ExportRenderer(typeof(ScopeButton), typeof(ScopeButtonRendererIos))]
namespace Helpa.Droid
{
    public class ScopeButtonRendererIos : ButtonRenderer
    {
        ScopeButton scopeButton;

        public ScopeButtonRendererIos()
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                scopeButton = (ScopeButton)e.NewElement;
                if (scopeButton.isSelected)
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

        void TouchUpInsideEvent(object sender, EventArgs eventArgs)
        {
            if (scopeButton.isSelected)
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
            scopeButton.isSelected = !scopeButton.isSelected;
            scopeButton.scopeName = Control.TitleLabel.Text.ToString();
            scopeButton.OnOffScope(scopeButton.isSelected);
        }

        /*
        public void OnClick(Android.Views.View v)
        {
            if (scopeButton.isSelected)
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
            scopeButton.isSelected = !scopeButton.isSelected;
            scopeButton.scopeName = Control.Text;
            scopeButton.OnOffScope(scopeButton.isSelected);
        }
        */
    }
}