using ShellDemo.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using ZXing;

namespace ShellDemo.ViewModels
{
    public class PickRestockViewModel : BaseViewModel
    {
       
        public string Action
        {
            get => CurrentOrderAction.Action;
        }

        public int Quantity
        {
            get => CurrentOrderAction.Quantity;
        }

        public string WorkingSection
        {
            get => CurrentOrderAction.WorkingSection();
        }

        public string OrderBarcodeID
        {
            get => CurrentOrder.barCode.ToString();
        }

        public string ItemBarcode
        {
            get => CurrentOrderAction.ItemBarcode.ToString();
        }

        private Order CurrentOrder;
        private OrderAction CurrentOrderAction;
        public ZXing.Net.Mobile.Forms.ZXingScannerView scanner;
        private string _userAction;
        public string UserAction
        {
            get => _userAction;
            set
            {
                _userAction = value;
            }
        }

        

        private string _quantityByUser;
        public string QuantityByUser
        {
            get => _quantityByUser;
            set
            {   
                _quantityByUser = value;
            }
        }

        private string _itemBarcodeByUser;
        public string ItemBarcodeByUser
        {
            get => _itemBarcodeByUser;
            set
            {
                string newBarcode = value;
                if (valideBarCode(value))
                {
                    SetProperty(ref _itemBarcodeByUser, value);
                    OnPropertyChanged(nameof(ErrorMessage));
                    IsScanning = false;
                }
                else
                {
                    OnPropertyChanged(nameof(ErrorMessage));
                }

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

        private string _errorMessage;
        public string ErrorMessage
        {
            get => _errorMessage;
            set {
                SetProperty(ref _errorMessage, value);
                OnPropertyChanged(nameof(CanSave));
            }
        }


        public bool CanSave => ValidateSave();

        public PickRestockViewModel(Order currentOrder, OrderAction currentOrderAction)
        {
            this.CurrentOrder = currentOrder; 
            this.CurrentOrderAction = currentOrderAction;
            SaveCommand = new Command(OnSave, ValidateSave);
            CancelCommand = new Command(OnCancel);
            this.PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();
        }





        private bool ValidateSave()
        {
            ErrorMessage = "";
            return validateQuatity(this.QuantityByUser) && valideBarCode(this._itemBarcodeByUser) && validateAction(this.UserAction);
     

        }

        
        public Command SaveCommand { get; }
        public Command CancelCommand { get; }

        private async void OnCancel()
        {
            await Shell.Current.Navigation.PopModalAsync();
        }

        private async void OnSave()
        {
           

           // await DataStore.AddItemAsync(newItem);

            await Shell.Current.Navigation.PopModalAsync();
        }

        public void OnPickerSelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = (Picker)sender;
            int selectedIndex = picker.SelectedIndex;

            if (selectedIndex != -1)
            {
                UserAction = picker.Items[selectedIndex];
            }
        }

        public void ZXingScannerView_OnScan()
        {
            valideBarCode(scanner.Result.Text);
          
        }

        private bool valideBarCode(string newBarCode)
        {
            try
            {
                int numb = Int32.Parse(newBarCode);
                if (numb == 0)
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
                ErrorMessage += "";
                return true;
            }catch(Exception e)
            {
                ErrorMessage += "Not valid quantity input\n";
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
        }
    }
}
