using ShellDemo.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Flurl.Http;
using System.Threading.Tasks;
using ShellDemo.Models;
using Newtonsoft.Json;
using ShellDemo.Services;
namespace ShellDemo.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public Command LoginCommand { get; }

        private bool needConfirmation = false;
        private UserSession respond;


        public string ShowButtonText
        {
            get
            {
                if (MobileApp.GetSingletion().User.IsLogout())
                {
                    return "Login";
                }
                else
                {
                    return "Logout and Login";
                }
            }
        }
        private string _errorMessage;
        public string ErrorMessage
        {
            get => _errorMessage;

            set
            {
                SetProperty(ref _errorMessage, value);
                OnPropertyChanged(nameof(errorOccurred));
            }
        }
        public bool errorOccurred => _errorMessage.Length != 0;
        

        public LoginViewModel()
        {
            LoginCommand = new Command(OnLoginClicked);
            Username = "";
            Password = "";

        }

        private string _username;
        public string Username
        {
            get
            {
                return _username;
            }
            set
            {
                SetProperty(ref _username, value);
                OnPropertyChanged(nameof(Username));
            }
        }

        private string _password;
        public string Password
        {
            get => _password;
            set
            {
                SetProperty(ref _password, value);
                OnPropertyChanged(nameof(Password));
            }
        }

        private bool _canLogin;
        public bool CanLogin
        {
            get => _canLogin;
            set{
                _canLogin = value;
            }
        }

        public bool DangerInLogout()
        {
            return MobileApp.GetSingletion().User.Orders.Count != 0;
        }

        public void ClearOutData()
        {
            Username = "";
            Password = "";
        }
        private void ConfirmLogin(UserSession ans)
        {
            try
            {
                Services.ServerRequest.ConfirmLoginRequest(ans);
                // means user successfully logined user
                needConfirmation = false;
            }
            catch (Exception e)
            {
                if (e.InnerException is FlurlHttpTimeoutException)
                {
                    throw e; // tell user to retry
                }
                else if (e.InnerException is FlurlHttpException)
                {
                    // most likely 404 code, means the server has already processsed it
                    int errorcode = (int)((Flurl.Http.FlurlHttpException)e.InnerException).StatusCode;
                    if (errorcode == 404)
                    {
                        needConfirmation = false; // the server has already process the request
                    }
                    else
                    {
                        // could be 500 code
                        throw e;
                    }

                }
                else
                {
                    throw e;
                }

            }


        }

        public void Logout()
        {
            try
            {
                Services.ServerRequest.LogoutRequest();
                MobileApp.GetSingletion().User.logoutUser();    

            }catch(Exception e)
            {
                ErrorMessage = "Fail to logout, please retry by click login button";
            }


        }
        private async void OnLoginClicked(object obj)
        {
            // ask the server to send request to server.
            CanLogin = false;


            try { 
                
                if(MobileApp.GetSingletion().User.IsLogout() == false)
                {
                    Logout();  //everytime when yser try to login, logout user first
                }



                if(needConfirmation == false)
                {
                    respond = Services.ServerRequest.LoginRequest(this.Username, this.Password);
                    needConfirmation = true;
                }

                ConfirmLogin(respond);



                MobileApp.GetSingletion().User.CurrentSessionUUID = respond.Session;
                MobileApp.GetSingletion().User.AccountName = this.Username;

                ErrorMessage = "";
                CanLogin = true;
                await Shell.Current.GoToAsync($"//{nameof(MainPage)}");

            }
            catch (FlurlHttpTimeoutException)
            {

            }
           
            catch(Exception e)
            {
 
                if(e.InnerException is FlurlHttpTimeoutException)
                {
                    ErrorMessage = "An network Error occurs, please retry";
                }else if(e.InnerException is FlurlHttpException )
                {
                    int errorcode = (int)((Flurl.Http.FlurlHttpException)e.InnerException).StatusCode;
                    if (errorcode == 403)
                    {
                        // means first time login, but got 403, account alredy login in on other device
                        ErrorMessage = "Incorrect username or password";
                    }
                    else if (errorcode == 405)
                    {
                        // it has been timeout, 
                        ErrorMessage = "You account is logined in on another device already!";
                        //TODO: what if connection lost on the way back?
                    }
                    else
                    {
                        // server error
                        ErrorMessage = "An error occurs with the server, please retry";
                    }
                }
                else
                {
                    ErrorMessage = "An error occurs , please retry";
                }
                
            }
            CanLogin = true;

            //catch(Java.Net.UnknownHostException)
            // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one


           
        }
    }
}
