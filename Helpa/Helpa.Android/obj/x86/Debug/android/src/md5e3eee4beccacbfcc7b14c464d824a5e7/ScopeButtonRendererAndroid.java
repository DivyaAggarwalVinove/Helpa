package md5e3eee4beccacbfcc7b14c464d824a5e7;


public class ScopeButtonRendererAndroid
	extends md51558244f76c53b6aeda52c8a337f2c37.ButtonRenderer
	implements
		mono.android.IGCUserPeer,
		android.view.View.OnClickListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onClick:(Landroid/view/View;)V:GetOnClick_Landroid_view_View_Handler:Android.Views.View/IOnClickListenerInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n" +
			"";
		mono.android.Runtime.register ("Helpa.Droid.ScopeButtonRendererAndroid, Helpa.Android", ScopeButtonRendererAndroid.class, __md_methods);
	}


	public ScopeButtonRendererAndroid (android.content.Context p0)
	{
		super (p0);
		if (getClass () == ScopeButtonRendererAndroid.class)
			mono.android.TypeManager.Activate ("Helpa.Droid.ScopeButtonRendererAndroid, Helpa.Android", "Android.Content.Context, Mono.Android", this, new java.lang.Object[] { p0 });
	}


	public ScopeButtonRendererAndroid (android.content.Context p0, android.util.AttributeSet p1)
	{
		super (p0, p1);
		if (getClass () == ScopeButtonRendererAndroid.class)
			mono.android.TypeManager.Activate ("Helpa.Droid.ScopeButtonRendererAndroid, Helpa.Android", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android", this, new java.lang.Object[] { p0, p1 });
	}


	public ScopeButtonRendererAndroid (android.content.Context p0, android.util.AttributeSet p1, int p2)
	{
		super (p0, p1, p2);
		if (getClass () == ScopeButtonRendererAndroid.class)
			mono.android.TypeManager.Activate ("Helpa.Droid.ScopeButtonRendererAndroid, Helpa.Android", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android:System.Int32, mscorlib", this, new java.lang.Object[] { p0, p1, p2 });
	}


	public void onClick (android.view.View p0)
	{
		n_onClick (p0);
	}

	private native void n_onClick (android.view.View p0);

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
