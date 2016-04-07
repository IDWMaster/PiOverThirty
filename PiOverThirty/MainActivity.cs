

using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Accounts;

[assembly: MetaData("com.facebook.sdk.ApplicationId", Value = "@string/app_id")]
[assembly: Permission(Name = Android.Manifest.Permission.Internet)]
[assembly: Permission(Name = Android.Manifest.Permission.WriteExternalStorage)]

namespace PiOverThirty
{
    



    [Activity(Label = "PiOverThirty", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Backend.FBActivity
    {
        
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            Backend.FBAPI.InitializeAPI(this);
            var fb = Backend.FBAPI.Instance;
           
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            
            // Get our button from the layout resource,
            // and attach an event to it
            Button button = FindViewById<Button>(Resource.Id.MyButton);
            Xamarin.Facebook.Login.Widget.ProfilePictureView ppp = new Xamarin.Facebook.Login.Widget.ProfilePictureView(this);
            ppp.ProfileId = fb.UserProfile.ProfileID;
            FindViewById<LinearLayout>(Resource.Id.linearLayout1).AddView(ppp);
            
            button.Click += async delegate {
                try
                {
                    await fb.Authenticate(this);
                    button.Text = "Logged in to Facebook";
                    button.Enabled = false;
                    dynamic friendsList = await fb.GetGraphData("me/friends");
                    button.Text = "Wow "+fb.UserProfile.FirstName+" "+fb.UserProfile.LastName+"! You have "+friendsList["summary"]["total_count"]+" friends!";
                    
                }
                catch(Exception er)
                {
                    button.Text = er.Message;
                }
            };
            //PendingAction act;
            
        }

       
    }
}

