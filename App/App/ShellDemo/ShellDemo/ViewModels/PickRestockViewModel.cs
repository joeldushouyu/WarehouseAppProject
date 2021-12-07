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


        /// <summary>
        /// The action perform when user click the nextbutton, the function will update the currentOrderAction to the database, indicate that the progress has been completed.
        /// After that, if there is no more action to perform, it direct user to UpdateOrderPage
        /// Else it direct user to PickRestockPage with the next correspond orderAction
        /// </summary>
        public async void OnSave()
        {



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
   
    }
}
