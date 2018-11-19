using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Foundation;
using Helpa;
using Helpa.iOS;
using MapKit;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Maps.iOS;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomMap), typeof(CustomMapRendererIos))]
namespace Helpa.iOS
{
    class CustomMapRendererIos : MapRenderer
    {
        CustomMap customMap;
        //List<CustomPin> customPins;
        protected override void OnElementChanged(ElementChangedEventArgs<View> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                var nativeMap = Control as MKMapView;
                if (nativeMap != null)
                {
                    nativeMap.RemoveAnnotations(nativeMap.Annotations);
                    nativeMap.GetViewForAnnotation = null;
                    //nativeMap.CalloutAccessoryControlTapped -= OnCalloutAccessoryControlTapped;
                    //nativeMap.DidSelectAnnotationView -= OnDidSelectAnnotationView;
                    //nativeMap.DidDeselectAnnotationView -= OnDidDeselectAnnotationView;
                }
            }

            if (e.NewElement != null)
            {
                if (e.NewElement != null)
                {
                    customMap = (CustomMap)e.NewElement;

                    var nativeMap = Control as MKMapView;

                    //customPins = customMap.CustomPins;

                    nativeMap.GetViewForAnnotation = GetViewForAnnotation;
                    //nativeMap.CalloutAccessoryControlTapped += OnCalloutAccessoryControlTapped;
                    //nativeMap.DidSelectAnnotationView += OnDidSelectAnnotationView;
                    //nativeMap.DidDeselectAnnotationView += OnDidDeselectAnnotationView;
                }
            }
        }

        protected override MKAnnotationView GetViewForAnnotation(MKMapView mapView, IMKAnnotation annotation)
        {
            MKAnnotationView annotationView = null;

            if (annotation is MKUserLocation)
                return null;


            //var customPin = GetCustomPin(annotation as MKPointAnnotation);
            //if (customPin == null)
            //{
            //    throw new Exception("Custom pin not found");
            //}
            MKPointAnnotation a = annotation as MKPointAnnotation;
            if(a !=null)
                annotationView = mapView.DequeueReusableAnnotation(a.GetTitle());
            //if (annotationView == null)
            //{
            //    annotationView = new CustomMKAnnotationView(annotation, customPin.Id.ToString());
            //    //annotationView.Image = UIImage.FromFile("pin.png");
            //    //annotationView.CalloutOffset = new CGPoint(0, 0);
            //    //annotationView.LeftCalloutAccessoryView = new UIImageView(UIImage.FromFile("monkey.png"));
            //    //annotationView.RightCalloutAccessoryView = UIButton.FromType(UIButtonType.DetailDisclosure);
            //    //((CustomMKAnnotationView)annotationView).Id = customPin.Id.ToString();
            //    //((CustomMKAnnotationView)annotationView).Url = customPin.Url;
            //}
            //annotationView.CanShowCallout = true;

            return annotationView;
        }

        //private object GetCustomPin(MKPointAnnotation mKPointAnnotation)
        //{

        //}
    }
}