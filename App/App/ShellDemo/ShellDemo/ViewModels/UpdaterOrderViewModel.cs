using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Flurl.Http;
using ShellDemo.Models;
using ShellDemo.Views;
using Xamarin.Forms;
using ShellDemo.Services;

namespace ShellDemo.ViewModels
{
    public class UpdaterOrderViewModel:BaseViewModel
    {

        public Command UpdateCommand { get; }
        public Command Finish { get; }

        private bool needConfirmation = false;
        private UpdateOrderResponse respond;

       

        public string ErrorOrderInformation { get; set; }
        public bool _isFinishUpdate;
        public bool IsFinishUpdate
        {
            get => _isFinishUpdate;
            set
            {
                SetProperty(ref _isFinishUpdate, value);
                OnPropertyChanged(nameof(IsFinishUpdate));
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
            Finish = new Command(FinishCommand);
        }
        

        private async void FinishCommand()
        {
            //https://stackoverflow.com/questions/24856116/how-to-popasync-more-than-1-page-in-xamarin-forms-navigation
            // Modify
            for (var counter = 1; ; counter++)
            {
                var removingPage = Shell.Current.Navigation.NavigationStack[Shell.Current.Navigation.NavigationStack.Count - 2];
                if(removingPage is OrderGettedListPage || removingPage ==null)
                {
                    break; // stop removing anymore
                }
                else
                {
                    Shell.Current.Navigation.RemovePage(removingPage);
                }
                
            }
            await Shell.Current.Navigation.PopAsync();
        }

        private void ConfirmUpdateOrder()
        {
            try
            {

                var resoind = Services.ServerRequest.ConfirmUpdateRequest();
                // means the server has successfully got the message from us.
                needConfirmation = false;
         
            }
            catch(Exception e)
            {
                if(e.InnerException is FlurlHttpTimeoutException)
                {
                    throw e; // tell user to retry in the onUpdate code's catch
                }else if(e.InnerException is FlurlHttpException)
                {
                    // most likely 404 code, means the server has already processsed it
                    int errorcode = (int)((Flurl.Http.FlurlHttpException)e.InnerException).StatusCode;
                    if(errorcode == 404)
                    {
                        // the server has already process the request
                        needConfirmation = false;
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

        public void OnUpdate()
        {
            IsFinishUpdate= false;
            try
            {
                
                if(needConfirmation == false)
                {
                    // means did not send any request to the server yet
                    respond = ServerRequest.UpdateRequest();
                    needConfirmation = true;
                }
                else
                {
                    ; //pass, go straight to confirm request as describe below.
                }

                Console.WriteLine("success");

                ConfirmUpdateOrder();

                // check to see if there are orders containing error
                UpdateOrderResponse errorOrders = respond;
                if(errorOrders.ErrorOrderID.Count == 0)
                {
                    // means no error order occurs during the update
                }
                else
                {
                    // means error order occurs
                    foreach(long orderID in errorOrders.ErrorOrderID)
                    {
                        ErrorOrderInformation = "Error Order Information";
                        Order ord = null;
                        foreach(Order ordertemp in MobileApp.GetSingletion().User.Orders)
                        {
                            if(ordertemp.IDAtDatabase == orderID)
                            {
                                ord = ordertemp;
                                break;
                            }
                            ErrorOrderInformation += String.Format("OrderID:{0}   OrderBarCode:{1}", ord.IDAtDatabase, ord.BarCode);


                        }
                    }
                }
                MobileApp.GetSingletion().User.Orders.Clear();
                ErrorMessage = "";
                IsFinishUpdate = true;

                // return true;
            }
            catch (Exception e)
            {
                if(e.InnerException is FlurlHttpTimeoutException)
                {
                    ErrorMessage = "An network error occurs, please retry";
                }else if(e.InnerException is FlurlHttpException)
                {
                    int errorcode = (int)((Flurl.Http.FlurlHttpException)e.InnerException).StatusCode;
                    if (errorcode == 404)
                    {

                        // means incorrect format
                        //should not happen,
                        //but if does, report 
                        ErrorMessage = "An undesire error happens, please retry";
                    }
                    else if (errorcode == 403)
                    {
                        // incorrect user UUID, 
                        //Nothing can do except let the user re-login
                        ErrorMessage = "Please try relogin with your account";
                    }
                    else
                    {
                        // mayeb 500
                        ErrorMessage = "An undesire error happens, please retry";
                    }
                }
                else
                {
                    ErrorMessage = "An undesire error happens, please retry";
                }
                
            }

           
        }
    }
}
