using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using iCoffe.Shared;

namespace iCoffe.Droid
{
    [Activity(Label = "SignInActivity")]
    public class SignInActivity : Activity
    {
        // Consts
        public const string C_USER_EMAIL = @"C_USER_EMAIL";
        public const string C_USER = @"C_USER";
        public const int C_REQUEST_CODE_REGISTER = 1;

        //Editors
        EditText userEmail;
        EditText userPassword;

        // Intermedia
        AlertDialog.Builder builder;
        Dialog dialog;
        ProgressDialog progressDialog;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.SignIn);

            FindViewById<Button>(Resource.Id.siSignBtn).Click += SignBtn_Click;
            FindViewById<Button>(Resource.Id.siSkipBtn).Click += SkipBtn_Click;
            FindViewById<Button>(Resource.Id.siRegBtn).Click += RegBtn_Click;

            userEmail = FindViewById<EditText>(Resource.Id.siEmailET);
            userPassword = FindViewById<EditText>(Resource.Id.siPasswordET);

            //ISharedPreferences prefs = GetSharedPreferences(MainActivity.C_DEFAULT_PREFS, FileCreationMode.Private);
            //ISharedPreferencesEditor editor = prefs.Edit();
            //editor.PutString(@"lp@m.ru", @"q12345");
            //editor.PutString(@"lp@m.ru" + C_USER, Data.SerializeUser(new User() { Email = @"lp@m.ru", FirstName = @"Pavel", LastName = @"Lyubin", City = @"Moscow" }));
            //editor.PutString(@"ii@m.ru", @"q12345");
            //editor.PutString(@"ii@m.ru" + C_USER, Data.SerializeUser(new User() { Email = @"ii@m.ru", FirstName = @"Ivan", LastName = @"Ivanov", City = @"Tver'" }));
            //editor.PutString(@"pp@m.ru", @"q12345");
            //editor.PutString(@"pp@m.ru" + C_USER, Data.SerializeUser(new User() { Email = @"pp@m.ru", FirstName = @"Petr", LastName = @"Petrov", City = @"Tula" }));
            //editor.PutString(@"ss@m.ru", @"q12345");
            //editor.PutString(@"ss@m.ru" + C_USER, Data.SerializeUser(new User() { Email = @"ss@m.ru", FirstName = @"Sidr", LastName = @"Sidrov", City = @"Pushino" }));

            //editor.Apply();
        }

        private void RegBtn_Click(object sender, EventArgs e)
        {
            StartActivityForResult(typeof(RegActivity), C_REQUEST_CODE_REGISTER);
            //throw new NotImplementedException();
        }

        private void SignBtn_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            if (string.IsNullOrEmpty(userEmail.Text))
            {
                Toast.MakeText(this, @"Введите свой E-mail!", ToastLength.Short).Show();
            }
            else
            {
                if (string.IsNullOrEmpty(userPassword.Text))
                {
                    Toast.MakeText(this, @"Введите свой Password!", ToastLength.Short).Show();
                }
                else
                {
                    string message = @"Проверка введенных данных...";
                    progressDialog = ProgressDialog.Show(this, @"", message, true);
                    ThreadPool.QueueUserWorkItem(state =>
                    {
                        GetAccessToken();
                    });
                    
                    //string accessToken = Rest.GetAccessToken(userEmail.Text, userPassword.Text);
                    //if (!String.IsNullOrEmpty(accessToken)) {
                    //    AlertDialog.Builder builder;
                    //    builder = new AlertDialog.Builder(this);
                    //    builder.SetTitle(@"Новый Access Token");
                    //    builder.SetMessage(accessToken);
                    //    builder.SetCancelable(false);
                    //    builder.SetPositiveButton(@"OK", delegate {
                    //        dialog.Dismiss();
                    //    });
                    //    dialog = builder.Show();

                    //    ISharedPreferences prefs = GetSharedPreferences(MainActivity.C_DEFAULT_PREFS, FileCreationMode.Private);
                    //    ISharedPreferencesEditor editor = prefs.Edit();
                    //    editor.PutString(MainActivity.C_ACCESS_TOKEN, accessToken);
                    //    editor.Apply();
                    //    Finish();
                    //    //ISharedPreferences prefs = GetSharedPreferences(MainActivity.C_DEFAULT_PREFS, FileCreationMode.Private);
                    //    //if (prefs.GetString(userEmail.Text, string.Empty).CompareTo(userPassword.Text) == 0)
                    //    //{
                    //    //    // Progress
                    //    //    string message = @"Проверка введенных данных...";
                    //    //    progressDialog = ProgressDialog.Show(this, @"", message, true);
                    //    //    //progressDialog.Show();
                    //    //    ThreadPool.QueueUserWorkItem(state =>
                    //    //    {
                    //    //        Thread.Sleep(5000);

                    //    //        RunOnUiThread(() =>
                    //    //        {
                    //    //            progressDialog.Dismiss();
                    //    //            Intent intent = new Intent(this, typeof(MainActivity));
                    //    //            intent.PutExtra(MainActivity.C_IS_USER_SIGN_IN, true);
                    //    //            string user = prefs.GetString(userEmail.Text + C_USER, string.Empty);
                    //    //            ISharedPreferencesEditor editor = prefs.Edit();
                    //    //            editor.PutString(C_USER, user);
                    //    //            editor.Apply();
                    //    //            StartActivity(intent);
                    //    //        });
                    //    //    });

                    //}
                    //else
                    //{
                    //    AlertDialog.Builder builder;
                    //    builder = new AlertDialog.Builder(this);
                    //    builder.SetTitle(Resource.String.error_caption);
                    //    builder.SetMessage("Пользователь с такими данными отсутствует!");
                    //    builder.SetCancelable(false);
                    //    builder.SetPositiveButton(@"OK", delegate {
                    //        dialog.Dismiss();
                    //    });
                    //    dialog = builder.Show();
                    //}
                }
            }
        }

        private void GetAccessToken()
        {
            string accessToken = Rest.GetAccessToken(userEmail.Text, userPassword.Text);
            RunOnUiThread(() =>
            {
                progressDialog.Dismiss();

                if (!String.IsNullOrEmpty(accessToken))
                {
                    AlertDialog.Builder builder;
                    builder = new AlertDialog.Builder(this);
                    builder.SetTitle(@"Новый Access Token");
                    builder.SetMessage(accessToken);
                    builder.SetCancelable(false);
                    builder.SetPositiveButton(@"OK", delegate {
                        dialog.Dismiss();
                        Finish();
                    });
                
                    ISharedPreferences prefs = GetSharedPreferences(MainActivity.C_DEFAULT_PREFS, FileCreationMode.Private);
                    ISharedPreferencesEditor editor = prefs.Edit();
                    editor.PutString(MainActivity.C_ACCESS_TOKEN, accessToken);
                    editor.Apply();

                    dialog = builder.Show();
                }
                else
                {
                    AlertDialog.Builder builder;
                    builder = new AlertDialog.Builder(this);
                    builder.SetTitle(Resource.String.error_caption);
                    builder.SetMessage("Пользователь с такими данными отсутствует!");
                    builder.SetCancelable(false);
                    builder.SetPositiveButton(@"OK", delegate {
                        dialog.Dismiss();
                    });
                    dialog = builder.Show();
                }
            });
        }

        private void SkipBtn_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            ISharedPreferences prefs = GetSharedPreferences(MainActivity.C_DEFAULT_PREFS, FileCreationMode.Private);
            ISharedPreferencesEditor editor = prefs.Edit();
            editor.PutString(C_USER, string.Empty);
            editor.Apply();
            Finish();
            //StartActivity(typeof(MainActivity));
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if (resultCode == Result.Ok)
            {
                if (requestCode == C_REQUEST_CODE_REGISTER)
                {
                    userEmail.Text = data.GetStringExtra(RegActivity.C_USER_EMAIL);
                    userPassword.Text = data.GetStringExtra(RegActivity.C_USER_PASSWORD);
                    GetAccessToken();
                    return;
                }               
            }
        }
    }
}