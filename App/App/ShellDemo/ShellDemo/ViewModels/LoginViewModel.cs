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
        private async void ConfirmLogin(UserSession ans)
        {
            try
            {
                await Services.ServerRequest.ConfirmLoginRequest(ans);
                // means user successfully logined user
                needConfirmation = false;
            }
            catch (Exception e)
            {
                throw e;

            }


        }

        public async void Logout()
        {
            try
            {
                await Services.ServerRequest.LogoutRequest(MobileApp.GetSingletion().User.CurrentSessionUUID);
                MobileApp.GetSingletion().User.logoutUser();    

            }catch(Exception e)
            {
                ErrorMessage = e.Message;
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
                    respond = await Services.ServerRequest.LoginRequest(this.Username, this.Password);
                    needConfirmation = true;
                }

                ConfirmLogin(respond);



                MobileApp.GetSingletion().User.CurrentSessionUUID = respond.Session;
                MobileApp.GetSingletion().User.AccountName = this.Username;

                ErrorMessage = "";
                CanLogin = true;
                await Shell.Current.GoToAsync($"//{nameof(MainPage)}");

            }

           
            catch(Exception e)
            {

                ErrorMessage = e.Message;
            }
            CanLogin = true;

            //catch(Java.Net.UnknownHostException)
            // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one


           
        }
    }
}
