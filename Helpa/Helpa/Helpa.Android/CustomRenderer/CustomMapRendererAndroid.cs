using Android.Content;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Graphics;
using Android.Views;
using Android.Widget;
using Helpa.Droid;
using Helpa;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.Android;
using Xamarin.Forms.Platform.Android;
using Com.Google.Maps.Android.Clustering;
using System.Linq;
using Java.Lang;
using Com.Google.Maps.Android.Clustering.View;
using System.Collections.Generic;
using Android.Support.V4.Content;
using Helpa.Models;

[assembly: ExportRenderer(typeof(CustomMap), typeof(CustomMapRendererAndroid))]
namespace Helpa.Droid
{
    class CustomMapRendererAndroid : MapRenderer, IOnMapReadyCallback,
        ClusterManager.IOnClusterClickListener, ClusterManager.IOnClusterItemClickListener,
        GoogleMap.IOnMarkerClickListener
    {
        CustomMap customMap;
        public CustomMapRendererAndroid(Context context) : base(context)
        {
        }
        
        protected override void OnElementChanged(ElementChangedEventArgs<Map> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                customMap = (CustomMap)e.NewElement;
                Control.GetMapAsync(this);
            }
        }

        protected override void OnMapReady(GoogleMap map)
        {
            base.OnMapReady(map);

            //SetupMapClusting(map);

            map.SetOnMarkerClickListener(this);

            //NativeMap.InfoWindowClick += OnInfoWindowClick;
           // NativeMap.SetInfoWindowAdapter(this);
        }

        // Declare a variable for the cluster manager.
        private ClusterManager mClusterManager;
        private void SetupMapClusting(GoogleMap map)
        {
            if (customMap.helperList != null)
            {

                var h = customMap.helperList.ElementAt(0);
                /************************************/
                CameraPosition.Builder builder = CameraPosition.InvokeBuilder();
                builder.Target(new LatLng(h.Latitude, h.Longitude));
                builder.Zoom(12f);
                CameraPosition cameraPosition = builder.Build();

                map.MoveCamera(CameraUpdateFactory.NewCameraPosition(cameraPosition));
                /********************************/

                // Position the map.
                //map.MoveCamera(CameraUpdateFactory.NewLatLngZoom(new LatLng(h.Latitude, h.Longitude), customMap.helperList.Count));

                // Initialize the manager with the context and the map.
                // (Activity extends context, so we can pass 'this' in the constructor.)
                mClusterManager = new ClusterManager(Context, map);

                // Point the map's listeners at the listeners implemented by the cluster
                // manager.
                map.SetOnCameraIdleListener(mClusterManager);
                map.SetOnMarkerClickListener(mClusterManager);

                // Add cluster items (markers) to the cluster manager.
                AddClusterItems(map, h);

               // mClusterManager.Cluster();
            }
            else
            {
                while (customMap.helperList == null) ;
            }
        }

        private void AddClusterItems(GoogleMap map, HelperHomeModel h)
        {
            var items = new List<MyItem>();

            // Add current location to the cluster list
            var currentMarker = new MarkerOptions();
            var me = new LatLng(h.Latitude, h.Longitude);
            currentMarker.SetPosition(me);
            var meMarker = new CircleOptions();
            meMarker.InvokeCenter(me);
            meMarker.InvokeRadius(32);
            meMarker.InvokeStrokeWidth(10);
            meMarker.InvokeFillColor(ContextCompat.GetColor(Context, Android.Resource.Color.HoloRedLight));
            map.AddCircle(meMarker);
            items.Add(new MyItem(h.Latitude, h.Longitude));

            // Create a log. spiral of markers to test clustering
            for (int i = 1; i < customMap.helperList.Count; i++)
            {
                var hi = customMap.helperList.ElementAt(i);
                var t = i * Math.Pi * 0.33f;
                var r = 0.005 * Math.Exp(0.1 * t);
                var x = r * Math.Cos(t);
                var y = r * Math.Sin(t);
                items.Add(new MyItem(hi.Latitude + x, hi.Longitude + y));
            }
            mClusterManager.AddItems(items);
            /*****************************************************************/

            // Set some lat/lng coordinates to start with.
            /*double lat = 51.5145160;
            double lng = -0.1270060;

            // Add ten cluster items in close proximity, for purposes of this example.
            for (int i = 1; i < customMap.helperList.Count; i++)
            {
                
                var h = customMap.helperList.ElementAt(i);
                double offset = i / 60d;
                lat = h.Latitude + offset;
                lng = h.Longitude + offset;
                MyItem offsetItem = new MyItem(lat, lng);
                mClusterManager.AddItem(offsetItem);
                
            }*/
        }

        public bool OnMarkerClick(Marker marker)
        {
            string selectedCluster = marker.Title.Substring(0, marker.Title.Length - 4).Trim();
            customMap.ClickedOnPin(selectedCluster);
            return true; //disables info window from showing but now I can't check for clicks on the map and get the data from the marker
        }

        protected override MarkerOptions CreateMarker(Pin pin)
        {
            var marker = new MarkerOptions();
            marker.SetPosition(new LatLng(pin.Position.Latitude, pin.Position.Longitude));
            marker.SetTitle(pin.Label);
            marker.SetSnippet(pin.Address);

            Android.Views.View v = null;
            LayoutInflater inflater = (LayoutInflater)Context.GetSystemService(Context.LayoutInflaterService);
            if (customMap.selectedHelper!=null && customMap.selectedHelper.LocationType == 'S')
                v = inflater.Inflate(Resource.Layout.Home, null);
            else if (customMap.selectedHelper != null && customMap.selectedHelper.LocationType == 'M')
                v = inflater.Inflate(Resource.Layout.Mobile, null);
            else
                v = inflater.Inflate(Resource.Layout.Job, null);

            TextView tv = v.FindViewById<TextView>(Resource.Id.tv);
            tv.Text = pin.Label;

            marker.SetIcon(BitmapDescriptorFactory.FromBitmap(LoadBitmapFromView(v)));
            
            return marker;
        }

        public static Bitmap LoadBitmapFromView(Android.Views.View v)
        {
            if (v.MeasuredHeight <= 0)
            {
                v.Measure(50, 50);
                Bitmap b = Bitmap.CreateBitmap(v.MeasuredWidth, v.MeasuredHeight, Bitmap.Config.Argb8888);
                Canvas c = new Canvas(b);
                v.Layout(0, 0, v.MeasuredWidth, v.MeasuredHeight);
                v.Draw(c);

                return b;
            }

            return null;
        }
        
        public bool OnClusterItemClick(Object p0)
        {
            return false;
        }

        public bool OnClusterClick(ICluster p0)
        {
            return false;
        }

        /*void OnInfoWindowClick(object sender, GoogleMap.InfoWindowClickEventArgs e)
        {
            //var customPin = GetCustomPin(e.Marker);
            
            //var customPin = CustomMap.Pins;
            //if (customPin == null)
            //{
            //    throw new Exception("Custom pin not found");
            //}

            //if (!string.IsNullOrWhiteSpace(customPin.Url))
            //{
            //    var url = Android.Net.Uri.Parse(customPin.Url);
            //    var intent = new Intent(Intent.ActionView, url);
            //    intent.AddFlags(ActivityFlags.NewTask);
            //    Android.App.Application.Context.StartActivity(intent);
            //}
        } */

        /*public Android.Views.View GetInfoContents(Marker marker)
        {
            //var inflater = Android.App.Application.Context.GetSystemService(Context.LayoutInflaterService) as Android.Views.LayoutInflater;
            //if (inflater != null)
            //{
            //    Android.Views.View view = null;

            //    //var customPin = GetCustomPin(marker);
            //    var customPin = CustomMap.Pins;
            //    if (customPin == null)
            //    {
            //        throw new Exception("Custom pin not found");
            //    }

            //    if (customPin.Id.ToString() == "Xamarin")
            //    {
            //        //view = inflater.Inflate(Resource.Layout.XamarinMapInfoWindow, null);
            //    }
            //    else
            //    {
            //        //view = inflater.Inflate(Resource.Layout.MapInfoWindow, null);
            //    }

            //    var infoTitle = view.FindViewById<TextView>(Resource.Id.InfoWindowTitle);
            //    var infoSubtitle = view.FindViewById<TextView>(Resource.Id.InfoWindowSubtitle);

            //    if (infoTitle != null)
            //    {
            //        infoTitle.Text = marker.Title;
            //    }
            //    if (infoSubtitle != null)
            //    {
            //        infoSubtitle.Text = marker.Snippet;
            //    }

            //    return view;
            //}
            //return null;
            throw new System.NotImplementedException();
        } */

        /*public Android.Views.View GetInfoWindow(Marker marker)
        {
            throw new System.NotImplementedException();
        }*/
    }
}