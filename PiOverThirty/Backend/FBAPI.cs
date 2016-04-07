using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Facebook;
using Xamarin.Facebook.Login;
using System.Threading.Tasks;
using Java.Lang;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
namespace PiOverThirty.Backend
{
    public abstract class FBActivity:Activity
    {
        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            if (Backend.FBAPI.Instance != null)
            {
                Backend.FBAPI.Instance.OnActivityResult(requestCode, resultCode, data);
            }
            base.OnActivityResult(requestCode, resultCode, data);
        }
    }
    public class FBProfile
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Name { get; set; }
        public Uri ProfileURL { get; set; }
        public string ProfileID { get; set; }
    }
    class FacebookManagerCallback :  Java.Lang.Object, IFacebookCallback
    {
        public FacebookManagerCallback()
        {

        }
        
        TaskCompletionSource<bool> tsktsktsk = new TaskCompletionSource<bool>();
        public Task<bool> GetTask()
        {
            AccessToken token = AccessToken.CurrentAccessToken;
            if(token != null)
            {
                tsktsktsk.SetResult(true);
            }
            return tsktsktsk.Task;
        }
        

        public void OnCancel()
        {
            tsktsktsk.SetException(new System.OperationCanceledException("The user has aborted the operation."));
        }

        public void OnError(FacebookException p0)
        {
            tsktsktsk.SetException(p0);
        }

        public void OnSuccess(Java.Lang.Object p0)
        {
            tsktsktsk.SetResult(true);
        }
    }
    class FBGraphAsyncOperation : Java.Lang.Object, GraphRequest.ICallback
    {
        TaskCompletionSource<dynamic> tsktsktsk = new TaskCompletionSource<dynamic>();
        public void OnCompleted(GraphResponse p0)
        {
            try
            {
                
                tsktsktsk.SetResult(JsonConvert.DeserializeObject(p0.RawResponse));
            }catch(System.Exception er)
            {
                tsktsktsk.SetException(er);
            }
        }
        public Task<dynamic> GetTask()
        {
            return tsktsktsk.Task;
        }
    }
    public class FBAPI
    {
        public void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            callbackManager.OnActivityResult(requestCode, (int)resultCode, data);
        }
        public static FBAPI Instance;
        public static void InitializeAPI(Context context)
        {
            if(Instance == null)
            {
                Instance = new FBAPI(context);
            }
        }
        public Task<dynamic> GetGraphData(string path)
        {
            

            var retval = new FBGraphAsyncOperation();
            new GraphRequest(AccessToken.CurrentAccessToken, path, null, HttpMethod.Get, retval).ExecuteAsync();
            return retval.GetTask();
        }
        ICallbackManager callbackManager;
        public Task<bool> Authenticate(FBActivity currentActivity)
        {
            
            var retval = new FacebookManagerCallback();
            
            LoginManager.Instance.RegisterCallback(callbackManager, retval);
            if (AccessToken.CurrentAccessToken == null)
            {
                LoginManager.Instance.LogInWithReadPermissions(currentActivity, new string[] { "public_profile", "user_friends" });
            }
            return retval.GetTask();
        }
        public FBProfile UserProfile
        {
            get
            {
                var p = Profile.CurrentProfile;
                
                return new FBProfile() { FirstName = p.FirstName, LastName = p.LastName, Name = p.Name, ProfileURL = new Uri(p.LinkUri.ToString()), ProfileID = p.Id };
            }
        }
        private FBAPI(Context ctx)
        {
            FacebookSdk.SdkInitialize(ctx);
            callbackManager = CallbackManagerFactory.Create();
        }
    }
}