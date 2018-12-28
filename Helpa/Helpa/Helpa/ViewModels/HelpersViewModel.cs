using Helpa.Models;
using Helpa.Services;
using Plugin.Geolocator;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace Helpa.ViewModels
{
    public class HelpersViewModel : INotifyPropertyChanged
    {
        //public IHelpersServices<HelperHomeModel> DataStore => DependencyService.Get<IHelpersServices<HelperHomeModel>>();
        public ObservableCollection<HelperHomeModel> helperHomeList { get; set; }

        public ObservableCollection<HelperHomeModel> helperHomeFilterList { get; set; }

        public ObservableCollection<HelperHome> HelperFullList { get; set; }

        ObservableCollection<HelperHome> helperHalfList=new ObservableCollection<HelperHome>();
        public ObservableCollection<HelperHome> HelperHalfList
        {
            get
            {
                return helperHalfList;
            }
            set { SetProperty(ref helperHalfList, value); }
        }
        public Command LoadItemsCommand { get; set; }

        public CustomMap mapHelper;
        public Helpers  helperInstance;
        public ActivityIndicator activityIndicator;

        public HelpersViewModel(Helpers helpers, CustomMap map)
        {
            try
            {
                // activityIndicator = Helpers.Instance.Content.FindByName<ActivityIndicator>("aiFindHelper");
                activityIndicator = helpers.activityIndicator;
                mapHelper = map;
                helperInstance = helpers;

                LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
                LoadItemsCommand.Execute(null);
            }
            catch(Exception e)
            {
                Console.Write(e.Message);
            }
        }

        public async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            
            try
            {
                HelperFullList = new ObservableCollection<HelperHome>();
                HelperHalfList = new ObservableCollection<HelperHome>();

                activityIndicator.IsRunning = true;
                HelpersServices helpersServices = new HelpersServices();
                var h = await helpersServices.GetHelpersList();
                if (h != null && (h.Count() > 0))
                {
                    mapHelper.helperList = h.ToList();
                    helperHomeList = new ObservableCollection<HelperHomeModel>(h.ToList());
                    helperHomeFilterList = helperHomeList;
                }
                
                //var HelperList = new ObservableCollection<HelperHome>(((new HelpersServices()).GetHelpersList(2000000, 28.4514279, 77.0704678)).Result.First().HelpersInLocalties);
                //for(int i=0;i<helperHomeList.Count;i++)
                //{
                //    helperHomeList.ElementAt(i).LocationType = 'S';
                //}
                
                #region Check Location Permission

                try
                {
                    var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);
                    if (status != PermissionStatus.Granted)
                    {
                        if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Location))
                        {
                            await helperInstance.DisplayAlert("Need location", "Helpa need that location", "OK");
                            return;
                        }

                        //var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Location);
                        await DependencyService.Get<IPermissionServices>().GetPermission(helperInstance);

                        //if (results.ContainsKey(Permission.Location))
                        //    status = results[Permission.Location];
                    }

                    if (status == PermissionStatus.Granted)
                    {
                        await DependencyService.Get<IPermissionServices>().CheckAndTurnOnGPS(helperInstance);

                        SetLocationOnMap(helperHomeList);
                    }
                    else if (status != PermissionStatus.Unknown)
                    {
                        await helperInstance.DisplayAlert("Location Denied", "Please allow location permission to get your location for Helpa app in Device setting", "OK");
                    }
                }
                catch (Exception ex)
                {
                    await helperInstance.DisplayAlert("Error", ex.Message, "OK");
                }

                /**********************************************************************/

                //var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);
                //if (status == PermissionStatus.Granted)
                /*{
                    ProgressBar pb = new ProgressBar();
                    
                    activityIndicator.IsRunning = true;

                    //var position = await CrossGeolocator.Current.GetPositionAsync();
                    var position = helperHomeList.ElementAt(0);
                    mapHelper.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(position.Latitude, position.Longitude), Distance.FromKilometers(1)));
                    mapHelper.IsShowingUser = true;
                    mapHelper.MapType = MapType.Street;

                    activityIndicator.IsRunning = false;

                    #region Call api to get data from server
                    // var helpers = await DataStore.GetHelpersList(10000, 28.4514279, 77.0704678);
                    // var helpers = await (new HelpersServices()).GetHelpersList(2000000, position.Latitude, position.Longitude);
                    // HelperList = new ObservableCollection<HelperHome>(helpers.First().HelpersInLocalties);
                    #endregion
                }*/
                #endregion
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public void SetLocationOnMap(ObservableCollection<HelperHomeModel> helperHomeList)
        {
            try
            {
                int i = 0;
                var t = "0";
                mapHelper.Pins.Clear();

                foreach (HelperHomeModel h in helperHomeList)
                {
                    mapHelper.selectedHelper = helperHomeList.ElementAt(i);
                    var pin = new Pin()
                    {
                        Type = PinType.Place,
                        Position = new Position(h.Latitude, h.Longitude),
                        Label = h.LocationName + " (" + h.Count + ")",
                        Address = i.ToString(),
                        Id = i.ToString()
                    };
                    mapHelper.Pins.Add(pin);
                    
                    t = (int.Parse(t) + h.Count).ToString();
                    i++;
                }

                var position = helperHomeList.ElementAt(0);
                
                mapHelper.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(position.Latitude, position.Longitude), Distance.FromKilometers(1)));
                mapHelper.IsShowingUser = true;
                mapHelper.MapType = MapType.Street;

                TotalHelpers = t;
            }
            catch (Exception e)
            {
                Debug.Write(e.Message);
            }

            activityIndicator.IsRunning = false;
        }

        string totalHelpers = "?";
        public string TotalHelpers
        {
            get
            {
                return totalHelpers;
            }
            set
            {
                SetProperty(ref totalHelpers, value);
            }
        }

        bool isBusy = false;
        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        protected bool SetProperty<T>(ref T backingStore, T value,
           [CallerMemberName]string propertyName = "",
           Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
