package md543ab08f537b20820eb6304a4f7928f42;


public class CustomMapRendererAndroid
	extends md5d956392b2b421ac2c9ccfc063253dcd9.MapRenderer
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
		mono.android.Runtime.register ("Helpa.Droid.CustomMapRendererAndroid, Helpa.Android, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", CustomMapRendererAndroid.class, __md_methods);
	}


	public CustomMapRendererAndroid (android.content.Context p0, android.util.AttributeSet p1, int p2)
	{
		super (p0, p1, p2);
		if (getClass () == CustomMapRendererAndroid.class)
			mono.android.TypeManager.Activate ("Helpa.Droid.CustomMapRendererAndroid, Helpa.Android, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "Android.Content.Context, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065:Android.Util.IAttributeSet, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065:System.Int32, mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e", this, new java.lang.Object[] { p0, p1, p2 });
	}


	public CustomMapRendererAndroid (android.content.Context p0, android.util.AttributeSet p1)
	{
		super (p0, p1);
		if (getClass () == CustomMapRendererAndroid.class)
			mono.android.TypeManager.Activate ("Helpa.Droid.CustomMapRendererAndroid, Helpa.Android, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "Android.Content.Context, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065:Android.Util.IAttributeSet, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065", this, new java.lang.Object[] { p0, p1 });
	}


	public CustomMapRendererAndroid (android.content.Context p0)
	{
		super (p0);
		if (getClass () == CustomMapRendererAndroid.class)
			mono.android.TypeManager.Activate ("Helpa.Droid.CustomMapRendererAndroid, Helpa.Android, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "Android.Content.Context, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065", this, new java.lang.Object[] { p0 });
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
