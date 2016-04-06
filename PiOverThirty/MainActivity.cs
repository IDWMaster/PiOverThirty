

using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Urho.Droid;
using Android.Accounts;
using Xamarin.Facebook.Login;
using Xamarin.Facebook;
using Java.Lang;

[assembly: MetaData("com.facebook.sdk.ApplicationId", Value = "@string/app_id")]
[assembly: Permission(Name = Android.Manifest.Permission.Internet)]
[assembly: Permission(Name = Android.Manifest.Permission.WriteExternalStorage)]

namespace PiOverThirty
{
    



    [Activity(Label = "PiOverThirty", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity, IFacebookCallback
    {
        int count = 1;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            FacebookSdk.SdkInitialize(this);

          //  Com.Facebook.FacebookSdk.SdkInitialize(this);
           
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            
            // Get our button from the layout resource,
            // and attach an event to it
            Button button = FindViewById<Button>(Resource.Id.MyButton);

            var lm = LoginManager.Instance;
            var callbackManager = Xamarin.Facebook.CallbackManagerFactory.Create();
            lm.RegisterCallback(callbackManager, this);
            button.Click += delegate {
                lm.LogInWithReadPermissions(this, new string[] { "public_profile", "user_friends" });
            };
            //PendingAction act;

            
            AbsoluteLayout layout = FindViewById<AbsoluteLayout>(Resource.Id.absoluteLayout1);
            var surface = UrhoSurface.CreateSurface<AppMain>(this);
            layout.AddView(surface);
            
        }

        void IFacebookCallback.OnCancel()
        {
            throw new NotImplementedException();
        }

        void IFacebookCallback.OnError(FacebookException p0)
        {
            throw new NotImplementedException();
        }

        void IFacebookCallback.OnSuccess(Java.Lang.Object p0)
        {
            Console.WriteLine("We're in!");
        }
    }
}

