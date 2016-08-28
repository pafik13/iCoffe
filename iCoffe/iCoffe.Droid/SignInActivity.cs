using System;
using System.Threading;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;

using iCoffe.Shared;
using Android.Views.InputMethods;

namespace iCoffe.Droid
{
    [Activity(Label = "SignInActivity")]
    public class SignInActivity : Activity
    {
        // Consts
        public const string C_USER_EMAIL = @"C_USER_EMAIL";
        public const string C_USER = @"C_USER";
        public const int C_REQUEST_CODE_SING_UP = 1;

        // Editors
        EditText userEmail;
        EditText userPassword;

        // Intermedia
        Dialog dialog;
        ProgressDialog progressDialog;

        // Flags
        bool IsBackPressed;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            RequestWindowFeature(WindowFeatures.NoTitle);

            // Create your application here
            SetContentView(Resource.Layout.SignIn);

            FindViewById<Button>(Resource.Id.siSigninB).Click += SigninB_Click;
            FindViewById<Button>(Resource.Id.siSignupB).Click += SingupB_Click;

            userEmail = FindViewById<EditText>(Resource.Id.siEmailET);
            userPassword = FindViewById<EditText>(Resource.Id.siPasswordET);
        }

        protected override void OnResume()
        {
            base.OnResume();
            IsBackPressed = false;
        }

        protected override void OnStop()
        {
            base.OnStop();
            IsBackPressed = false;
        }

        public override void OnBackPressed()
        {
            if (IsBackPressed)
            {
                GetSharedPreferences(MainActivity.C_DEFAULT_PREFS, FileCreationMode.Private)
                    .Edit()
                    .PutBoolean(MainActivity.C_IS_BACK_PRESSED_IN_SIGN_IN, IsBackPressed)
                    .Apply();

                base.OnBackPressed();
            }
            else
            {
                Toast.MakeText(this, Resource.String.repush_for_exit, ToastLength.Short).Show();
                IsBackPressed = true;
            }
        }

        private void SingupB_Click(object sender, EventArgs e)
        {
            StartActivityForResult(typeof(SignUpActivity), C_REQUEST_CODE_SING_UP);
        }

        private void SigninB_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            if (string.IsNullOrEmpty(userEmail.Text))
            {
                Toast.MakeText(this, @"Введите, пожалуйста, <E-mail>", ToastLength.Short).Show();
            }
            else
            {
                if (string.IsNullOrEmpty(userPassword.Text))
                {
                    Toast.MakeText(this, @"Введите, пожалуйста, <Пароль>", ToastLength.Short).Show();
                }
                else
                {
                    // Check if no view has focus:
                    if (CurrentFocus != null)
                    {
                        InputMethodManager imm = (InputMethodManager)GetSystemService(InputMethodService);
                        imm.HideSoftInputFromWindow(CurrentFocus.WindowToken, 0);
                    }

                    string message = @"Выполняется вход...";
                    progressDialog = ProgressDialog.Show(this, @"", message, true);
                    ThreadPool.QueueUserWorkItem(state =>
                    {
                        GetAccessToken();
                    });
                }
            }
        }

        private void GetAccessToken()
        {
            //string accessToken = Rest.GetAccessToken(userEmail.Text, userPassword.Text);
            string accessToken = Rest.GetBasicToken(userEmail.Text, userPassword.Text);
            RunOnUiThread(() =>
            {
                if (progressDialog != null) progressDialog.Dismiss();

                if (!string.IsNullOrEmpty(accessToken))
                {
                    ISharedPreferences sharedPreferences = GetSharedPreferences(MainActivity.C_DEFAULT_PREFS, FileCreationMode.Private);
                    bool isSignedLater = sharedPreferences.GetBoolean(userEmail.Text, false);

                    sharedPreferences.Edit()
                        .PutString(MainActivity.C_ACCESS_TOKEN, accessToken)
                        .PutBoolean(MainActivity.C_IS_NEED_TUTORIAL, !isSignedLater)
                        .PutBoolean(userEmail.Text, true)
                        .Apply();

                    Finish();
                }
                else
                {
                    AlertDialog.Builder builder;
                    builder = new AlertDialog.Builder(this);
                    builder.SetTitle(Resource.String.error_caption);
                    builder.SetMessage("Не найдены данные. Пожалуйста, зарегестрируйтесь!");
                    builder.SetCancelable(false);
                    builder.SetPositiveButton(@"OK", delegate {
                        dialog.Dismiss();
                    });
                    dialog = builder.Show();
                }
            });
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if (resultCode == Result.Ok)
            {
                if (requestCode == C_REQUEST_CODE_SING_UP)
                {
                    userEmail.Text = data.GetStringExtra(SignUpActivity.C_USER_EMAIL);
                    userPassword.Text = data.GetStringExtra(SignUpActivity.C_USER_PASSWORD);
                    string message = @"Выполняется вход...";
                    progressDialog = ProgressDialog.Show(this, @"", message, true);
                    GetAccessToken();
                    return;
                }
            }
        }
    }
}
