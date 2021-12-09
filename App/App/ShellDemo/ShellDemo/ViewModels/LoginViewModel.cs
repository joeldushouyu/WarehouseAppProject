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

       
        public bool IsRunning { get => !CanLogin; }

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
                SetProperty(ref _canLogin, value);
                OnPropertyChanged(nameof(CanLogin));
                OnPropertyChanged(nameof(IsRunning));
            }
        }

        /// <summary>
        /// This function check to see if there are still orders left in User.Orders
        /// </summary>
        /// <returns>return true if does, else false</returns>
        public bool DangerInLogout()
        {
            return MobileApp.GetSingletion().User.Orders.Count != 0;
        }

        /// <summary>
        /// This function clear out the data in Username and Password Property
        /// </summary>
        public void ClearOutData()
        {
            Username = "";
            Password = "";
        }

        /// <summary>
        /// This function sends a confirm request to the server, confirm login. Throw Execption for any error occurs
        /// </summary>
        /// <param name="ans"> UserSession with current UUID from server</param>
        private async Task ConfirmLogin(UserSession ans)
        {
            try
            {
                await Services.ServerRequest.ConfirmLoginRequest(ans);
                // means user successfully logined user
                needConfirmation = false;
            }
            catch (Exception e)
            {
                throw new ConfirmException(e.Message);

            }


        }

        /// <summary>
        /// This sends a logout request to the server to logout current account
        /// </summary>
        public async Task Logout()
        {
            try
            {
                CanLogin = false;
                await Services.ServerRequest.LogoutRequest(MobileApp.GetSingletion().User.CurrentSessionUUID);
                MobileApp.GetSingletion().User.logoutUser();
             

            }catch(Exception e)
            {
                ErrorMessage = e.Message;
            }
            finally
            {
                CanLogin = true;
            }


        }

        /// <summary>
        /// Event handler when user clicks the login button
        /// This sends a login request to the server
        /// </summary>
        private async void OnLoginClicked()
        {
            // ask the server to send request to server.
            CanLogin = false;


           try { 
                
                if(MobileApp.GetSingletion().User.IsLogout() == false)
                {
                   await Logout();  //everytime when yser try to login, logout user first
                    CanLogin = false;
                }



                if(needConfirmation == false)
                {
                    respond = await Services.ServerRequest.LoginRequest(this.Username, this.Password);
                    needConfirmation = true;
                }

                await ConfirmLogin(respond);



                MobileApp.GetSingletion().User.CurrentSessionUUID = respond.Session;
                MobileApp.GetSingletion().User.AccountName = this.Username;

                ErrorMessage = "";
                CanLogin = true;
                

            }
            catch(Exception e)
            {

                ErrorMessage = e.Message;
            }

            finally
            {   if(MobileApp.GetSingletion().User.IsLogout() == false)
                {
                    await Shell.Current.GoToAsync($"//{nameof(MainPage)}");
                }
              CanLogin = true;  
            }
            

           
        }
    }
}
