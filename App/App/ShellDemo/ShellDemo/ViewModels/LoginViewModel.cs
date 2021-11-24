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

        private bool _canLogin;
        public bool CanLogin
        {
            get => _canLogin;
            set{
                _canLogin = value;
            }
        }

        private bool ConfirmLogin(UserSession ans)
        {
            try
            {
                string url = MobileApp.GetSingletion().BaseUrl + "/confirm";
                var respondCode =url.WithTimeout(20).PostJsonAsync(ans).Result;
                // means user successfully logined user
                return true;
            }
            catch (FlurlHttpTimeoutException er)
            {
                throw er;
             
            }catch (FlurlHttpException e)
            {
                int statusCode = (int)((Flurl.Http.FlurlHttpException)e).StatusCode;
                if(statusCode == 404)
                {
                    // could be the user has already login, or for somereason it is lost on the server

                    //check if server already login
                    try
                    {
                        string url_isLogin = MobileApp.GetSingletion().BaseUrl + "/islogin";
                        var res =  url_isLogin.PostJsonAsync(new User { AccountName = _username, Password = _password });

                        // success
                        return true;
              
                        
                    }
                    catch(FlurlHttpException es)
                    {
                        //here means just not login
                        throw es;
                    }
                }
                else
                {
                    throw e;
                }
            }
            

        }

        private async void OnLoginClicked(object obj)
        {
            // ask the server to send request to server.
            CanLogin = false;
            try
            {
                string url_isLogin = MobileApp.GetSingletion().BaseUrl + "/islogin";


                try
                {
                    var res = await url_isLogin.PostJsonAsync(new User { AccountName = _username, Password = _password });
                    ErrorMessage = "You account is logined in on another device already!";
                }
                catch(FlurlHttpException es)
                {
                    int statusCode = (int)((Flurl.Http.FlurlHttpException)es).StatusCode;
                  
                    if (statusCode == 400)
                    {
                        //means not login yet
                        string url = MobileApp.GetSingletion().BaseUrl + "/login";
                        UserSession ans = await url.WithTimeout(20).PostJsonAsync(new User
                        { AccountName = this.Username, Password = this.Password }).ReceiveJson<UserSession>();

                        // send another request to the server, to confirm it receive the session 


                        // if 
                        if ( ConfirmLogin(ans) == true)
                        {
                            MobileApp.GetSingletion().User.CurrentSessionUUID = ans.Session;
                            MobileApp.GetSingletion().User.AccountName = this.Username;

                            ErrorMessage = "";
                            CanLogin = true;
                            await Shell.Current.GoToAsync($"//{nameof(MainPage)}");
                        }
                        else
                        {
                            ErrorMessage = "An network error occurs, please retry";
                        }
                       
                    }
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
                    //TODO: what if connection lost on the way back?
                }
                else
                {
                    // server error
                    ErrorMessage = "An error occurs with the server, please retry";
                }

            }
            catch(Exception e)
            {
                Console.WriteLine("hello world");
            }
            CanLogin = true;

            //catch(Java.Net.UnknownHostException)
            // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one


           
        }
    }
}
