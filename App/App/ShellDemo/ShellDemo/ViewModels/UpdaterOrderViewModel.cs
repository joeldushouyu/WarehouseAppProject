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


        private string _errorOrderInformation;
        public string ErrorOrderInformation
        {
            get
            {
                return _errorOrderInformation;
            }
            set
            {
                SetProperty(ref _errorOrderInformation, value);
                OnPropertyChanged(nameof(ErrorOrderInformation));
            }
        }
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

        /// <summary>
        /// The function sends an  confirm Update Order request to the server, it throws Exception for any error occurs.
        /// </summary>
        private async Task ConfirmUpdateOrder()
        {
            try
            {

                var resoind = await Services.ServerRequest.ConfirmUpdateRequest(MobileApp.GetSingletion().User.CurrentSessionUUID);
                // means the server has successfully got the message from us.
                needConfirmation = false;
         
            }
            catch(Exception e)
            {
                throw new  ConfirmException(e.Message);
                
            }

        }

        /// <summary>
        /// The function sends update request to the server to update Orders picked by user.
        /// </summary>
        public async void OnUpdate()
        {
            IsFinishUpdate= false;
            try
            {
                
                if(needConfirmation == false)
                {
                    // means did not send any request to the server yet
                    respond = await ServerRequest.UpdateRequest(MobileApp.GetSingletion().User.CurrentSessionUUID, MobileApp.GetSingletion().User.Orders);
                    needConfirmation = true;
                }
                else
                {
                    ; //pass, go straight to confirm request as describe below.
                }

                Console.WriteLine("success");

                await ConfirmUpdateOrder();

                // clear out the database


                // check to see if there are orders containing error
                UpdateOrderResponse errorOrders = respond;
                if(errorOrders.ErrorOrderID.Count == 0)
                {
                    // means no error order occurs during the update
                }
                else
                {
                    
                    ErrorOrderInformation = "";
                    // means error order occurs
                    foreach(long orderID in errorOrders.ErrorOrderID)
                    {
                        
                        Order ord = null;
                        foreach(Order ordertemp in MobileApp.GetSingletion().User.Orders)
                        {
                            if(ordertemp.IDAtDatabase == orderID)
                            {
                                ord = ordertemp;
                                ErrorOrderInformation += String.Format("OrderID:{0}   OrderBarCode:{1}", ord.IDAtDatabase, ord.BarCode)+ "\n";
                                break;
                            }
                            


                        }
                    }
                }
                MobileApp.GetSingletion().User.Orders.Clear();
                // also clear out the database
                _= await MobileApp.GetSingletion().LocalDatabase.ClearOrderActionAsync();
                _= await MobileApp.GetSingletion().LocalDatabase.ClearOrderAsync();
                ErrorMessage = "";
                IsFinishUpdate = true;

                // return true;
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
            }

           
        }
    }
}
