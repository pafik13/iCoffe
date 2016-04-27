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
    [Activity(Label = "iCoffe.Droid", MainLauncher = true, Icon = "@drawable/icon")]
    public class SignInActivity : Activity
    {
        // Consts
        public const string C_USER_EMAIL = @"C_USER_EMAIL";
        public const string C_USER = @"C_USER";

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

            userEmail = FindViewById<EditText>(Resource.Id.siEmailET);
            userPassword = FindViewById<EditText>(Resource.Id.siPasswordET);

            ISharedPreferences prefs = GetSharedPreferences(@"icoffe", FileCreationMode.Private);
            ISharedPreferencesEditor editor = prefs.Edit();
            editor.PutString(@"lp@m.ru", @"q12345");
            editor.PutString(@"lp@m.ru" + C_USER, Data.SerializeUser(new User() { Email = @"lp@m.ru", FirstName = @"Pavel", LastName = @"Lyubin", City = @"Moscow" }));
            editor.PutString(@"ii@m.ru", @"q12345");
            editor.PutString(@"ii@m.ru" + C_USER, Data.SerializeUser(new User() { Email = @"ii@m.ru", FirstName = @"Ivan", LastName = @"Ivanov", City = @"Tver'" }));
            editor.PutString(@"pp@m.ru", @"q12345");
            editor.PutString(@"pp@m.ru" + C_USER, Data.SerializeUser(new User() { Email = @"pp@m.ru", FirstName = @"Petr", LastName = @"Petrov", City = @"Tula" }));
            editor.PutString(@"ss@m.ru", @"q12345");
            editor.PutString(@"ss@m.ru" + C_USER, Data.SerializeUser(new User() { Email = @"ss@m.ru", FirstName = @"Sidr", LastName = @"Sidrov", City = @"Pushino" }));

            editor.Apply();
        }

        private void SignBtn_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            if (string.IsNullOrEmpty(userEmail.Text))
            {
                Toast.MakeText(this, @"������� ���� E-mail!", ToastLength.Short).Show();
            }
            else
            {
                if (string.IsNullOrEmpty(userPassword.Text))
                {
                    Toast.MakeText(this, @"������� ���� Password!", ToastLength.Short).Show();
                }
                else
                {
                    ISharedPreferences prefs = GetSharedPreferences(@"icoffe", FileCreationMode.Private);
                    if (prefs.GetString(userEmail.Text, string.Empty).CompareTo(userPassword.Text) == 0)
                    {
                        // Progress
                        string message = @"�������� ��������� ������...";
                        progressDialog = ProgressDialog.Show(this, @"", message, true);
                        //progressDialog.Show();
                        ThreadPool.QueueUserWorkItem(state =>
                        {
                            Thread.Sleep(5000);

                            RunOnUiThread(() =>
                            {
                                progressDialog.Dismiss();
                                Intent intent = new Intent(this, typeof(MainActivity));
                                intent.PutExtra(MainActivity.C_IS_USER_SIGN_IN, true);
                                string user = prefs.GetString(userEmail.Text + C_USER, string.Empty);
                                ISharedPreferencesEditor editor = prefs.Edit();
                                editor.PutString(C_USER, user);
                                editor.Apply();
                                StartActivity(intent);
                            });
                        });
                       
                    }
                    else
                    {
                        AlertDialog.Builder builder;
                        builder = new AlertDialog.Builder(this);
                        builder.SetTitle(Resource.String.error_caption);
                        builder.SetMessage("������������ � ������ ������� �����������!");
                        builder.SetCancelable(false);
                        builder.SetPositiveButton(@"OK", delegate {
                            dialog.Dismiss();
                        });
                        dialog = builder.Show();
                    }
                }
            }
        }

        private void SkipBtn_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            ISharedPreferences prefs = GetSharedPreferences(@"icoffe", FileCreationMode.Private);
            ISharedPreferencesEditor editor = prefs.Edit();
            editor.PutString(C_USER, string.Empty);
            editor.Apply();
            StartActivity(typeof(MainActivity));
        }
    }
}