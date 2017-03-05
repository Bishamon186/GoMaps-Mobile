using Android.App;
using Android.Widget;
using Android.OS;
using Xamarin.Auth;
using System;
using System.Linq;

namespace GoogleAccount
{
    [Activity(Label = "GoogleAccount", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        Button googleBtn;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Main);
            googleBtn = FindViewById<Button>(Resource.Id.GoogleButton);
            googleBtn.Click += delegate { GoogleAuthentcation(Constants.GOOGLE_ID, Constants.GOOGLE_SCOPE, Constants.GOOGLE_AUTH, Constants.GOOGLE_REDIRECTURL, Constants.GOOGLE_REQUESTURL); };
        }
        
        void GoogleAuthentcation(string id, string scope, string authurl, string redirecturl, string requesturl)
        {
            var auth = new OAuth2Authenticator(id, scope, new Uri(authurl), new Uri(redirecturl));
            //если пользователь захочет отменить аутентификацию
            auth.AllowCancel = true;
            StartActivity(auth.GetUI(this));
            auth.Completed += async (sender, req) =>
            {
                //если аутентификация не удалась
                if (!req.IsAuthenticated)
                {
                    //всплывает окошко с сообщением об ошибке
                    Toast.MakeText(this, Constants.AUTH_ERROR, ToastLength.Short).Show();
                    return;
                }
                //запрос к получению параметров
                var request = new OAuth2Request("GET", new Uri(requesturl), null, req.Account);
                //ответ на запрос
                var response = await request.GetResponseAsync();
                //если ответ получен
                if (response != null)
                {
                    var UserData = response.GetResponseText();
                }
                //если ответ НЕ получен
                else
                {
                    Toast.MakeText(this, Constants.AUTH_ERROR, ToastLength.Short).Show();
                }
            };
        }
    }

internal class Constants
    {
        public static readonly string GOOGLE_ID = "88489526392-inlgdr0lts7qnq31pqm9sgrsd3d7hubd.apps.googleusercontent.com";
        public static readonly string GOOGLE_SCOPE = "https://www.googleapis.com/auth/plus.login";
        public static readonly string GOOGLE_AUTH = "https://accounts.google.com/o/oauth2/auth";
        public static readonly string GOOGLE_REDIRECTURL = "https://www.googleapis.com/plus/v1/people/me";
        public static readonly string GOOGLE_REQUESTURL = "https://www.googleapis.com/oauth2/v2/userinfo";
        public static readonly string AUTH_ERROR = "Authentication error";
    }
}

