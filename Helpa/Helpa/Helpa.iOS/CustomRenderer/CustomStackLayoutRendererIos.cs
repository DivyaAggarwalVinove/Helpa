//using Helpa.CustomControl;
using System;
using CoreGraphics;
using Helpa;
using Helpa.Droid;
//using Android.Content;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomStackLayout), typeof(CustomStackLayoutRendererIos))]
namespace Helpa.Droid
{
    public class CustomStackLayoutRendererIos : VisualElementRenderer<StackLayout>
    {
        public CustomStackLayoutRendererIos()
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<StackLayout> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                View sl = e.NewElement;
                sl.BackgroundColor = Color.White;
                sl.VerticalOptions = LayoutOptions.Center;

                this.Layer.CornerRadius = 5.0f;
                this.Layer.ShadowRadius = 5.0f;
            }
            //CustomStackLayout csl = (CustomStackLayout)Element;

            /*if (Control != null)
            {

                //this.SetBackgroundResource(Resource.Drawable.custom_stacklayout_style);
                //Control.TextAlignment = Android.Views.TextAlignment.Center;
            }*/
        }
    }
}