

using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Urho.Droid;
using Android.Accounts;
namespace PiOverThirty
{
    [Activity(Label = "PiOverThirty", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        int count = 1;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            Com.Facebook.FacebookSdk.SdkInitialize(this);
            Com.Facebook.Login.LoginManager mngr;
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            Button button = FindViewById<Button>(Resource.Id.MyButton);

            button.Click += delegate { button.Text = string.Format("{0} clicks!", count++); };


            AbsoluteLayout layout = FindViewById<AbsoluteLayout>(Resource.Id.absoluteLayout1);
            var surface = UrhoSurface.CreateSurface<AppMain>(this);
            layout.AddView(surface);
            
        }
    }
}

