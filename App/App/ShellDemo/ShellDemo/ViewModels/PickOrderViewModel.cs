using Flurl.Http;
using ShellDemo.Models;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
namespace ShellDemo.ViewModels
{

    public class PickOrderViewModel : BaseViewModel
    {
        private Order ord;
        public ZXing.Net.Mobile.Forms.ZXingScannerView scanner;

        private bool needConfirmation = false;
        private List<OrderAction> respond;

        private string _barCodeid;

        public string BarCodeID
        {
            get
            {
                return _barCodeid;
            }
            set
            {
                string newBarcode = value;
                if (valideBarCode(value))
                {
                    SetProperty(ref _barCodeid, value);
                    OnPropertyChanged(nameof(ErrorBarCodeMessage));
                    IsScanning = false;
                }
                else
                {
                    OnPropertyChanged(nameof(ErrorBarCodeMessage));
                }

            }
        }

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

        private bool _isTyping;
        public bool IsTyping
        {
            get => _isScanning;
            set
            {
                SetProperty(ref _isTyping, value);
                OnPropertyChanged(nameof(IsTyping));
            }
        }

        public bool Scanning => IsScanning;


        public bool ErrorOccurred => _errorBarCodeMessage.Length != 0;
        private string _errorBarCodeMessage;
        public string ErrorBarCodeMessage
        {
            get => _errorBarCodeMessage;
            set
            {
                SetProperty(ref _errorBarCodeMessage, value);
                OnPropertyChanged(nameof(ErrorBarCodeMessage));
                OnPropertyChanged(nameof(CanGetOrder));
            }
        }

        private string _errorGetOrderMessage;
        public string ErrorGetOrderMessage
        {
            get => _errorGetOrderMessage;
            set
            {
                SetProperty(ref _errorGetOrderMessage, value);
                OnPropertyChanged(nameof(ErrorGetOrderMessage));
            }
        }
        public long OrderID => this.ord.IDAtDatabase;

        public bool HaveBarcodeID => ord.BarCode != 0;
        public bool CanChangeBarcodeID => !(HaveBarcodeID);

        public bool CanGetOrder => ErrorBarCodeMessage.Length == 0;
        public Command ScanCommand { get; }
        public Command PickOrderCommand { get; }
        public PickOrderViewModel(Order ord)
        {
            this.ord = ord;
            this._barCodeid = ord.BarCode.ToString(); // if no barcode being assinged, it will default be 0
            ScanCommand = new Command(OnScanning);
            PickOrderCommand = new Command(PickOrder);


        }

        /// <summary>
        /// Send request to server to confirm Pick Order, throw Execption when error occurs
        /// </summary>
        private async void ConfirmGetOrder()
        {
            try
            {   
                await Services.ServerRequest.ConfirmPickOrderRequest(MobileApp.GetSingletion().User.CurrentSessionUUID);
                // means user successfully Get the order
                needConfirmation = false;
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        private async void PickOrder()
        {
            try
            {


                if (needConfirmation == false)
                {
                    respond = await Services .ServerRequest.PickOrderRequest(this.ord, this._barCodeid, MobileApp.GetSingletion().User.CurrentSessionUUID);
                    needConfirmation = true;

                    foreach (OrderAction orda in respond)
                    {
                        ord.OrderActions.Add(orda);
                    }

                }


                // got and send a confirm request


                ConfirmGetOrder();
                // after server successfully receive the request
                MobileApp.GetSingletion().User.Orders.Add(ord);
                _ = await MobileApp.GetSingletion().LocalDatabase.SaveOrderAsync(ord);// save to database

                foreach(OrderAction ordAct in ord.OrderActions)
                {
                    _ = await MobileApp.GetSingletion().LocalDatabase.SaveOrderActionAsync(ordAct);
                }


                //push back to orderlist page
                // remove detail page,
                //https://stackoverflow.com/questions/24856116/how-to-popasync-more-than-1-page-in-xamarin-forms-navigation
                for (var counter = 1; counter < 2; counter++)
                {
                    Shell.Current.Navigation.RemovePage(Shell.Current.Navigation.NavigationStack[Shell.Current.Navigation.NavigationStack.Count - 2]);
                }
                await Shell.Current.Navigation.PopAsync();

            }
                catch (Exception e)
            {
                ErrorGetOrderMessage = e.Message;
                
            }
        }

        public void OnScanning()
        {
            IsScanning = true;


        }
        public void ZXingScannerView_OnScan()
        {
            if (valideBarCode(scanner.Result.Text))
            {
                // if is valid barcode id
                ErrorBarCodeMessage = "";
                _barCodeid = scanner.Result.Text;
            }
            else
            {
                ErrorBarCodeMessage = "Not valid Barcode ID";
                // not valid id
            }
        }

        /// <summary>
        /// Verify wheter the passing barcode is valid or not
        /// </summary>
        /// <param name="newBarCode"></param>
        /// <returns></returns>
        private bool valideBarCode(string newBarCode)
        {
            try
            {
                int numb = Int32.Parse(newBarCode);
                if (numb == 0)
                {
                    throw new Exception("");
                }
                ErrorBarCodeMessage = "";
                return true;
            }
            catch (Exception e)
            {
                ErrorBarCodeMessage = "Not valid Barcode ID";
                return false;
            }
        }
    }
}
