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

using RestSharp;

using iCoffe.Shared;
using System.Net;

namespace iCoffe.Droid
{
    [Activity(Label = "RegActivity")]
    public class RegActivity : Activity
    {
        //Consts
        public const string C_USER_EMAIL = @"C_USER_EMAIL";
        public const string C_USER_PASSWORD = @"C_USER_PASSWORD";

        //Editors
        EditText userLastName;
        EditText userFirstName;
        EditText userCity;
        EditText userEmail;
        EditText userPassword;
        EditText userConfirm;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.Reg);

            FindViewById<Button>(Resource.Id.rExitBtn).Click += ExitBtn_Click;
            FindViewById<Button>(Resource.Id.rRegisterBtn).Click += RegisterBtn_Click;

            userLastName = FindViewById<EditText>(Resource.Id.rLastNameET);
            userFirstName = FindViewById<EditText>(Resource.Id.rFirstNameET);
            userCity = FindViewById<EditText>(Resource.Id.rCityET);
            userEmail = FindViewById<EditText>(Resource.Id.rEmailET);
            userPassword = FindViewById<EditText>(Resource.Id.rPasswordET);
            userConfirm = FindViewById<EditText>(Resource.Id.rConfirmET);
        }

        private void ExitBtn_Click(object sender, EventArgs e)
        {
            SetResult(Result.Canceled);
            Finish();
            //throw new NotImplementedException();
        }

        private void RegisterBtn_Click(object sender, EventArgs e)
        {
            if (IsInputAccepted())
            {
                TryRegister();
            }
        }

        private bool IsInputAccepted()
        {
            if (String.IsNullOrEmpty(userEmail.Text))
            {
                Toast.MakeText(this, @"Введите свой <E-mail>.", ToastLength.Short).Show();
                return false;
            }

            if (String.IsNullOrEmpty(userPassword.Text))
            {
                Toast.MakeText(this, @"Введите <Пароль>.", ToastLength.Short).Show();
                return false;
            }

            if (String.IsNullOrEmpty(userConfirm.Text))
            {
                Toast.MakeText(this, @"Введите <Подтверждение>.", ToastLength.Short).Show();
                return false;
            }

            if (String.Compare(userConfirm.Text, userPassword.Text) != 0)
            {
                Toast.MakeText(this, @"<Пароль> и <Подтверждение> должны совпадать.", ToastLength.Short).Show();
                return false;
            }

            return true;
        }

        private void TryRegister()
        {
            var client = new RestClient(Settings.ApiUrl);
            var request = new RestRequest(Settings.RegisterPath, Method.POST);
            request.AddParameter(@"Email", userEmail.Text);
            request.AddParameter(@"Password", userPassword.Text);
            request.AddParameter(@"ConfirmPassword", userConfirm.Text);
            var response = client.Execute(request);
            if (response.StatusCode == HttpStatusCode.OK)
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
                    .SetMessage("Ошибка регистрации. Попробуйте еще раз!")
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