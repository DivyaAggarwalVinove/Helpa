using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Gms.Maps.Model;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Com.Google.Maps.Android.Clustering;
using Com.Google.Maps.Android.Clustering.View;
using Java.Lang;

namespace Helpa.Droid.Model
{
    public class CustomMarkerRenderer : DefaultClusterRenderer
    {
        protected CustomMarkerRenderer(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        protected override void OnBeforeClusterItemRendered(Java.Lang.Object p0, MarkerOptions p1)
        {
            base.OnBeforeClusterItemRendered(p0, p1);
        }

        protected override void OnBeforeClusterRendered(ICluster p0, MarkerOptions p1)
        {
            base.OnBeforeClusterRendered(p0, p1);
        }

        public override void SetOnClusterClickListener(ClusterManager.IOnClusterClickListener p0)
        {
            base.SetOnClusterClickListener(p0);
        }
    }
}