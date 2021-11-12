using ShellDemo.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Flurl.Http;
using System.Threading.Tasks;
using ShellDemo.Models;
using Newtonsoft.Json;

namespace ShellDemo.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public Command LoginCommand { get; }

        private bool timedOut = false;

        private string _errorMessage;
        public string ErrorMessage
        {
            get => _errorMessage;
            set => _errorMessage = value;
        }
        public bool errorOccurred => _errorMessage.Length != 0;
        

        public LoginViewModel()
        {
            LoginCommand = new Command(OnLoginClicked);
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
                _username = value;
            }
        }

        private string _password;
        public string Password
        {
            get => _password;
            set => _password = value;
        }



        private async void OnLoginClicked(object obj)
        {
            // ask the server to send request to server.
            try
            {
                string url_isLogin = MobileApp.GetSingletion().BaseUrl + "/islogin";
                var res = await url_isLogin.PostJsonAsync(new User { AccountName = "Joel", Password = "200367" });



                while (res == null) ;
      
                
                if (res.StatusCode == 200)
                {
                    ErrorMessage = "You account is logined in on another device already!";
                }
                else //error code 404
                {

                    string url = MobileApp.GetSingletion().BaseUrl + "/login";
                    UserSession ans = await url.WithTimeout(20).PostJsonAsync(new User
                    { AccountName = this.Username, Password = this.Password }).ReceiveJson<UserSession>();

                    MobileApp.GetSingletion().User.CurrentSession = ans;
                    MobileApp.GetSingletion().User.AccountName = this.Username;

                    ErrorMessage = "";
                    await Shell.Current.GoToAsync($"//{nameof(MainPage)}");
                }
                
          
                
            }
            catch( FlurlHttpTimeoutException e )
            {
                ErrorMessage = "An network Error occurs, please retry";

            }
            catch(FlurlHttpException e)
            {
                int errorcode = (int)((Flurl.Http.FlurlHttpException)e).StatusCode;
                if(errorcode == 403 )
                {
                    // means first time login, but got 403, account alredy login in on other device
                    ErrorMessage = "Incorrect username or password";
                }else if(errorcode ==405)
                {
                    // it has been timeout, 
                    ErrorMessage = "You account is logined in on another device already!";
                }
                else
                {
                    // server error
                    ErrorMessage = "An error occurs with the server, please retry";
                }

            }

            //catch(Java.Net.UnknownHostException)
            // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one


           
        }
    }
}
