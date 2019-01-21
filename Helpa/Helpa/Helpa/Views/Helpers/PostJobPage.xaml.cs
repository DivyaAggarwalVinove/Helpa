using AsNum.XFControls;
using DurianCode.PlacesSearchBar;
using Helpa.Models;
using System.Linq;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections;
using System;
using Helpa.Services;
using Helpa.Utility;

namespace Helpa.Views.Helpers
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PostJobPage : ContentPage
    {
        private IList<ScopeModel> scopes;
        private IList<ServiceModel> services;
        public RegisterUserModel loggedUser;
        private int selectedService;
        public string selectedLocation;
        public AutoCompletePrediction selectedPrediction;

        public PostJobPage()
        {
            InitializeComponent();

            NavigationPage.SetHasNavigationBar(this, false);

            SetRadioList(new List<string>()
            {
                "Normal",
                "Urgent"
            }, rgPJJobStatus);

            //SetRadioList(new List<String>()
            //{
            //    "Helper's Home",
            //    "Mobile Helper"
            //}, rgPJHelperType);

            SetServices();
        }

        private void SetRadioList(IEnumerable<string> genderList, RadioGroup radioGroup)
        {
            radioGroup.ItemsSource = (IEnumerable)genderList;
            radioGroup.SelectedItem = (object)genderList.ElementAt<string>(0);
            StackLayout content = (StackLayout)radioGroup.Content;
            for (int index = 0; index < content.Children.Count; ++index)
            {
                Radio child = (Radio)content.Children[index];
                child.Margin = new Thickness(0.0, 0.0, 25.0, 0.0);
                child.VerticalOptions = LayoutOptions.Center;
            }
        }

        private async void SetServices()
        {
            IList<ServiceModel> servicesAsync = await new Utilities().GetServicesAsync();
            services = servicesAsync;
            int num1 = services.Count();

            for (int index = 0; index < (num1 - 1) / 3 + 1; ++index)
                gridPJServices.RowDefinitions.Add(new RowDefinition()
                {
                    Height = GridLength.Auto
                });

            for (int index = 0; index < 3; ++index)
                gridPJServices.ColumnDefinitions.Add(new ColumnDefinition()
                {
                    Width = GridLength.Star
                });
            gridPJServices.Padding = new Thickness(0.0, 0.0, 10.0, 0.0);

            for (int index = 0; index < num1; ++index)
            {
                Button button = new Button();
                button.BackgroundColor = Color.FromHex("F1F5F6");
                button.CornerRadius = 3;
                button.BorderColor = Color.FromHex("BCC1C4");
                button.BorderWidth = 1.0;
                button.HorizontalOptions = LayoutOptions.FillAndExpand;
                button.Margin = new Thickness(5.0, 0.0);
                button.TextColor = Color.FromHex("BCC1C4");
                button.Text = services.ElementAt(index).ServiceName;

                button.Clicked += (sender, e) =>
                 {
                     Button btn = (Button)sender;

                     if (btn.BorderColor == Color.FromHex("BCC1C4"))
                     {
                         Grid grid = (Grid)btn.Parent;

                         // Unselect previous selected Service
                         Button btnSelectedService = (Button)grid.Children.ElementAt(selectedService * 2);
                         btnSelectedService.BorderColor = Color.FromHex("BCC1C4");
                         btnSelectedService.TextColor = Color.FromHex("BCC1C4");

                         Image imgSelected = (Image)grid.Children.ElementAt(selectedService * 2 + 1);
                         imgSelected.IsVisible = false;

                         // if (btnSelectedService.Text.Equals("Child Care"))
                         if(btnSelectedService.Text.ToLower().Contains("child"))
                         {
                             for (int i = gridChildCare.Children.Count - 2; i >= 0; i--)
                             {
                                 ChildView content = (ChildView)gridChildCare.Children[i];
                                 gridChildCare.Children.Remove(content);
                                 gridChildCare.RowDefinitions.RemoveAt(i);
                             }

                             gridChildCare.RowDefinitions.RemoveAt(0);
                             btnAddMoreChild.IsVisible = false;
                         }

                         // Select Service
                         btn.BorderColor = Color.FromHex("FE7890");
                         btn.TextColor = Color.FromHex("FE7890");

                         selectedService = services.IndexOf(services.Where(x => x.ServiceName == btn.Text).FirstOrDefault());
                         Image img = (Image)grid.Children.ElementAt(selectedService * 2 + 1);
                         img.IsVisible = true;

                         SetScopes(services.ElementAt(selectedService).Id);

                         //if (btn.Text.Equals("Child Care"))
                         if(btn.Text.ToLower().Contains("child"))
                         {
                             btnAddMoreChild.IsVisible = true;
                             
                             gridChildCare.RowDefinitions.Add(new RowDefinition()
                             {
                                 Height = GridLength.Auto
                             });

                             gridChildCare.RowDefinitions.Add(new RowDefinition()
                             {
                                 Height = GridLength.Auto
                             });


                             gridChildCare.Children.Remove(btnAddMoreChild);

                             ChildView cv = new ChildView();
                             gridChildCare.Children.Add(cv, 0, 0);
                             gridChildCare.Children.Add(btnAddMoreChild, 0, 1);
                         }
                     }
                 };

                Image image = new Image();
                image.Source = "selected.png";
                image.Aspect = Aspect.AspectFit;
                image.Margin = new Thickness(15.0, 0.0, -5.0, 0.0);
                image.HorizontalOptions = LayoutOptions.End;
                image.IsVisible = false;
                gridPJServices.Children.Add((View)button, index % 3, index / 3);
                gridPJServices.Children.Add((View)image, index % 3, index / 3);
            }

            SetScopes(services.ElementAt(selectedService).Id);
            ((Button)gridPJServices.Children[0]).BorderColor = Color.FromHex("FE7890");
            ((Button)gridPJServices.Children[0]).TextColor = Color.FromHex("FE7890");
            gridPJServices.Children[1].IsVisible = true;
            App.Database.DeleteService();
            int num2 = App.Database.SaveServices((IEnumerable<ServiceModel>)services);
        }

        private void ClickOnAddMoreChild(object sender, EventArgs e)
        {
            gridChildCare.RowDefinitions.Add(new RowDefinition()
            {
                Height = GridLength.Auto
            });
            gridChildCare.Children.Remove(btnAddMoreChild);

            ChildView cv = new ChildView();
            gridChildCare.Children.Add(cv, 0, gridChildCare.RowDefinitions.Count - 2);
            gridChildCare.Children.Add(btnAddMoreChild, 0, gridChildCare.RowDefinitions.Count - 1);
        }

        private void RemoveScope()
        {
            foreach (View view in this.gridPJScopes.Children.ToList<View>())
                gridPJScopes.Children.Remove(view);

            gridPJScopes.RowDefinitions = new RowDefinitionCollection();
            gridPJScopes.ColumnDefinitions = new ColumnDefinitionCollection();
        }

        private async void SetScopes(int selectedServiceId)
        {
            scopes = await (new Utilities().GetScpoesAsync(selectedServiceId));
            RemoveScope();

            int num1 = scopes.Count();
            for (int index = 0; index < (num1 - 1) / 2 + 1; ++index)
                gridPJScopes.RowDefinitions.Add(new RowDefinition()
                {
                    Height = GridLength.Auto
                });
            for (int index = 0; index < 2; ++index)
                gridPJScopes.ColumnDefinitions.Add(new ColumnDefinition()
                {
                    Width = GridLength.Star
                });
            gridPJScopes.Padding = new Thickness(0.0, 0.0, 10.0, 0.0);

            for (int index = 0; index < num1; ++index)
            {
                Button button1 = new Button();
                button1.BackgroundColor = Color.FromHex("F1F5F6");
                button1.CornerRadius = 3;
                button1.BorderColor = Color.FromHex("BCC1C4");
                button1.BorderWidth = 1.0;
                button1.HorizontalOptions = LayoutOptions.FillAndExpand;
                button1.Margin = new Thickness(5.0, 0.0);
                button1.TextColor = Color.FromHex("BCC1C4");
                button1.Text = this.scopes.ElementAt<ScopeModel>(index).ScopeName;
                button1.Clicked += (EventHandler)((sender, e) =>
                {
                    Button button = (Button)sender;
                    if (button.BorderColor == Color.FromHex("BCC1C4"))
                    {
                        button.BorderColor = Color.FromHex("FE7890");
                        button.TextColor = Color.FromHex("FE7890");
                    }
                    else
                    {
                        button.BorderColor = Color.FromHex("BCC1C4");
                        button.TextColor = Color.FromHex("BCC1C4");
                    }
                });
                this.gridPJScopes.Children.Add((View)button1, index % 2, index / 2);
            }
            App.Database.DeleteService();
            int num2 = App.Database.SaveServices((IEnumerable<ServiceModel>)this.services);
        }

        private void OnHomeLocationSelected(object sender, EventArgs args)
        {
            if (this.cbPJHelperHome.Checked)
            {
                this.cbPJMobileHome.Checked = !this.cbPJMobileHome.Checked;
                this.btnPJHelperDistrict.IsVisible = false;
                this.entryPJLocation.IsVisible = true;
            }
            else
                this.cbPJHelperHome.Checked = !this.cbPJHelperHome.Checked;
        }

        private void OnMobileLocationSelected(object sender, EventArgs args)
        {
            if (this.cbPJMobileHome.Checked)
            {
                this.cbPJHelperHome.Checked = !this.cbPJHelperHome.Checked;
                this.btnPJHelperDistrict.IsVisible = true;
                this.entryPJLocation.IsVisible = false;
            }
            else
                this.cbPJMobileHome.Checked = !this.cbPJMobileHome.Checked;
        }

        private void OnClickHourly(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            if (button.BorderColor == Color.FromHex("BCC1C4"))
            {
                button.BorderColor = Color.FromHex("FE7890");
                button.TextColor = Color.FromHex("FE7890");
                this.btnPJPriceTbd.BorderColor = Color.FromHex("BCC1C4");
                this.btnPJPriceTbd.TextColor = Color.FromHex("BCC1C4");
                this.gridPJPriceHour.IsVisible = true;
            }
            else
            {
                button.BorderColor = Color.FromHex("BCC1C4");
                button.TextColor = Color.FromHex("BCC1C4");
                this.gridPJPriceHour.IsVisible = false;
                if (!(this.btnPJPriceHourly.BorderColor == Color.FromHex("BCC1C4")) || !(this.btnPJPriceMonthly.BorderColor == Color.FromHex("BCC1C4")) || !(this.btnPJPriceDaily.BorderColor == Color.FromHex("BCC1C4")))
                    return;
                this.btnPJPriceTbd.BorderColor = Color.FromHex("FE7890");
                this.btnPJPriceTbd.TextColor = Color.FromHex("FE7890");
            }
        }

        private void OnClickDaily(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            if (button.BorderColor == Color.FromHex("BCC1C4"))
            {
                button.BorderColor = Color.FromHex("FE7890");
                button.TextColor = Color.FromHex("FE7890");
                this.btnPJPriceTbd.BorderColor = Color.FromHex("BCC1C4");
                this.btnPJPriceTbd.TextColor = Color.FromHex("BCC1C4");
                this.gridPJPriceDay.IsVisible = true;
            }
            else
            {
                button.BorderColor = Color.FromHex("BCC1C4");
                button.TextColor = Color.FromHex("BCC1C4");
                this.gridPJPriceDay.IsVisible = false;
                if (!(this.btnPJPriceHourly.BorderColor == Color.FromHex("BCC1C4")) || !(this.btnPJPriceMonthly.BorderColor == Color.FromHex("BCC1C4")) || !(this.btnPJPriceDaily.BorderColor == Color.FromHex("BCC1C4")))
                    return;
                this.btnPJPriceTbd.BorderColor = Color.FromHex("FE7890");
                this.btnPJPriceTbd.TextColor = Color.FromHex("FE7890");
            }
        }

        private void OnClickMonthly(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            if (button.BorderColor == Color.FromHex("BCC1C4"))
            {
                button.BorderColor = Color.FromHex("FE7890");
                button.TextColor = Color.FromHex("FE7890");
                this.btnPJPriceTbd.BorderColor = Color.FromHex("BCC1C4");
                this.btnPJPriceTbd.TextColor = Color.FromHex("BCC1C4");
                this.gridPJPriceMonth.IsVisible = true;
            }
            else
            {
                button.BorderColor = Color.FromHex("BCC1C4");
                button.TextColor = Color.FromHex("BCC1C4");
                this.gridPJPriceMonth.IsVisible = false;
                if (!(this.btnPJPriceHourly.BorderColor == Color.FromHex("BCC1C4")) || !(this.btnPJPriceMonthly.BorderColor == Color.FromHex("BCC1C4")) || !(this.btnPJPriceDaily.BorderColor == Color.FromHex("BCC1C4")))
                    return;
                this.btnPJPriceTbd.BorderColor = Color.FromHex("FE7890");
                this.btnPJPriceTbd.TextColor = Color.FromHex("FE7890");
            }
        }

        private void OnClickTBD(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            if (!(button.BorderColor == Color.FromHex("BCC1C4")))
                return;
            button.BorderColor = Color.FromHex("FE7890");
            button.TextColor = Color.FromHex("FE7890");
            this.btnPJPriceDaily.BorderColor = Color.FromHex("BCC1C4");
            this.btnPJPriceDaily.TextColor = Color.FromHex("BCC1C4");
            this.gridPJPriceDay.IsVisible = false;
            this.btnPJPriceHourly.BorderColor = Color.FromHex("BCC1C4");
            this.btnPJPriceHourly.TextColor = Color.FromHex("BCC1C4");
            this.gridPJPriceHour.IsVisible = false;
            this.btnPJPriceMonthly.BorderColor = Color.FromHex("BCC1C4");
            this.btnPJPriceMonthly.TextColor = Color.FromHex("BCC1C4");
            this.gridPJPriceMonth.IsVisible = false;
        }

        private void SetMinHour(object sender, EventArgs args)
        {
            this.btnPJPriceHrMin.Text = "$ " + ((Xamarin.RangeSlider.Forms.RangeSlider)sender).LowerValue.ToString();
        }

        private void SetMaxHour(object sender, EventArgs args)
        {
            this.btnPJPriceHrMax.Text = "$ " + ((Xamarin.RangeSlider.Forms.RangeSlider)sender).UpperValue.ToString();
        }

        private void SetMinDay(object sender, EventArgs args)
        {
            this.btnPJPriceDayMin.Text = "$ " + ((Xamarin.RangeSlider.Forms.RangeSlider)sender).LowerValue.ToString();
        }

        private void SetMaxDay(object sender, EventArgs args)
        {
            this.btnPJPriceDayMax.Text = "$ " + ((Xamarin.RangeSlider.Forms.RangeSlider)sender).UpperValue.ToString();
        }

        private void SetMinMonth(object sender, EventArgs args)
        {
            this.btnPJPriceMonthMin.Text = "$ " + ((Xamarin.RangeSlider.Forms.RangeSlider)sender).LowerValue.ToString();
        }

        private void SetMaxMonth(object sender, EventArgs args)
        {
            this.btnPJPriceMonthMax.Text = "$ " + ((Xamarin.RangeSlider.Forms.RangeSlider)sender).UpperValue.ToString();
        }

        private void OnClickSun(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            if (button.BorderColor == Color.FromHex("BCC1C4"))
            {
                button.TextColor = Color.FromHex("FE7890");
                button.BorderColor = Color.FromHex("FE7890");
            }
            else
            {
                button.TextColor = Color.FromHex("BCC1C4");
                button.BorderColor = Color.FromHex("BCC1C4");
            }
        }

        private void OnClickMon(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            if (button.BorderColor == Color.FromHex("BCC1C4"))
            {
                button.TextColor = Color.FromHex("FE7890");
                button.BorderColor = Color.FromHex("FE7890");
            }
            else
            {
                button.TextColor = Color.FromHex("BCC1C4");
                button.BorderColor = Color.FromHex("BCC1C4");
            }
        }

        private void OnClickTue(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            if (button.BorderColor == Color.FromHex("BCC1C4"))
            {
                button.TextColor = Color.FromHex("FE7890");
                button.BorderColor = Color.FromHex("FE7890");
            }
            else
            {
                button.TextColor = Color.FromHex("BCC1C4");
                button.BorderColor = Color.FromHex("BCC1C4");
            }
        }

        private void OnClickWed(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            if (button.BorderColor == Color.FromHex("BCC1C4"))
            {
                button.TextColor = Color.FromHex("FE7890");
                button.BorderColor = Color.FromHex("FE7890");
            }
            else
            {
                button.TextColor = Color.FromHex("BCC1C4");
                button.BorderColor = Color.FromHex("BCC1C4");
            }
        }

        private void OnClickThu(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            if (button.BorderColor == Color.FromHex("BCC1C4"))
            {
                button.TextColor = Color.FromHex("FE7890");
                button.BorderColor = Color.FromHex("FE7890");
            }
            else
            {
                button.TextColor = Color.FromHex("BCC1C4");
                button.BorderColor = Color.FromHex("BCC1C4");
            }
        }

        private void OnClickFri(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            if (button.BorderColor == Color.FromHex("BCC1C4"))
            {
                button.TextColor = Color.FromHex("FE7890");
                button.BorderColor = Color.FromHex("FE7890");
            }
            else
            {
                button.TextColor = Color.FromHex("BCC1C4");
                button.BorderColor = Color.FromHex("BCC1C4");
            }
        }

        private void OnClickSat(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            if (button.BorderColor == Color.FromHex("BCC1C4"))
            {
                button.TextColor = Color.FromHex("FE7890");
                button.BorderColor = Color.FromHex("FE7890");
            }
            else
            {
                button.TextColor = Color.FromHex("BCC1C4");
                button.BorderColor = Color.FromHex("BCC1C4");
            }
        }

        private void OnDescriptionChange(object sender, TextChangedEventArgs e)
        {
            this.lblDescCount.Text = ((Editor)sender).Text.Length.ToString() + "/300";
        }

        private void OnFocus(object sender, FocusEventArgs e)
        {
            Navigation.PushAsync(new LocationPage(this)
            {
                selectedLoc = entryPJLocation.Text
            });
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (selectedLocation == null)
                return;

            entryPJLocation.Text = selectedLocation;
        }

        private void OnBackPress(object sender, EventArgs e)
        {
            App.NavigationPage.Navigation.PopAsync();
        }

        private void SetStartTime(object sender, EventArgs e)
        {
            int startTime = (int) rsPJTime.LowerValue;

            int startTimeHour = startTime / 60;
            int startTimeMinute = startTime % 60;

            if (startTimeHour.ToString().Length == 1)
                btnPJStartTime.Text = "0" + startTimeHour;
            else
                btnPJStartTime.Text = startTimeHour.ToString();

            if (startTimeMinute.ToString().Length == 1)
                btnPJStartTime.Text = btnPJStartTime.Text + "0" + startTimeMinute;
            else
                btnPJStartTime.Text = btnPJStartTime.Text + startTimeMinute;
        }

        private void SetEndTime(object sender, EventArgs e)
        {
            int endTime = (int)rsPJTime.UpperValue;

            int endTimeHour = endTime / 60;
            int endTimeMinute = endTime % 60;

            if (endTimeHour.ToString().Length == 1)
                btnPJEndTime.Text = "0" + endTimeHour;
            else
                btnPJEndTime.Text = endTimeHour.ToString();

            if (endTimeMinute.ToString().Length == 1)
                btnPJEndTime.Text = btnPJEndTime.Text + "0" + endTimeMinute;
            else
                btnPJEndTime.Text = btnPJEndTime.Text + endTimeMinute;
        }

        private async void OnClickPostJob(object sender, EventArgs e)
        {
            aiPostJob.IsRunning = true;
            JobPostModel jpm = new JobPostModel();
            string text = entryPJTitle.Text;
            if (text == null || text.Equals(""))
            {
                await DisplayAlert("", "Please enter Job Title.", "OK");
                aiPostJob.IsRunning = false;
            }
            else
            {
                jpm.JobTitle = text;
                string l = entryPJLocation.Text;
                if (l == null || l.Equals(""))
                {
                    await DisplayAlert("", "Please enter Job Location.", "OK");
                    aiPostJob.IsRunning = false;
                }
                else
                {
                    try
                    {
                        Place place = await Places.GetPlace(selectedPrediction.Place_ID, Constants.googlePlaceApiKey);
                        jpm.Location = new List<LocationModel>()
                        {
                            new LocationModel()
                             {
                               LocationName = l,
                               Latitude = place.Latitude.ToString(),
                               Longitude = place.Longitude.ToString()
                             }
                        };
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.StackTrace);
                    }

                    jpm.JobType = !rgPJJobStatus.SelectedItem.ToString().ToLower().Equals("normal") ? "U" : "N";

                    ServiceModel serviceModel = services.ElementAt<ServiceModel>(selectedService);
                    jpm.JobServicesId = serviceModel.Id;
                    jpm.JobServicesName = serviceModel.ServiceName;

                    //if (serviceModel.ServiceName.Equals("Child Care"))
                    if (serviceModel.ServiceName.ToLower().Contains("child"))
                    {
                        jpm.Receivers = new List<Receiver>();

                        for (int i = 0; i < gridChildCare.Children.Count - 1; i++)
                        {
                            ChildView content = (ChildView)gridChildCare.Children[i];
                            Grid grid = (Grid)content.Children.ElementAt(0);
                            RadioGroup radioGroup = (RadioGroup)grid.Children.ElementAt(1);
                            Button button = (Button)grid.Children.ElementAt(3);
                            int gender = (new List<string>()
                                          {
                                            "Male",
                                            "Female",
                                            "N/A"
                                          }).IndexOf(radioGroup.SelectedItem.ToString());
                            jpm.Receivers.Add(new Receiver() { ReceiverAge = (int) float.Parse(button.Text), ReceiverGender = gender+1 });
                        }
                    }

                    jpm.HelperType = !cbPJHelperHome.Checked ? "M" : "S";

                    if (btnPJPriceTbd.BorderColor == Color.FromHex("BCC1C4"))
                    {
                        if (btnPJPriceDaily.BorderColor == Color.FromHex("FE7890"))
                        {
                            jpm.IsDaily = true;
                            jpm.MinDailyPrice = btnPJPriceDayMin.Text.Substring(2);
                            jpm.MaxDailyPrice = btnPJPriceDayMax.Text.Substring(2);
                        }
                        if (btnPJPriceMonthly.BorderColor == Color.FromHex("FE7890"))
                        {
                            jpm.IsMonthly = true;
                            jpm.MinMonthlyPrice = btnPJPriceMonthMin.Text.Substring(2);
                            jpm.MaxMonthlyPrice = btnPJPriceMonthMax.Text.Substring(2);
                        }
                        if (btnPJPriceHourly.BorderColor == Color.FromHex("FE7890"))
                        {
                            jpm.IsHourly = true;
                            jpm.MinHourlyPrice = btnPJPriceHrMin.Text.Substring(2);
                            jpm.MaxHourlyPrice = btnPJPriceHrMax.Text.Substring(2);
                        }
                    }
                    if (entryPJDescription.Text != null)
                        jpm.JobDescription = entryPJDescription.Text;
                    if (btnPJSun.BorderColor == Color.FromHex("FE7890"))
                        jpm.IsSunday = true;
                    if (btnPJMon.BorderColor == Color.FromHex("FE7890"))
                        jpm.IsMonday = true;
                    if (btnPJTue.BorderColor == Color.FromHex("FE7890"))
                        jpm.IsTuesday = true;
                    if (btnPJWed.BorderColor == Color.FromHex("FE7890"))
                        jpm.IsWednesday = true;
                    if (btnPJThu.BorderColor == Color.FromHex("FE7890"))
                        jpm.IsThursday = true;
                    if (btnPJFri.BorderColor == Color.FromHex("FE7890"))
                        jpm.IsFriday = true;
                    if (btnPJSat.BorderColor == Color.FromHex("FE7890"))
                        jpm.IsSaturday = true;

                    //jpm.StartTime = rsPJTime.LowerValue / 60 + ":" + rsPJTime.LowerValue % 60 + ":00";
                    //jpm.EndTime = rsPJTime.UpperValue / 60 + ":" + rsPJTime.UpperValue % 60 + ":00";
                    jpm.StartTime = btnPJStartTime.Text.Substring(0, 2) + ":" + btnPJStartTime.Text.Substring(2, 2) + ":00";
                    jpm.EndTime = btnPJEndTime.Text.Substring(0, 2) + ":" + btnPJEndTime.Text.Substring(2, 2) + ":00";

                    jpm.JobScope = new List<JobScopes>();
                    foreach (Button button in gridPJScopes.Children.ToList())
                    {
                        if (button.BorderColor == Color.FromHex("FE7890"))
                        {
                            string scope = button.Text;
                            var s = scopes.Where((x => x.ScopeName == scope)).FirstOrDefault();
                            jpm.JobScope.Add(new JobScopes() { ScopeId = s.Id.ToString(), ScopeName = s.ScopeName });
                        }
                    }
                    jpm.UserId = loggedUser.Id;
                    int num = await new JobServices().SaveJob(jpm) ? 1 : 0;

                    aiPostJob.IsRunning = false;
                    if (num != 0)
                    {
                        await DisplayAlert("", "Job Post successfully.", "OK");
                        Page page = await Navigation.PopAsync();
                    }
                    else
                        await DisplayAlert("", "Job not post. Please try again.", "OK");
                }
            }
        }
    }
}