using Android.Content;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Graphics;
using Android.Views;
using Android.Widget;
using Helpa.Droid;
using Helpa.Models;
using Helpa;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.Android;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomMap), typeof(CustomMapRendererAndroid))]
namespace Helpa.Droid
{
    class CustomMapRendererAndroid : MapRenderer, GoogleMap.IOnMarkerClickListener
    {
        //List<string> siteList;
        CustomMap customMap;
        public CustomMapRendererAndroid(Context context) : base(context)
        {
        }
        
        protected override void OnElementChanged(ElementChangedEventArgs<Map> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                //NativeMap.InfoWindowClick -= OnInfoWindowClick;
            }

            if (e.NewElement != null)
            {
                customMap = (CustomMap)e.NewElement;
                //siteList = formsMap.SiteList;
                Control.GetMapAsync(this);
            }
        }

        protected override void OnMapReady(GoogleMap map)
        {
            base.OnMapReady(map);

            map.SetOnMarkerClickListener(this);
            //NativeMap.InfoWindowClick += OnInfoWindowClick;
            //NativeMap.SetInfoWindowAdapter(this);
        }

        public bool OnMarkerClick(Marker marker)
        {
            HelperHomeModel helper = new HelperHomeModel();

            customMap.selectedHelper = helper;
            customMap.ClickedOnPin();

            return true; //disables info window from showing but now I can't check for clicks on the map and get the data from the marker
        }

        protected override MarkerOptions CreateMarker(Pin pin)
        {
            var marker = new MarkerOptions();
            marker.SetPosition(new LatLng(pin.Position.Latitude, pin.Position.Longitude));
            marker.SetTitle(pin.Label);
            marker.SetSnippet(pin.Address);

            LayoutInflater inflater = (LayoutInflater)Context.GetSystemService(Context.LayoutInflaterService);
            Android.Views.View v = inflater.Inflate(Resource.Layout.Main, null);

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