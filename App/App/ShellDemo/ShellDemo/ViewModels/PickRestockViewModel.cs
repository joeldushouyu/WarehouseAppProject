using ShellDemo.Models;
using ShellDemo.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using ZXing;
using Flurl;
using Flurl.Http;

namespace ShellDemo.ViewModels
{
    public class PickRestockViewModel : BaseViewModel
    {

        public string Action
        {
            get => CurrentOrderAction.Action;
        }

        public long Quantity
        {
            get => CurrentOrderAction.Quantity;
        }

        public string WorkingSection
        {
            get => CurrentOrderAction.WorkingSection();
        }

        public string OrderBarcodeID
        {
            get => CurrentOrder.BarCode.ToString();
        }

        public string WarningTextColor
        {
            get
            {
                if(this.CurrentOrder.ErrorOccurred == true)
                {
                    // change text to red
                    return "Red";

                }
                else
                {
                    return "Black";
                }
            }
        }

        public string ItemBarcode
        {
            get => CurrentOrderAction.ItemBarcode.ToString();
        }

        public string Location
        {
            get
            {
                if(this.CurrentOrderAction.Initialpick == true)
                {
                    return this.CurrentOrderAction.WorkingSection();
                }
                else
                {
                    return "Docker Area";
                }
                
            }
            

        }

        private Order CurrentOrder;
        private OrderAction CurrentOrderAction;

        private string _userAction;
        public string UserAction
        {
            get => _userAction;
            set
            {
                var data = value;
               
                SetProperty(ref _userAction, value);
                OnPropertyChanged(nameof(CanSave));
                OnPropertyChanged(nameof(ErrorMessage));
                
            }
        }

        

        private string _quantityByUser;
        public string QuantityByUser
        {
            get => _quantityByUser;
            set
            {
                var data = value;
              
                SetProperty(ref _quantityByUser, value);
                OnPropertyChanged(nameof(CanSave));
                OnPropertyChanged(nameof(ErrorMessage));



            }
        }

        private string _itemBarcodeByUser;
        public string ItemBarcodeByUser
        {
            get => _itemBarcodeByUser;
            set
            {
              
                SetProperty(ref _itemBarcodeByUser, value);
                OnPropertyChanged(nameof(CanSave));
                OnPropertyChanged(nameof(ErrorMessage));
                IsScanning = false;
                
            }
        }

        public bool Scanning => IsScanning;
        private bool _isScanning;
        public bool IsScanning
        {
            get => _isScanning;
            set
            {
                SetProperty(ref _isScanning, value);
                OnPropertyChanged(nameof(Scanning));
            }
        }

        private string _errorMessage="";
        public string ErrorMessage
        {
            get => _errorMessage;

        }

        private bool _canSave = false;

        public bool CanSave
        {
            get
            {
                _canSave = true;
                _errorMessage = "";

                // barcode
                try
                {
                    long numb = long.Parse(this.ItemBarcode);
                    if (numb == 0 || numb != long.Parse(this.ItemBarcodeByUser))
                    {
                        throw new Exception("");
                    }
                    _errorMessage += "";
                    
                }
                catch (Exception e)
                {
                    _errorMessage += "Not valid Barcode ID\n";
                    _canSave = false;
                }

                //quantity

                try
                {
                    if (Int32.Parse(this.QuantityByUser) == this.Quantity)
                    {
                        _errorMessage += "";
                        
                    }
                    else
                    {
                        throw new Exception();
                    }

                }
                catch (Exception e)
                {
                    _errorMessage += "Not valid or correct quantity input\n";
                     _canSave = false;
                }
                //action
                if (this.UserAction == this.Action)
                {
                    _errorMessage += "";
                 
                }
                else
                {
                    _errorMessage += "Incorrect action performed\n";
                    _canSave = false;
                }

                return _canSave;
            }
        }

        public PickRestockViewModel(Order currentOrder, OrderAction currentOrderAction)
        {
            this.CurrentOrder = currentOrder;
            this.CurrentOrderAction = currentOrderAction;
            SaveCommand = new Command(OnSave);
            CancelCommand = new Command(OnCancel);
            ScanningCommand = new Command(OnScanning);
            this.PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();
        }





        
        public Command SaveCommand { get; }
        public Command CancelCommand { get; }
        public Command ScanningCommand { get; }
        


        private async void OnCancel()
        {
            await Shell.Current.Navigation.PopModalAsync();
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
            catch(FlurlHttpTimeoutException e)
            {
                throw e;
           
            }catch(FlurlHttpException e)
            {
                
                int statusCode = (int)((Flurl.Http.FlurlHttpException)e).StatusCode;
                if(statusCode == 404)
                {

                    try
                    {
                        // it could be that the server could have already execute the command
                        string url = MobileApp.GetSingletion().BaseUrl + "/readyToLogout";
                        var respond = url.PostJsonAsync(userSess).Result;
                        // if still here, means no exception occur and got 200 code
                    }catch(FlurlHttpException es)
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

        public async void OnSave()
        {


            // await DataStore.AddItemAsync(newItem);

            //await Shell.Current.Navigation.PopModalAsync();
            /*
            this.CurrentOrderAction.Completed = true;
            int nextOrderActionIndex = this.CurrentOrder.currentOrderActionIndex();
            if(nextOrderActionIndex == -1)
            {
                // means complete
            }
            else
            {
                /*var page = new PickRestockPage(new PickRestockViewModel(this.CurrentOrder, this.CurrentOrder.OrderActions[this.CurrentOrder.currentOrderActionIndex()]));
                _ = Shell.Current.Navigation.PushAsync(page);
                Shell.Current.Navigation.PopAsync();

                //get the page before this page, which should be the OrderGettedListPage;
                OrderGettedListPage page = Shell.Current.Navigation.NavigationStack[Shell.Current.Navigation.NavigationStack.Count - 1] as OrderGettedListPage;
                OrderGettedListViewModel viewModel = page.BindingContext as OrderGettedListViewModel;
                viewModel.StartPicking();


            }*/

            this.CurrentOrderAction.Completed = true;

            // update the database
            _ = await MobileApp.GetSingletion().LocalDatabase.UpdateOrderActionAsync(this.CurrentOrderAction);


            this.CurrentOrderAction.Initialpick = true;


            List<OrderAction> sortedActions = MobileApp.GetSingletion().User.SortedOrderActions;
            if(sortedActions.Count == 0)
            {
                // means all done

                var page = new UpdateOrderPage(new UpdaterOrderViewModel());
                _ = Shell.Current.Navigation.PushAsync(page);
            }
            else
            {
                // still have orderActions
                OrderAction nextOrdAct = sortedActions[0];
                sortedActions.Remove(nextOrdAct);
                var correspondOrder = MobileApp.GetSingletion().User.Orders.Find((Order ord) => ord.IDAtDatabase == nextOrdAct.IDAtDatabase);
                var page = new PickRestockPage(new PickRestockViewModel(correspondOrder, nextOrdAct));
                _ = Shell.Current.Navigation.PushAsync(page);
            }
            
            

        }


        private async Task<bool> UpdateOrder()
        {
            try
            {
                string updateUrl = MobileApp.GetSingletion().BaseUrl + "/updateOrder";
                var respond = await updateUrl.WithTimeout(20).PostJsonAsync(new UpdateOrderRequest { Session = MobileApp.GetSingletion().User.CurrentSessionUUID,
                                                                                                       Orders = MobileApp.GetSingletion().User.Orders});
                Console.WriteLine("success");
                return true;
            }
            catch(FlurlHttpTimeoutException e)
            {
                return false;
            }catch(FlurlHttpException e)
            {
                return false;
                
            }catch(Exception e)
            {
                return false;
            }
        }

        public void OnPicker_SelectedIndexChanged(object sender)
        {
            var picker = (Picker)sender;
            int selectedIndex = picker.SelectedIndex;

            if (selectedIndex != -1)
            {
                UserAction = picker.Items[selectedIndex];
            }
        }


        public void OnScanning()
        {
            IsScanning = true;

            //ZXingScannerView_OnScan();

        }
        /*
        private bool valideBarCode(string newBarCode)
        {
            try
            {
                int numb = Int32.Parse(newBarCode);
                if (numb == 0 || numb != Int32.Parse(this.ItemBarcode))
                {
                    throw new Exception("");
                }
                ErrorMessage += "";
                return true;
            }
            catch (Exception e)
            {
                ErrorMessage += "Not valid Barcode ID\n";
                return false;
            }
        }
        private bool validateQuatity(string quant)
        {
            try
            {   
                if(Int32.Parse(quant) == this.Quantity)
                {
                    ErrorMessage += "";
                    return true;
                }
                else
                {
                    throw new Exception();
                }
                
            }catch(Exception e)
            {
                ErrorMessage += "Not valid or correct quantity input\n";
                return false;
            }
        }
        private bool validateAction(string act)
        {
            if (act == this.Action)
            {
                ErrorMessage += "";
                return true;
            }
            else
            {
                ErrorMessage += "Incorrect action performed\n";
                return false;
            }
        }*/
    }
}
