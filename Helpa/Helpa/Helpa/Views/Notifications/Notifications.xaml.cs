using Helpa.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Helpa
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Notifications : ContentPage
	{
		public Notifications ()
		{
			InitializeComponent ();

            BindingContext = new NotificationsViewModel(this);
        }
	}
}