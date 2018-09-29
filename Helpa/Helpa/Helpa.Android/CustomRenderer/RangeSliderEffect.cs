using Android.Content;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Xamarin.RangeSlider.Forms.Samples.Droid.PlattformEffects;

[assembly: ResolutionGroupName("EffectsSlider")]
[assembly: ExportEffect(typeof(RangeSliderEffect), "RangeSliderEffect")]
namespace Xamarin.RangeSlider.Forms.Samples.Droid.PlattformEffects
{
    public class RangeSliderEffect : PlatformEffect
    {
        protected override void OnAttached()
        {
            var ctrl = (RangeSliderControl)Control;

           Context context = Xamarin.Forms.Forms.Context;
           //Bitmap icon = BitmapFactory.DecodeResource(context.Resources, Resource.Drawable.);
            //ctrl.SetCustomThumbImage(icon);
        }

        protected override void OnDetached()
        {
        }
    }
}