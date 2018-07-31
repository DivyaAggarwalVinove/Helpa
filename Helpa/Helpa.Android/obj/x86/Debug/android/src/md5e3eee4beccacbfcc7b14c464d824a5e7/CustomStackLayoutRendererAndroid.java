package md5e3eee4beccacbfcc7b14c464d824a5e7;


public class CustomStackLayoutRendererAndroid
	extends md51558244f76c53b6aeda52c8a337f2c37.VisualElementRenderer_1
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("Helpa.Droid.CustomStackLayoutRendererAndroid, Helpa.Android", CustomStackLayoutRendererAndroid.class, __md_methods);
	}


	public CustomStackLayoutRendererAndroid (android.content.Context p0)
	{
		super (p0);
		if (getClass () == CustomStackLayoutRendererAndroid.class)
			mono.android.TypeManager.Activate ("Helpa.Droid.CustomStackLayoutRendererAndroid, Helpa.Android", "Android.Content.Context, Mono.Android", this, new java.lang.Object[] { p0 });
	}


	public CustomStackLayoutRendererAndroid (android.content.Context p0, android.util.AttributeSet p1)
	{
		super (p0, p1);
		if (getClass () == CustomStackLayoutRendererAndroid.class)
			mono.android.TypeManager.Activate ("Helpa.Droid.CustomStackLayoutRendererAndroid, Helpa.Android", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android", this, new java.lang.Object[] { p0, p1 });
	}


	public CustomStackLayoutRendererAndroid (android.content.Context p0, android.util.AttributeSet p1, int p2)
	{
		super (p0, p1, p2);
		if (getClass () == CustomStackLayoutRendererAndroid.class)
			mono.android.TypeManager.Activate ("Helpa.Droid.CustomStackLayoutRendererAndroid, Helpa.Android", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android:System.Int32, mscorlib", this, new java.lang.Object[] { p0, p1, p2 });
	}

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
