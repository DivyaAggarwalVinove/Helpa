
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Xamarin.RangeSlider.Forms;
using Xamarin.RangeSlider.Forms.Samples.iOS.PlattformEffects;

[assembly: ResolutionGroupName("EffectsSlider")]
//[assembly: ExportEffect(typeof(RangeSliderEffect), "RangeSliderEffect")]

[assembly: ExportRenderer(typeof(RangeSlider), typeof(RangeSliderEffect))]
namespace Xamarin.RangeSlider.Forms.Samples.iOS.PlattformEffects
{
    public class RangeSliderEffect : PlatformEffect
    {
        protected override void OnAttached()
        {
            // var ctrl = (Xamarin.RangeSlider.RangeSliderControl)Control;
            // ctrl.TintColor = Color.Fuchsia.ToUIColor();
        }

        protected override void OnDetached()
        {
        }
    }
}