using System;
using Android.Gms.Maps.Model;
using Com.Google.Maps.Android.Clustering;
using Com.Google.Maps.Android.Clustering.View;

public class MyItem : Java.Lang.Object, IClusterItem
{    private LatLng mPosition { get; set; }
    private string mTitle { get; set; }
    private string mSnippet { get; set; }

    public LatLng Position {
        get
        {
            return mPosition;
        }
    }

    public string Snippet
    {
        get
        {
            return mSnippet;
        }
    }

    public string Title
    {
        get
        {
            return mTitle;
        }
    }

    public MyItem(double lat, double lng)
    {
        mPosition = new LatLng(lat, lng);
    }

    public MyItem(double lat, double lng, string title, string snippet)
    {
        mPosition = new LatLng(lat, lng);
        mTitle = title;
        mSnippet = snippet;
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }
}