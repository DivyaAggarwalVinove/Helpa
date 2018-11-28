using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Helpa
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Messages : ContentPage
	{
        public Messages()
        {
            InitializeComponent();

            var user = App.Database.GetLoggedUser();

            if (user != null)
                wvMessage.Source = "http://180.151.232.92:151/Login.aspx?Email=" + user.Email;
            else
                DisplayAlert("", "You are logged-in. Please login for Message funactionality", "Ok");
        }
	}
}