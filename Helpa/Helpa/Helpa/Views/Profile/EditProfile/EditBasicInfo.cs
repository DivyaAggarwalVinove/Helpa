using AsNum.XFControls;
using Helpa.Models;
using Helpa.Services;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
namespace Helpa.Views.Profile.UserProfile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditBasicInfo : ContentPage
    {
        #region Variable Declaration
        //private ProfileInfoResponse _objProfileInfoResponse;
        //private RestApi _apiService;
        private string _baseUrl;
        //private HeaderModel _objHeaderModel;
        #endregion
        public EditBasicInfo()
        {
            InitializeComponent();
            IEnumerable<string> genders = new List<string>() { "Male", "Female", "Rather no to say" };
            SetRadioList(genders, rgGender);
            NavigationPage.SetHasNavigationBar(this, false);
            //_objProfileInfoResponse = new ProfileInfoResponse();
            //_apiService = new RestApi();
            //_objHeaderModel = new HeaderModel();
            //_objHeaderModel.TokenCode = Settings.TokenCode;
            //_baseUrl = Domain.Url + Domain.GetProfileInfoApiConstant;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            LoadPageData();
        }
        private void LoadPageData()
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                //DependencyService.Get<ITrust>().Show("No Network!");
            }
            else
            {
               // XFAIPageLoad.IsVisible = true;
                //  _objProfileInfoResponse = await _apiService.GetAsyncData_GetApi(new Get_API_Url().GetProfileInfoApi(_baseUrl,Settings.UserID), true, _objHeaderModel, _objProfileInfoResponse);
              //  XFAIPageLoad.IsVisible = false;
            }
        }

        void SetRadioList(IEnumerable<string> genderList, RadioGroup radioGroup)
        {
            radioGroup.ItemsSource = genderList;
            radioGroup.SelectedItem = genderList.ElementAt(0);
            StackLayout content = (StackLayout)radioGroup.Content;
            Radio rg = null;
            for (int i = 0; i < content.Children.Count; i++)
            {
                rg = (Radio)(content.Children[i]);
                rg.Margin = new Thickness(0, 0, 10, 0);
                rg.VerticalOptions = LayoutOptions.Center;
            }
        }

        private void OnBackPress(object sender, EventArgs e)
        {
            App.NavigationPage.Navigation.PopAsync();
        }
    }
}