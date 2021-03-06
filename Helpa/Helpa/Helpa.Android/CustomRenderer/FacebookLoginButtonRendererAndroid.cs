﻿using Android.App;
using System;
using Xamarin.Facebook.Login;
using Xamarin.Facebook.Login.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Helpa;
using Helpa.Droid;
using Android.Content;
using Java.Util;

[assembly: ExportRenderer(typeof(FacebookLoginButton), typeof(FacebookLoginButtonRendererAndroid))]
namespace Helpa.Droid
{
    public class FacebookLoginButtonRendererAndroid : ViewRenderer<Button, LoginButton>
    {
        private static Activity _activity;

        public FacebookLoginButtonRendererAndroid(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
        {
            base.OnElementChanged(e);

            _activity = this.Context as MainActivity;
            var loginButton = new LoginButton(this.Context);
            var facebookCallback = new FacebookCallback<LoginResult>
            {
                HandleSuccess = shareResult =>
                {
                    Action<string> local = App.PostSuccessFacebookAction;
                    if (local != null)
                    {
                        local(shareResult.AccessToken.Token);
                    }
                }
        ,
                HandleCancel = () =>
                {
                    Console.WriteLine("HelloFacebook: Canceled");
                },
                HandleError = shareError =>
                {
                    Console.WriteLine("HelloFacebook: Error: {0}", shareError);
                }
            };
            //loginButton.SetBackgroundDrawable(Resources.GetDrawable( Resource.Drawable.fb));
            loginButton.RegisterCallback(MainActivity.CallbackManager, facebookCallback);
            loginButton.SetReadPermissions(new string[] { "email", "user_gender" });
                        
            loginButton.SetCompoundDrawablesWithIntrinsicBounds(null, null, null, null);
            //loginButton.SetPadding(Resources.GetDimensionPixelSize(Resource.Dimension.fb_margin_override_lr),
            //    Resources.GetDimensionPixelSize(Resource.Dimension.fb_margin_override_top),
            //    Resources.GetDimensionPixelSize(Resource.Dimension.fb_margin_override_lr),
            //    Resources.GetDimensionPixelSize(Resource.Dimension.fb_margin_override_bottom));

            loginButton.TextAlignment = Android.Views.TextAlignment.Center;
            loginButton.Gravity = Android.Views.GravityFlags.Center;
            loginButton.Text = "Sign up with Facebook";
            loginButton.SetPadding(0, 10, 0, 10);
            base.SetNativeControl(loginButton);
        }
    }
}