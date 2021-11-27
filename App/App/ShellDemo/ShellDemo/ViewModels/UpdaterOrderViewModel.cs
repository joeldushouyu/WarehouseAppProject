using System;
using System.Collections.Generic;
using System.Text;
using Flurl.Http;
using ShellDemo.Models;
using Xamarin.Forms;

namespace ShellDemo.ViewModels
{
    public class UpdaterOrderViewModel:BaseViewModel
    {

        public Command UpdateCommand { get; }

        public bool _isUpdating;
        public bool IsUpdating
        {
            get => _isUpdating;
            set
            {
                SetProperty(ref _isUpdating, value);
                OnPropertyChanged(nameof(IsUpdating));
            }
        }

        public string _errorMessage;
        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                SetProperty(ref _errorMessage, value);
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }

        public UpdaterOrderViewModel()
        {
            UpdateCommand = new Command(OnUpdate);
        }

        private void ResetRequest()
        {

        }

        private bool ConfirmUpdateOrder(UserSession userSess)
        {
            try
            {
                string url = MobileApp.GetSingletion().BaseUrl + "/confirm";
                var respondCode = url.WithTimeout(20).PostJsonAsync(userSess).Result;
                // means the server has successfully got the message from us.
                return true;
            }
            catch (FlurlHttpTimeoutException e)
            {
                throw e;

            }
            catch (FlurlHttpException e)
            {

                int statusCode = (int)((Flurl.Http.FlurlHttpException)e).StatusCode;
                if (statusCode == 404)
                {

                    try
                    {
                        // it could be that the server could have already execute the command
                        string url = MobileApp.GetSingletion().BaseUrl + "/readyToLogout";
                        var respond = url.PostJsonAsync(userSess).Result;
                        // if still here, means no exception occur and got 200 code
                    }
                    catch (FlurlHttpException es)
                    {
                        throw es;
                    }

                }
                else
                {
                    throw e;
                }
            }
            return false;
        }

        public void OnUpdate()
        {
            try
            {
                string updateUrl = MobileApp.GetSingletion().BaseUrl + "/updateOrder";
                var respond = updateUrl.WithTimeout(20).PostJsonAsync(new UpdateOrderRequest
                {
                    Session = MobileApp.GetSingletion().User.CurrentSessionUUID,
                    Orders = MobileApp.GetSingletion().User.Orders
                }).Result;
                Console.WriteLine("success");

                ConfirmUpdateOrder(new UserSession
                {
                    Session = MobileApp.GetSingletion().User.CurrentSessionUUID

                });

                MobileApp.GetSingletion().User.Orders.Clear();
                // return true;
            }
            catch (FlurlHttpTimeoutException e)
            {
                ErrorMessage ="An network error occurs, please retry";
            }
            catch (FlurlHttpException e)
            {
                int errorcode = (int)((Flurl.Http.FlurlHttpException)e).StatusCode;
                if(errorcode == 404)
                {
                    // means something wrong with the data
                    // for some reason, the data does not consist with the server's status
                    // call reset API with the server.
                }
                // return false;

            }
            catch (Exception e)
            {
                // return false;
            }
        }
    }
}
