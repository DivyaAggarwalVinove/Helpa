using Helpa.Models;
using Helpa.ViewModels.ProfileViewModel.DetailedProfileViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Helpa.Views.Profile.DetailedProfile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DetailedProfilePage : ContentPage
    {
        public RegisterUserModel LoggedinUser { get; set; }
        public DetailedProfilePage(RegisterUserModel LoggedinUser)
        {
            InitializeComponent();
            this.LoggedinUser = LoggedinUser;

            BindingContext = new DetailedProfileViewModel(LoggedinUser);
        }
    }
}