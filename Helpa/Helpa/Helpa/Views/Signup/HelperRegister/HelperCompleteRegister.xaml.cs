using AsNum.XFControls;
using Helpa.DependencyServices;
using Helpa.Models;
using Helpa.Services;
using Helpa.Utility;
using Plugin.Geolocator;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.RangeSlider.Forms;

namespace Helpa
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HelperCompleteRegister : ContentPage
    {
        RegisterUserModel currentUser;
        HelperServiceModel helperServices;
        static int currentService;

        private static HelperCompleteRegister instance;
        public static HelperCompleteRegister Instance
        {
            get { return instance; }
            set { instance = value; }
        }

        public HelperCompleteRegister(RegisterUserModel user)
        {
            InitializeComponent();

            currentService = 0;
            instance = this;
            NavigationPage.SetHasNavigationBar(this, false);
            currentUser = user;

            btnHelperRegSelectedService.isSelected = true;
            //btnHelperRegTbd.isSelected = false;

            entryHelperRegUsername1.Text = user.UserName;
            entryHelperRegPhone1.Text = user.MobileNumber;
            entryHelperRegEmail1.Text = user.Email;

            IEnumerable<string> genders = new List<string>() { "Male", "Female", "Rather no to say" };
            SetRadioList(genders, rgHelperGender);

            //IEnumerable<String> locationTypes = new List<string>() { "Helper's Home", "Mobile Helper"};
            //SetRadioList(locationTypes, rgHelperLocationType);

            IEnumerable<string> statusList = new List<string>() { "Available", "Not available" };
            SetRadioList(statusList, rgHelperStatus);

            SetServices();

            //if (user.IsServiced)
            //    GoToBuildTrust();
            //else 
            //if (user.IsCompleted)
            //    GoToEditServices();
            //else
            //if (user.IsRegistered)
            //{
            //    GoToBasicInfo();
            //}

            MessagingCenter.Subscribe<HelperRegister2,string>(this, "Current Address", (sender, currLoc) =>
            {
                string[] locations = currLoc.Split(new char[] { ';' });
                entryHelperRegLocation.Text = locations[0];
                helperServices.Service[currentService].LocationName = locations[0];
                helperServices.Service[currentService].Latitude = locations[1];
                helperServices.Service[currentService].Longitude = locations[2];
            });

            MessagingCenter.Subscribe<HelperRegister3, string>(this, "Selected Address", (sender, selectedAddress) =>
            {
                entryHelperRegLocation.Text = selectedAddress;

            });

            MessagingCenter.Subscribe<ServiceButton, bool>(this, "Select Or Unselect Service", (sender, isSelected) =>
            {
                try
                {
                    string serviceName = ((ServiceButton)sender).serviceName;

                    if (string.Equals(serviceName, "Hourly"))
                    {
                        if (((ServiceButton)sender).isSelected)
                            gridPriceHour.IsVisible = true;
                        else
                            gridPriceHour.IsVisible = false;
                    }
                    else if (string.Equals(serviceName, "Daily"))
                    {
                        if (((ServiceButton)sender).isSelected)
                            gridPriceDay.IsVisible = true;
                        else
                            gridPriceDay.IsVisible = false;
                    }
                    else if (string.Equals(serviceName, "Monthly"))
                    {
                        if (((ServiceButton)sender).isSelected)
                            gridPriceMonth.IsVisible = true;
                        else
                            gridPriceMonth.IsVisible = false;
                    }
                    else
                    {
                        List<ServiceModel> services = App.Database.GetServicesAsync();
                        ServiceModel service = services.Where(a => a.ServiceName == serviceName).FirstOrDefault();
                        //var selectedService = helperServices.Service.Where(a => a.ServiceId == service.Id).FirstOrDefault();
                        int serviceIndex = services.IndexOf(service);
                        var gService = gridServices.Children.Where(c => Grid.GetRow(c) == (serviceIndex) / 3 && Grid.GetColumn(c) == (serviceIndex) % 3);
                        gService.ElementAt(1).IsVisible = !gService.ElementAt(1).IsVisible;

                        if (isSelected)
                        {
                            AddGrid(gridHelperEditServices);

                            HelperServices helperService = new HelperServices();
                            helperService.ServiceId = service.Id;
                            helperService.ServiceName = service.ServiceName;
                            helperServices.Service.Add(helperService);

                            //selectedService.isSelected = true;
                            //App.Database.SaveServiceAsync(selectedService);
                        }
                        else
                        {
                            int count = gridHelperEditServices.ColumnDefinitions.Count;

                            for (int i = count - 1; i >= 0; i--)
                            {
                                gridHelperEditServices.ColumnDefinitions.RemoveAt(i);
                                gridHelperEditServices.Children.RemoveAt(i);
                            }

                            for (int i = 0; i < count - 1; i++)
                                AddGrid(gridHelperEditServices);

                            HelperServices helperService =  helperServices.Service.Where(a => a.ServiceId == service.Id).FirstOrDefault();
                            helperServices.Service.Remove(helperService);

                            //selectedService.isSelected = false;
                            //App.Database.SaveServiceAsync(selectedService);
                        }

                        var g = gridHelperEditServices.Children.Where(c => Grid.GetRow(c) == 0 && Grid.GetColumn(c) == 0).First();
                        g.BackgroundColor = Color.FromHex("#FF748C");
                    }
                }
                catch (Exception e)
                {
                    Console.Write(e.StackTrace);
                }
            });

            MessagingCenter.Subscribe<ScopeButton, bool>(this, "Select Or Unselect Service", (sender, isSelected) =>
            {
                try
                {
                    string scopeName = ((ScopeButton)sender).scopeName;
                    List<ScopeModel> scopes = App.Database.GetScopesAsync();
                    ScopeModel scope = scopes.Where(a => a.ScopeName == scopeName).FirstOrDefault();

                    if (isSelected)
                    {
                        HelperScopes helperScope = new HelperScopes() { ScopeId = scope.Id };
                        helperServices.Service[currentService].Scopes.Add(helperScope);
                    }
                    else
                    {
                        HelperScopes helperScope = helperServices.Service[currentService].Scopes.Where(x => x.ScopeId == scope.Id).FirstOrDefault();
                        helperServices.Service[currentService].Scopes.Remove(helperScope);
                    }

                    //App.Database.SaveScopeAsync(selectedScope);
                }
                catch (Exception e)
                {
                    Console.Write(e.StackTrace);
                }
            });
        }

        void AddGrid(Grid grid)
        {
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.1, GridUnitType.Star) });

            Grid g = new Grid();
            g.HorizontalOptions = LayoutOptions.FillAndExpand;
            g.VerticalOptions = LayoutOptions.FillAndExpand;
            g.BackgroundColor = Color.FromHex("#818A8F");
            grid.Children.Add(g, grid.ColumnDefinitions.Count - 1, 0);
            grid.BackgroundColor = Color.Transparent;
        }

        void OnHomeLocationSelected(object sender, EventArgs args)
        {
            //rgHelperHomeLocation.Checked = !rgHelperHomeLocation.Checked;
            if (rgHelperHomeLocation.Checked)
            {
                rgHelperMobileLocation.Checked = !rgHelperMobileLocation.Checked;

                btnHelperRegDistrict.IsVisible = false;
                entryHelperRegLocation.IsVisible = true;
            }
            else
                rgHelperHomeLocation.Checked = !rgHelperHomeLocation.Checked;
        }

        void OnMobileLocationSelected(object sender, EventArgs args)
        {
            if (rgHelperMobileLocation.Checked)
            {
                rgHelperHomeLocation.Checked = !rgHelperHomeLocation.Checked;

                btnHelperRegDistrict.IsVisible = true;
                entryHelperRegLocation.IsVisible = false;
            }
            else
                rgHelperMobileLocation.Checked = !rgHelperMobileLocation.Checked;
            //rgHelperMobileLocation.Checked = !rgHelperMobileLocation.Checked;
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

        //void SetStatus()
        //{
        //    IEnumerable<string> statusList = new List<string>() { "Available", "Not available" };
        //    rgHelperStatus.ItemsSource = statusList;
        //    StackLayout content = (StackLayout)rgHelperStatus.Content;
        //    Radio rg = null;
        //    for (int i = 0; i < content.Children.Count; i++)
        //    {
        //        rg = (Radio)(content.Children[i]);
        //        rg.Margin = new Thickness(0, 0, 20, 0);
        //        rg.VerticalOptions = LayoutOptions.Center;
        //    }
        //}

        async void SetServices()
        {
            #region set services
            IList<ServiceModel> services = await (new Utilities()).GetServicesAsync();

            int servicesCount = services.Count();
            for (int i = 0; i < (servicesCount - 1) / 3 + 1; i++)
                gridServices.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            for (int i = 0; i < 3; i++)
                gridServices.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
            
            gridServices.Padding = new Thickness(0, 0, 10, 0);

            ServiceButton serviceButton;
            for (int i = 0; i < servicesCount; i++)
            {
                serviceButton = new ServiceButton();
                serviceButton.Text = services.ElementAt(i).ServiceName;
                serviceButton.Margin = new Thickness(5, 5, 5, 5);
                serviceButton.HorizontalOptions = LayoutOptions.FillAndExpand;
                
                Image image = new Image();
                image.Source = "selected.png";
                image.Aspect = Aspect.AspectFit;
                image.Margin = new Thickness(20, 15, -5, 15);
                image.HorizontalOptions = LayoutOptions.End;
                image.IsVisible = false;

                gridServices.Children.Add(serviceButton, i % 3, i / 3);
                gridServices.Children.Add(image, i % 3, i / 3);                
                #endregion
            }
            App.Database.DeleteServiceAsync();
            await App.Database.SaveServicesAsync(services);
        }

        void SetPriceType()
        {
            List<string> price = new List<string>{ "Hourly", "Daily", "Monthly", "TBD" };

            foreach (View gPrice in gridPriceType.Children.ToList())
            {
                gridPriceType.Children.Remove(gPrice);
            }

            gridPriceType.RowDefinitions = new RowDefinitionCollection();
            gridPriceType.ColumnDefinitions = new ColumnDefinitionCollection();

            gridPriceType.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            foreach (string p in price)
                gridPriceType.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            ServiceButton priceButton;
            for (int i = 0; i < price.Count; i++)
            {
                priceButton = new ServiceButton();
                priceButton.Text = price.ElementAt(i);
                priceButton.Margin = new Thickness(5, 5, 5, 5);
                
                //Image image = new Image();
                //image.Source = "selected.png";
                //image.Aspect = Aspect.AspectFit;
                //image.Margin = new Thickness(20, 15, 0, 15);
                //image.HorizontalOptions = LayoutOptions.End;
                //image.IsVisible = false;

                gridPriceType.Children.Add(priceButton, i, 0);
                //gridPriceType.Children.Add(image, i, 0);
            }
        }

        async void SetScopes(int serviceId)
        {
            #region set scopes
            List<ScopeModel> scopes = await (new Utilities()).GetScpoesAsync(serviceId);

            foreach(View gScope in gridScopes.Children.ToList())
            {
                gridScopes.Children.Remove(gScope);
            }

            int scopesCount = scopes.Count();
            for (int i = 0; i < (scopesCount - 1) / 3 + 1; i++)
                gridScopes.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            for (int i = 0; i < 3; i++)
                gridScopes.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

            ScopeButton scopeButton;
            for (int i = 0; i < scopesCount; i++)
            {
                scopeButton = new ScopeButton();
                scopeButton.Text = scopes.ElementAt(i).ScopeName;
                scopeButton.Margin = new Thickness(10, 5, 10, 5);

                //Image image = new Image();
                //image.Source = "selected.png";
                //image.Aspect = Aspect.AspectFit;
                //image.Margin = new Thickness(20, 15, 0, 15);
                //image.HorizontalOptions = LayoutOptions.End;
                //image.IsVisible = false;
                
                gridScopes.Children.Add(scopeButton, i % 3, i / 3);
                //gridScopes.Children.Add(image, i % 3, i / 3);
            }
            #endregion

            App.Database.DeleteScopeAsync();
            await App.Database.SaveScopeAsync(scopes);
        }

        //async void SetLocation()
        //{ 
        //    var locator = CrossGeolocator.Current;
        //    TimeSpan ts = TimeSpan.FromTicks(1000000);
        //    Plugin.Geolocator.Abstractions.Position position = await locator.GetPositionAsync(ts);
        //    var addr = await locator.GetAddressesForPositionAsync(new Plugin.Geolocator.Abstractions.Position(position.Latitude, position.Longitude), "AIzaSyDminfXt_CoSb9UTXpPFZwQIG2lDduDMjs");

        //    var a = addr.FirstOrDefault();
        //    entryHelperRegSearch.Text = a.FeatureName + "," + a.SubLocality + "," + a.Locality + "," + a.CountryName + "," + a.PostalCode;
        //}

        async void OnHelperRegBasicInfoNext(object sender, EventArgs args)
        {
            if (string.IsNullOrEmpty(entryHelperRegUsername1.Text))
            {
                await DisplayAlert("Warning", "Please enter username", "Ok");
            }
            else if (string.IsNullOrEmpty(entryHelperRegPhone1.Text))
            {
                await DisplayAlert("Warning", "Please enter phone number", "Ok");
            }
            else if (string.IsNullOrEmpty(entryHelperRegEmail1.Text))
            {
                await DisplayAlert("Warning", "Please enter email", "Ok");
            }
            else if (Utils.IsValidMobileNo(entryHelperRegPhone1.Text))
            {
                await DisplayAlert("Warning", "Please enter valid mobile number", "Ok");
            }
            else if (!Utils.IsValidEmail(entryHelperRegEmail1.Text))
            {
                await DisplayAlert("Warning", "Please enter valid email", "Ok");
            }
            else
            {
                currentUser.IsVerified = true;
                currentUser.MobileNumber = entryHelperRegPhone1.Text;
                try
                {
                    Gender g = (Gender)Enum.Parse(typeof(Gender), rgHelperGender.SelectedItem.ToString());
                    currentUser.Gender = (int)g;
                }
                catch(Exception e)
                {
                    Console.Write(e.StackTrace);
                    currentUser.Gender = 3;
                }
                //currentUser.Gender = rgHelperGender.SelectedItem.ToString();
                currentUser.Email = entryHelperRegEmail1.Text;

                await (new RegisterServices()).CompleteRegisterService(currentUser);
                //App.Database.SaveUserAsync(currentUser);
                //GoToEditServices();
            }
        }

        public void GotoNext(RegisterUserModel userModel)
        {
            userModel.IsCompleted = true;
            App.Database.SaveUserAsync(userModel);
            GoToEditServices();
            //Navigation.PushAsync(new HelperRegister1(userModel));
        }

        public void ShowError(string error)
        {
            DisplayAlert("Error", error, "Ok");
        }

        public void ShowSmsMessage(ResponseModel message, bool isSuccess)
        {
            if (isSuccess)
            {
                entryHelperRegPhone1.IsEnabled = false;
            }

            DisplayAlert("", message.Message, "Ok");
        }

        public void ShowVerificationMessage(ResponseModel message, bool isSuccess)
        {
            DisplayAlert("", message.Message, "Ok");
        }

        void OnHelperRegEditServicesNext(object sender, EventArgs args)
        {
            List<ServiceModel> selectedServices = App.Database.GetServicesAsync();
            if (helperServices.Service.Count == 0)
                DisplayAlert("Warning", "Please select at least any one service.", "Ok");
            else
            {
                if (rgHelperStatus.SelectedItem.ToString() == "Available")
                    helperServices.Status = true;
                else
                    helperServices.Status = false;

                helperServices.Qualification = entryHelperRegQualifications.Text;
                helperServices.ExperienceYears = int.Parse(btnHelperRegExpCount.Text);
                helperServices.MinAge = int.Parse(btnHelperRegAgeMin.Text);
                helperServices.MaxAge = int.Parse(btnHelperRegAgeMax.Text);
                helperServices.UserId = App.Database.GetRegisteredUser().Id;

                GotoNextService(currentService);
            }
        }

        void OnCallNextService(object sender, EventArgs args)
        {
            try
            {
                // List<ServiceModel> selectedServices = App.Database.GetServicesAsync();
                if(entryHelperRegLocation.Text.Equals(""))
                {
                    DisplayAlert("Warning", "Please select location.", "Ok");
                    return;
                }

                //Save services data
                HelperServices service = new HelperServices();
                if (rgHelperHomeLocation.Checked)
                    helperServices.Service[currentService].LocationType = 1;
                else
                    helperServices.Service[currentService].LocationType = 2;

                if (((ServiceButton)gridPriceType.Children.ElementAt(0)).isSelected)
                {
                    helperServices.Service[currentService].Hour = true;
                    helperServices.Service[currentService].MinPriceHour = float.Parse(btnHelperRegPriceHrMin.Text.Substring(2));
                    helperServices.Service[currentService].MaxPriceHour = float.Parse(btnHelperRegPriceHrMax.Text.Substring(2));
                }

                if (((ServiceButton)gridPriceType.Children.ElementAt(1)).isSelected)
                {
                    helperServices.Service[currentService].Day = true;
                    helperServices.Service[currentService].MinDayPrice = float.Parse(btnHelperRegPriceDayMin.Text.Substring(2));
                    helperServices.Service[currentService].MaxDayPrice = float.Parse(btnHelperRegPriceDayMax.Text.Substring(2));
                }

                if (((ServiceButton)gridPriceType.Children.ElementAt(2)).isSelected)
                {
                    helperServices.Service[currentService].Month = true;
                    helperServices.Service[currentService].MinMonthPrice = float.Parse(btnHelperRegPriceMonthMin.Text.Substring(2));
                    helperServices.Service[currentService].MaxMonthPrice = float.Parse(btnHelperRegPriceMonthMax.Text.Substring(2));
                }

                //helperServices.Service[currentService].Scopes = new List<HelperScopes>();
                //HelperScopes scope = new HelperScopes();
                //foreach (ScopeModel scope in scopes)
                //{
                //    helperServices.Service[currentService].Scopes.Add(scope.ScopeId);
                //    services.ElementAt(currentService).Scopes = (scopes);
                //}

                //Save Service into database
                //App.Database.SaveServiceAsync(service);

                GotoNextService(++currentService);
            }
            catch(Exception e)
            {
                Console.Write(e.StackTrace);
            }
        }
                
        async void GotoNextService(int i)
        {
            if (i >= helperServices.Service.Count)
            {
                helperServices = (await (new HelpersServices()).SaveHelperServices(helperServices));

                if (helperServices.HelperId != 0)
                {
                    RegisterUserModel user = await App.Database.GetUsersAsync(helperServices.UserId);
                    user.IsServiced = true;
                    await App.Database.SaveUserAsync(user);

                    await DisplayAlert("Information", "Services saved successfully", "Ok");

                    GoToBuildTrust();
                }
                else
                {
                    await DisplayAlert("Error", "Somethin wrong! Please try again.", "Ok");
                }


                return; // Call HelperServices and go to Build Trust
            }
            
            var g = gridHelperEditServices.Children.ElementAt(i+1);
            g.BackgroundColor = Color.FromHex("#FF748C");

            svHelperEditServices.IsVisible = false;
            svHelperRegHelperHome.IsVisible = true;

            // Reset data
            HelperServices service = helperServices.Service.ElementAt(i);
            btnHelperRegSelectedService.Text = service.ServiceName;

            SetScopes(service.ServiceId);
            SetPriceType();
            helperServices.Service[i].Scopes = new List<HelperScopes>();

            entryHelperRegLocation.Text = "";

            //btnHelperRegHourly.isSelected = false;
            gridPriceHour.IsVisible = false;

            //btnHelperRegMonthly.isSelected = false;
            gridPriceMonth.IsVisible = false;

            //btnHelperRegDaily.isSelected = false;
            gridPriceDay.IsVisible = false;

            //btnHelperRegTbd.isSelected = false;
        }
        
        void SetYearOfExperience(object sender, EventArgs args) => 
            btnHelperRegExpCount.Text =((RangeSlider)sender).UpperValue.ToString();

        void SetMinHour(object sender, EventArgs args) => 
            btnHelperRegPriceHrMin.Text = "$ " + ((RangeSlider)sender).LowerValue.ToString();

        void SetMaxHour(object sender, EventArgs args) =>
            btnHelperRegPriceHrMax.Text = "$ " + ((RangeSlider)sender).UpperValue.ToString();
        
        void SetMinDay(object sender, EventArgs args) =>
            btnHelperRegPriceDayMin.Text = "$ " + ((RangeSlider)sender).LowerValue.ToString();
        
        void SetMaxDay(object sender, EventArgs args) =>
            btnHelperRegPriceDayMax.Text = "$ " + ((RangeSlider)sender).UpperValue.ToString();
        
        void SetMinMonth(object sender, EventArgs args) =>
            btnHelperRegPriceMonthMin.Text = "$ " + ((RangeSlider)sender).LowerValue.ToString();

        void SetMaxMonth(object sender, EventArgs args) =>
            btnHelperRegPriceMonthMax.Text = "$ " + ((RangeSlider)sender).UpperValue.ToString();
        
        void SetMinAge(object sender, EventArgs args) =>
            btnHelperRegAgeMin.Text = ((RangeSlider)sender).LowerValue.ToString();

        void SetMaxAge(object sender, EventArgs args) =>
            btnHelperRegAgeMax.Text = ((RangeSlider)sender).UpperValue.ToString();

        //protected override bool OnBackButtonPressed()
        //{
        //    if (svHelperEditServices.IsVisible)
        //    {
        //        GoToBasicInfo();
        //        return true;
        //    }
        //    else
        //    {
        //        return base.OnBackButtonPressed();
        //    }
        //}

        void OnFocus(object sender, EventArgs args)
        {
            Navigation.PushAsync(new HelperRegister2());
        }

        List<string> Certificate = new List<string>();
        async void OnCertificateUpload(object sender, EventArgs args)
        {
            ((Button)sender).IsEnabled = false;
            Stream stream = await DependencyService.Get<IPicturePicker>().GetImageStreamAsync();
            Stream stream1=new MemoryStream();
           
            if (stream != null)
            {
                imgTrustDoc.Source = ImageSource.FromStream(() => stream);

                //stream.CopyTo(stream1);

                //StreamReader reader = new StreamReader(stream1);
                //byte[] bytedata = Encoding.Default.GetBytes(reader.ReadToEnd());
                //string strbase64 = Convert.ToBase64String(bytedata);
                //Certificate.Add(strbase64);
            }

            ((Button)sender).IsEnabled = true;            
        }

        List<string> IdCard = new List<string>();
        async void OnIdCardUpload(object sender, EventArgs args)
        {
            ((Button)sender).IsEnabled = false;
            Stream stream = await DependencyService.Get<IPicturePicker>().GetImageStreamAsync();

            if (stream != null)
            {
                imgTrustDoc.Source = ImageSource.FromStream(() => stream);
                
                // Convert to Base64
                //StreamReader reader = new StreamReader(stream);
                //byte[] bytedata = Encoding.Default.GetBytes(reader.ReadToEnd());
                //string strBase64 = Convert.ToBase64String(bytedata);
                //IdCard.Add(strBase64);
            }

            ((Button)sender).IsEnabled = true;
        }

        List<string> Carousel = new List<string>();
        async void OnCarouselUpload(object sender, EventArgs args)
        {
            ((Button)sender).IsEnabled = false;
            Stream stream = await DependencyService.Get<IPicturePicker>().GetImageStreamAsync();

            if (stream != null)
            {
                imgTrustDoc.Source = ImageSource.FromStream(() => stream);
                
                // Convert to Base64
                //StreamReader reader = new StreamReader(stream);
                //byte[] bytedata = Encoding.Default.GetBytes(reader.ReadToEnd());
                //string strBase64 = Convert.ToBase64String(bytedata);

                //Carousel.Add(strBase64);
            }

            ((Button)sender).IsEnabled = true;
        }

        async void OnTrustUpload(object sender, EventArgs args)
        {
            var trust = new  { HelperId = 42,
            SelfIntroduction = entrySelfIntrodution.Text,
                Certificate = Certificate,
                Carousels = Carousel
            };

            bool result = await (new Trust()).UploadTrust(trust);

            if (result)
                await DisplayAlert("Information", "Helper Sign up successfully", "Ok");
            else
                await DisplayAlert("Information", "Helper Sign up successfully. but document not uploaded ", "Ok");

            List<Page> pages = Navigation.NavigationStack.ToList();
            for (int i = pages.Count - 1; i > 0; i--)
            {
                if (!pages[i].ToString().Contains("Login"))
                    await Navigation.PopAsync();
                else
                    break;
            }
        }

        void GoToBasicInfo()
        {
            helperCompleteRegisterTitle.Text = "Helper Sign Up 1/3";
            gridHelperEditServices.BackgroundColor = Color.FromHex("#818A8F");

            for (int i = 0; i < gridHelperEditServices.ColumnDefinitions.Count; i++)
            {
                var g = gridHelperEditServices.Children.ElementAt(i);
                g.BackgroundColor = Color.FromHex("#818A8F");
            }
            svHelperBasicInfo.IsVisible = true;
            svHelperEditServices.IsVisible = false;
        }

        void GoToEditServices()
        {
            helperCompleteRegisterTitle.Text = "Helper Sign Up 2/3";
            gridHelperEditServices.BackgroundColor = Color.Transparent;
            gridHelperEditServices.Children.ElementAt(0).BackgroundColor = Color.FromHex("#FF748C");
            svHelperBasicInfo.IsVisible = false;
            svHelperEditServices.IsVisible = true;

            helperServices = new HelperServiceModel();
            helperServices.Service = new List<HelperServices>();
        }

        void GoToBuildTrust()
        {
            //await (new HelpersServices()).get();
            //gridHelperBasic.BackgroundColor = Color.FromHex("#FF748C");
            //gridHelperEditServices.BackgroundColor = Color.FromHex("#FF748C");
            gridHelperBuildTrust.BackgroundColor = Color.FromHex("#FF748C");

            helperCompleteRegisterTitle.Text = "Helper Sign Up 3/3";
            svHelperBasicInfo.IsVisible = false;
            svHelperRegHelperHome.IsVisible = false;
            svHelperBuildTrust.IsVisible = true;
        }
    }
}