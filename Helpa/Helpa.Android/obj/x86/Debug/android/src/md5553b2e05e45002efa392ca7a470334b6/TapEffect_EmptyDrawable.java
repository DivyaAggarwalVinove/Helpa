package md5553b2e05e45002efa392ca7a470334b6;


public class TapEffect_EmptyDrawable
	extends android.graphics.drawable.Drawable
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_getOpacity:()I:GetGetOpacityHandler\n" +
			"n_draw:(Landroid/graphics/Canvas;)V:GetDraw_Landroid_graphics_Canvas_Handler\n" +
			"n_setAlpha:(I)V:GetSetAlpha_IHandler\n" +
			"n_setColorFilter:(Landroid/graphics/ColorFilter;)V:GetSetColorFilter_Landroid_graphics_ColorFilter_Handler\n" +
			"";
		mono.android.Runtime.register ("AsNum.XFControls.Droid.Effects.TapEffect+EmptyDrawable, AsNum.XFControls.Droid", TapEffect_EmptyDrawable.class, __md_methods);
	}


	public TapEffect_EmptyDrawable ()
	{
		super ();
		if (getClass () == TapEffect_EmptyDrawable.class)
			mono.android.TypeManager.Activate ("AsNum.XFControls.Droid.Effects.TapEffect+EmptyDrawable, AsNum.XFControls.Droid", "", this, new java.lang.Object[] {  });
	}


	public int getOpacity ()
	{
		return n_getOpacity ();
	}

	private native int n_getOpacity ();


	public void draw (android.graphics.Canvas p0)
	{
		n_draw (p0);
	}

	private native void n_draw (android.graphics.Canvas p0);


	public void setAlpha (int p0)
	{
		n_setAlpha (p0);
	}

	private native void n_setAlpha (int p0);


	public void setColorFilter (android.graphics.ColorFilter p0)
	{
		n_setColorFilter (p0);
	}

	private native void n_setColorFilter (android.graphics.ColorFilter p0);

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
