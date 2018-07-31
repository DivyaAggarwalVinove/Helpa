package md5e3eee4beccacbfcc7b14c464d824a5e7;


public class CustomMapRendererAndroid
	extends md55b943cb46934695d066180d3c9f40d32.MapRenderer
	implements
		mono.android.IGCUserPeer,
		com.google.android.gms.maps.GoogleMap.OnMarkerClickListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onMarkerClick:(Lcom/google/android/gms/maps/model/Marker;)Z:GetOnMarkerClick_Lcom_google_android_gms_maps_model_Marker_Handler:Android.Gms.Maps.GoogleMap/IOnMarkerClickListenerInvoker, Xamarin.GooglePlayServices.Maps\n" +
			"";
		mono.android.Runtime.register ("Helpa.Droid.CustomMapRendererAndroid, Helpa.Android", CustomMapRendererAndroid.class, __md_methods);
	}


	public CustomMapRendererAndroid (android.content.Context p0)
	{
		super (p0);
		if (getClass () == CustomMapRendererAndroid.class)
			mono.android.TypeManager.Activate ("Helpa.Droid.CustomMapRendererAndroid, Helpa.Android", "Android.Content.Context, Mono.Android", this, new java.lang.Object[] { p0 });
	}


	public CustomMapRendererAndroid (android.content.Context p0, android.util.AttributeSet p1)
	{
		super (p0, p1);
		if (getClass () == CustomMapRendererAndroid.class)
			mono.android.TypeManager.Activate ("Helpa.Droid.CustomMapRendererAndroid, Helpa.Android", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android", this, new java.lang.Object[] { p0, p1 });
	}


	public CustomMapRendererAndroid (android.content.Context p0, android.util.AttributeSet p1, int p2)
	{
		super (p0, p1, p2);
		if (getClass () == CustomMapRendererAndroid.class)
			mono.android.TypeManager.Activate ("Helpa.Droid.CustomMapRendererAndroid, Helpa.Android", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android:System.Int32, mscorlib", this, new java.lang.Object[] { p0, p1, p2 });
	}


	public boolean onMarkerClick (com.google.android.gms.maps.model.Marker p0)
	{
		return n_onMarkerClick (p0);
	}

	private native boolean n_onMarkerClick (com.google.android.gms.maps.model.Marker p0);

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
