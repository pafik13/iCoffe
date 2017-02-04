using System;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;

using tutCoffee.Shared;
using System.Net;
using Android.Content.PM;

namespace tutCoffee.Droid
{
    [Activity(Label = "SignUpActivity", ScreenOrientation = ScreenOrientation.Portrait)]
    public class SignUpActivity : Activity
    {
        //Consts
        public const string C_USER_EMAIL = @"C_USER_EMAIL";
        public const string C_USER_PASSWORD = @"C_USER_PASSWORD";

        //Editors
        EditText userEmail;
        EditText userPassword;
        EditText userConfirm;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            RequestWindowFeature(WindowFeatures.NoTitle);

            // Create your application here
            SetContentView(Resource.Layout.SignUp);

            //FindViewById<Button>(Resource.Id.suExitB).Click += ExitB_Click;
            FindViewById<Button>(Resource.Id.suSignupB).Click += suSignupB_Click;

            userEmail = FindViewById<EditText>(Resource.Id.suEmailET);
            userPassword = FindViewById<EditText>(Resource.Id.suPasswordET);
            userConfirm = FindViewById<EditText>(Resource.Id.suConfirmET);
        }

        private void ExitB_Click(object sender, EventArgs e)
        {
            SetResult(Result.Canceled);
            Finish();
        }

        private void suSignupB_Click(object sender, EventArgs e)
        {
            if (IsInputAccepted())
            {
                TryRegister();
            }
        }

        private bool IsInputAccepted()
        {
            if (string.IsNullOrEmpty(userEmail.Text))
            {
                Toast.MakeText(this, @"Введите, пожалуйста, <E-mail>.", ToastLength.Short).Show();
                return false;
            }

            if (string.IsNullOrEmpty(userPassword.Text))
            {
                Toast.MakeText(this, @"Введите, пожалуйста, <Пароль>.", ToastLength.Short).Show();
                return false;
            }

            if (string.IsNullOrEmpty(userConfirm.Text))
            {
                Toast.MakeText(this, @"Введите, пожалуйста, <Подтверждение>.", ToastLength.Short).Show();
                return false;
            }

            if (string.Compare(userConfirm.Text, userPassword.Text) != 0)
            {
                Toast.MakeText(this, @"<Пароль> и <Подтверждение> не совпадают.", ToastLength.Short).Show();
                return false;
            }

            return true;
        }

        private void TryRegister()
        {
            var responseStatusCode = Rest.SignUp(userEmail.Text, userPassword.Text, userConfirm.Text);
            if (responseStatusCode == HttpStatusCode.OK)
            {
                Intent myIntent = new Intent(this, typeof(SignInActivity));
                myIntent.PutExtra(C_USER_EMAIL, userEmail.Text);
                myIntent.PutExtra(C_USER_PASSWORD, userPassword.Text);
                SetResult(Result.Ok, myIntent);
                Finish();
            }
            else
            {
                new AlertDialog.Builder(this)
                    .SetTitle(Resource.String.error_caption)
                    .SetMessage("Произошла ошибка. Попробуйте еще раз!")
                    .SetCancelable(false)
                    .SetPositiveButton(@"OK", (dialog, args) => {
                        if (dialog is Dialog)
                        {
                            ((Dialog)dialog).Dismiss();
                        }
                    })
                    .Show();
            }
        }
    }
}
